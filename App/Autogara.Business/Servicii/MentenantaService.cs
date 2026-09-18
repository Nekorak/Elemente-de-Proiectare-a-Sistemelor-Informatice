using Autogara.Business.Dto;
using Autogara.Business.Reguli;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.Domain.Entitati;
using Microsoft.EntityFrameworkCore;

namespace Autogara.Business.Servicii;

public sealed class MentenantaService(BazaDeDate bd, Sesiune sesiune)
{
    public Task<List<MentenantaRand>> ListeazaAsync(int? autobuzId = null)
    {
        sesiune.CereAdmin();

        return bd.CitesteAsync(db =>
            db.MentenantaAutobuze
                .Where(m => autobuzId == null || m.AutobuzID == autobuzId)
                .OrderByDescending(m => m.Data).ThenByDescending(m => m.MentenantaID)
                .Select(m => new MentenantaRand(m.MentenantaID, m.AutobuzID, m.Autobuz!.NrInmatriculare, m.TipLucrare,
                    m.Data, m.Kilometraj, m.Observatii))
                .ToListAsync());
    }

    public Task<int> AdaugaInregistrareAsync(MentenantaEditare date)
    {
        var admin = sesiune.CereAdmin();

        var erori = new List<string>();
        Validari.Obligatoriu(erori, date.TipLucrare, "Tipul lucrării", 100);
        Validari.Optional(erori, date.Observatii, "Observațiile", 500);
        if (date.Kilometraj is < 0) erori.Add("Kilometrajul nu poate fi negativ.");
        if (date.Data > OraLocala.Azi) erori.Add("Data lucrării nu poate fi în viitor.");
        ValidareException.AruncaDaca(erori);

        return bd.ScrieAsync(async db =>
        {
            var a = await db.Autobuze.FirstOrDefaultAsync(x => x.AutobuzID == date.AutobuzID)
                    ?? throw new NegasitException("Autobuzul nu există.");

            var ultimKm = await db.MentenantaAutobuze.Where(m => m.AutobuzID == date.AutobuzID && m.Data <= date.Data)
                .MaxAsync(m => m.Kilometraj);
            if (date.Kilometraj is { } km && ultimKm is { } ultim && km < ultim)
                throw new ValidareException($"Kilometrajul ({km:N0}) este mai mic decât ultimul înregistrat ({ultim:N0}).");

            var m = new MentenantaAutobuz
            {
                AutobuzID = date.AutobuzID,
                TipLucrare = date.TipLucrare.Trim(),
                Data = date.Data,
                Kilometraj = date.Kilometraj,
                Observatii = Validari.Curata(date.Observatii),
            };
            db.MentenantaAutobuze.Add(m);
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"Mentenanță {a.NrInmatriculare}: {m.TipLucrare}", "MentenantaAutobuze", m.MentenantaID.ToString());
            return m.MentenantaID;
        });
    }

    /// <summary>Autobuzele active cu ITP-ul expirat sau care expira in urmatoarele <paramref name="zileInainte"/> zile.</summary>
    public Task<List<ItpRand>> AutobuzeCuItpApropiatAsync(int zileInainte = 30)
    {
        sesiune.CerePersonal();
        var azi = OraLocala.Azi;
        var limita = azi.AddDays(zileInainte);

        return bd.CitesteAsync(async db =>
        {
            var randuri = await db.Autobuze
                .Where(a => a.Activ && (a.DataExpirareITP == null || a.DataExpirareITP <= limita))
                .OrderBy(a => a.DataExpirareITP)
                .Select(a => new { a.AutobuzID, a.NrInmatriculare, a.Model, a.Status, a.DataExpirareITP })
                .ToListAsync();

            return randuri.Select(a => new ItpRand(a.AutobuzID, a.NrInmatriculare, a.Model, a.Status, a.DataExpirareITP,
                a.DataExpirareITP is { } d ? d.DayNumber - azi.DayNumber : null)).ToList();
        });
    }

    /// <summary>ITP nou: actualizeaza data de expirare si adauga inregistrarea in istoricul de mentenanta.</summary>
    public Task InregistreazaItpAsync(int autobuzId, DateOnly dataInspectie, DateOnly dataExpirareNoua, int? kilometraj)
    {
        var admin = sesiune.CereAdmin();
        if (dataExpirareNoua <= dataInspectie)
            throw new ValidareException("Data de expirare trebuie să fie după data inspecției.");

        return bd.InTranzactieAsync(async db =>
        {
            var a = await db.Autobuze.FirstOrDefaultAsync(x => x.AutobuzID == autobuzId)
                    ?? throw new NegasitException("Autobuzul nu există.");

            a.DataExpirareITP = dataExpirareNoua;
            db.MentenantaAutobuze.Add(new MentenantaAutobuz
            {
                AutobuzID = autobuzId,
                TipLucrare = "Inspecție tehnică periodică (ITP)",
                Data = dataInspectie,
                Kilometraj = kilometraj,
                Observatii = $"Valabil până la {dataExpirareNoua:dd.MM.yyyy}",
            });
            await db.SaveChangesAsync();
            await db.InregistreazaLogAuditAsync(admin.UtilizatorID, $"ITP {a.NrInmatriculare} până la {dataExpirareNoua:dd.MM.yyyy}", "Autobuze", a.AutobuzID.ToString());
        });
    }
}
