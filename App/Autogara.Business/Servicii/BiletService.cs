using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Autogara.Business.Servicii;

/// <summary>
/// Vanzarea si anularea biletelor. Pretul final, codul biletului si rambursarea le calculeaza
/// procedurile stocate, in aceeasi tranzactie cu ocuparea/eliberarea locului.
/// </summary>
public sealed class BiletService(BazaDeDate bd, RezervareService rezervari, Sesiune sesiune)
{
    /// <summary>Pretul afisat inainte de confirmare (aceeasi formula ca fn_CalculeazaPret).</summary>
    public Task<PretDto> CalculeazaPretAsync(int cursaId, int? tipReducereId)
    {
        sesiune.CereAutentificare();

        return bd.CitesteAsync(async db =>
        {
            var pretBaza = await db.Curse.Where(c => c.CursaID == cursaId).Select(c => (decimal?)c.Pret).FirstOrDefaultAsync()
                           ?? throw new NegasitException("Cursa nu există.");

            var reducere = tipReducereId is null
                ? null
                : await db.TipuriReducere.FirstOrDefaultAsync(t => t.TipReducereID == tipReducereId && t.Activ);

            var procent = reducere?.ProcentReducere ?? 0;
            return new PretDto(pretBaza, reducere?.Denumire, procent, PretCalculator.PretCuReducere(pretBaza, procent));
        });
    }

    /// <summary>Transforma rezervarea provizorie in bilet platit.</summary>
    public async Task<BiletEmis> ConfirmaVanzareAsync(Guid rezervareId, VanzareDto date)
    {
        var u = sesiune.CereAutentificare();
        ValidareException.AruncaDaca(Valideaza(date, u));

        var emis = await bd.ScrieAsync(async db =>
        {
            var (biletId, cod, pret) = await db.ConfirmaVanzareBiletAsync(
                rezervareId, date.NumePasager.Trim(), Validari.Curata(date.TelefonPasager), date.TipReducereID,
                u.UtilizatorID, date.MetodaPlata, Validari.Curata(date.NumarBonFiscal));

            var loc = await db.Bilete.Where(b => b.BiletID == biletId)
                .Select(b => new { b.CursaID, b.Loc!.NumarLoc })
                .FirstAsync();

            return new BiletEmis(biletId, cod, pret, loc.CursaID, loc.NumarLoc);
        });

        Log.Information("Bilet {Cod} vândut de {Utilizator}: {Pret} MDL", emis.CodBilet, u.NumeUtilizator, emis.Pret);
        return emis;
    }

    /// <summary>Vanzare dintr-un pas pentru ghiseu (F2): rezerva locul si confirma imediat.</summary>
    public async Task<BiletEmis> VanzareRapidaAsync(int locId, VanzareDto date)
    {
        var u = sesiune.CerePersonal();
        ValidareException.AruncaDaca(Valideaza(date, u));

        var rezervare = await rezervari.RezervaLocAsync(locId, durataMinute: 2);
        try
        {
            return await ConfirmaVanzareAsync(rezervare.RezervareID, date);
        }
        catch
        {
            try { await rezervari.ElibereazaRezervareAsync(rezervare.RezervareID); }
            catch (Exception ex) { Log.Warning(ex, "Rezervarea {Id} nu a putut fi eliberată; expiră singură", rezervare.RezervareID); }
            throw;
        }
    }

    /// <summary>Cat s-ar rambursa daca biletul s-ar anula acum (pentru confirmarea din UI).</summary>
    public Task<EstimareAnulare> EstimeazaAnulareAsync(Guid biletId)
    {
        var u = sesiune.CereAutentificare();

        return bd.CitesteAsync(async db =>
        {
            var b = await db.Bilete.Where(x => x.BiletID == biletId)
                        .Select(x => new { x.Pret, x.Status, x.VanzutDeUtilizatorID, x.Cursa!.DataCursa, x.Cursa.OraPlecare, StatusCursa = x.Cursa.Status })
                        .FirstOrDefaultAsync()
                    ?? throw new NegasitException("Biletul nu există.");

            VerificaAcces(u, b.VanzutDeUtilizatorID);

            if (b.Status != StatusBilet.Activ)
                return new EstimareAnulare(b.Pret, 0, 0, false, "Biletul a fost deja anulat sau rambursat.");

            if (b.StatusCursa == StatusCursa.Anulata)
                return new EstimareAnulare(b.Pret, 100, b.Pret, true, "Cursa a fost anulată: rambursare integrală.");

            var plecare = b.DataCursa.ToDateTime(b.OraPlecare);
            var acum = OraLocala.Acum;
            if (b.StatusCursa != StatusCursa.Planificata || plecare <= acum)
                return new EstimareAnulare(b.Pret, 0, 0, false, "Cursa a plecat; biletul nu mai poate fi anulat.");

            var procent = PretCalculator.ProcentRambursare(plecare, acum);
            var explicatie = procent switch
            {
                100 => "Cu peste 24 de ore înainte de plecare: rambursare integrală.",
                50 => "Între 2 și 24 de ore înainte de plecare: se rambursează 50%.",
                _ => "Cu mai puțin de 2 ore înainte de plecare: biletul se anulează fără rambursare.",
            };
            return new EstimareAnulare(b.Pret, procent, PretCalculator.SumaRambursata(b.Pret, procent), true, explicatie);
        });
    }

    /// <param name="rambursareIntegrala">Doar Admin: rambursare 100% indiferent de ora (ex. reclamatie).</param>
    public async Task<RezultatAnulare> AnuleazaBiletAsync(Guid biletId, bool rambursareIntegrala = false)
    {
        var u = sesiune.CereAutentificare();
        if (rambursareIntegrala && u.Rol != RolTip.Admin)
            throw new AccesInterzisException("Doar administratorul poate aproba rambursarea integrală în afara politicii.");

        var rezultat = await bd.ScrieAsync(async db =>
        {
            var b = await db.Bilete.Where(x => x.BiletID == biletId)
                        .Select(x => new { x.CodBilet, x.VanzutDeUtilizatorID })
                        .FirstOrDefaultAsync()
                    ?? throw new NegasitException("Biletul nu există.");

            VerificaAcces(u, b.VanzutDeUtilizatorID);

            var (suma, status) = await db.AnuleazaBiletAsync(biletId, u.UtilizatorID, rambursareIntegrala);
            return new RezultatAnulare(b.CodBilet, suma, status);
        });

        Log.Information("Bilet {Cod} anulat de {Utilizator}; rambursat {Suma} MDL", rezultat.CodBilet, u.NumeUtilizator, rezultat.SumaRambursata);
        return rezultat;
    }

    public Task<BiletDetalii?> CautaDupaCodAsync(string codBilet)
    {
        var u = sesiune.CereAutentificare();
        if (string.IsNullOrWhiteSpace(codBilet))
            throw new ValidareException("Introduceți codul biletului.");

        var cod = codBilet.Trim().ToUpperInvariant();
        return bd.CitesteAsync(async db =>
        {
            var q = db.Bilete.Where(b => b.CodBilet == cod);
            if (u.Rol == RolTip.Pasager)
                q = q.Where(b => b.VanzutDeUtilizatorID == u.UtilizatorID);

            return (await Detalii(q).ToListAsync()).Select(Local).FirstOrDefault();
        });
    }

    /// <summary>Biletele cumparate (Pasager) sau vandute (Casier/Admin) de utilizatorul curent.</summary>
    public Task<List<BiletDetalii>> BileteleMeleAsync(int ultimeleZile = 90)
    {
        var u = sesiune.CereAutentificare();
        var deLa = OraLocala.Azi.AddDays(-ultimeleZile);

        return bd.CitesteAsync(async db =>
            (await Detalii(db.Bilete.Where(b => b.VanzutDeUtilizatorID == u.UtilizatorID && b.Cursa!.DataCursa >= deLa)
                    .OrderByDescending(b => b.DataEmitere))
                .ToListAsync())
            .Select(Local).ToList());
    }

    public Task<List<BiletDetalii>> BileteCursaAsync(int cursaId)
    {
        sesiune.CerePersonal();

        return bd.CitesteAsync(async db =>
            (await Detalii(db.Bilete.Where(b => b.CursaID == cursaId).OrderBy(b => b.Loc!.NumarLoc))
                .ToListAsync())
            .Select(Local).ToList());
    }

    private static IQueryable<BiletDetalii> Detalii(IQueryable<Bilet> q) =>
        q.Select(b => new BiletDetalii(
            b.BiletID, b.CodBilet, b.Status, b.NumePasager, b.TelefonPasager, b.Pret,
            b.TipReducere == null ? null : b.TipReducere.Denumire,
            b.DataEmitere, // UTC aici; se converteste in Local()
            b.VanzutDe!.Prenume + " " + b.VanzutDe.Nume,
            b.CursaID, b.Cursa!.Traseu!.Denumire, b.Cursa.DataCursa, b.Cursa.OraPlecare, b.Cursa.Status,
            b.Loc!.NumarLoc, b.Cursa.Autobuz!.NrInmatriculare,
            b.Plati.Where(p => p.Status == StatusPlata.Finalizata).OrderBy(p => p.DataPlata)
                .Select(p => (MetodaPlata?)p.MetodaPlata).FirstOrDefault()));

    private static BiletDetalii Local(BiletDetalii b) => b with { DataEmitereLocal = OraLocala.DinUtc(b.DataEmitereLocal) };

    private static void VerificaAcces(UtilizatorAutentificat u, Guid vanzutDe)
    {
        if (u.Rol == RolTip.Pasager && vanzutDe != u.UtilizatorID)
            throw new AccesInterzisException("Puteți gestiona doar biletele proprii.");
    }

    private static List<string> Valideaza(VanzareDto d, UtilizatorAutentificat u)
    {
        var erori = new List<string>();
        Validari.Obligatoriu(erori, d.NumePasager, "Numele pasagerului", 200);
        Validari.Telefon(erori, d.TelefonPasager);
        Validari.Optional(erori, d.NumarBonFiscal, "Numărul bonului fiscal", 50);
        if (u.Rol == RolTip.Pasager && d.MetodaPlata != MetodaPlata.Card)
            erori.Add("Cumpărarea online se face doar cu cardul.");
        return erori;
    }
}
