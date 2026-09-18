using Autogara.DataAccess.Vederi;
using Autogara.Domain.Entitati;
using Autogara.Domain.Enumerari;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Autogara.DataAccess;

/// <summary>
/// Maparea 1:1 pe tabelele din 03_Tables.sql. Schema bazei de date se creeaza doar din
/// scripturile SQL — aici nu exista migrari EF.
/// </summary>
public class AutogaraDbContext(DbContextOptions<AutogaraDbContext> options) : DbContext(options)
{
    public DbSet<Rol> Roluri => Set<Rol>();
    public DbSet<Utilizator> Utilizatori => Set<Utilizator>();
    public DbSet<Nod> Noduri => Set<Nod>();
    public DbSet<Conexiune> Conexiuni => Set<Conexiune>();
    public DbSet<Statie> Statii => Set<Statie>();
    public DbSet<Autobuz> Autobuze => Set<Autobuz>();
    public DbSet<MentenantaAutobuz> MentenantaAutobuze => Set<MentenantaAutobuz>();
    public DbSet<Sofer> Soferi => Set<Sofer>();
    public DbSet<Traseu> Trasee => Set<Traseu>();
    public DbSet<TraseuOprire> TraseuOpriri => Set<TraseuOprire>();
    public DbSet<TipReducere> TipuriReducere => Set<TipReducere>();
    public DbSet<Cursa> Curse => Set<Cursa>();
    public DbSet<Loc> Locuri => Set<Loc>();
    public DbSet<RezervareProvizorie> RezervariProvizorii => Set<RezervareProvizorie>();
    public DbSet<Bilet> Bilete => Set<Bilet>();
    public DbSet<Plata> Plati => Set<Plata>();
    public DbSet<LogAudit> LogAudit => Set<LogAudit>();

    public DbSet<CursaActivaRand> CurseActive => Set<CursaActivaRand>();
    public DbSet<LocDisponibilRand> LocuriDisponibile => Set<LocDisponibilRand>();
    public DbSet<VanzareZilnicaRand> VanzariZilnice => Set<VanzareZilnicaRand>();
    public DbSet<OcupareCursaRand> OcupareCurse => Set<OcupareCursaRand>();
    public DbSet<RaportComparativRand> RapoarteComparative => Set<RaportComparativRand>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("autogara");

        mb.Entity<Rol>(e =>
        {
            e.ToTable("Roluri");
            e.HasKey(x => x.RolID);
            e.Property(x => x.Denumire).HasMaxLength(50);
        });

        mb.Entity<Utilizator>(e =>
        {
            e.ToTable("Utilizatori");
            e.HasKey(x => x.UtilizatorID);
            e.Property(x => x.NumeUtilizator).HasMaxLength(50);
            e.Property(x => x.ParolaHash).HasMaxLength(256);
            e.Property(x => x.Nume).HasMaxLength(100);
            e.Property(x => x.Prenume).HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(150);
            e.Property(x => x.Telefon).HasMaxLength(20);
            Audit(e);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.RolID);
            e.Ignore(x => x.NumeComplet);
        });

        mb.Entity<Nod>(e =>
        {
            e.ToTable("Noduri");
            e.HasKey(x => x.NodID);
            e.Property(x => x.Tip).HasMaxLength(20).HasConversion(Conversie<TipNod>());
            e.Property(x => x.Nume).HasMaxLength(100);
            Audit(e);
            e.HasOne(x => x.Statie).WithOne(s => s.Nod).HasForeignKey<Statie>(s => s.NodID);
        });

        mb.Entity<Conexiune>(e =>
        {
            e.ToTable("Conexiuni");
            e.HasKey(x => x.ConexiuneID);
            e.Property(x => x.DistantaKm).HasPrecision(6, 2);
            e.Property(x => x.CreatLa).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(x => x.NodPlecare).WithMany().HasForeignKey(x => x.NodPlecareID).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.NodSosire).WithMany().HasForeignKey(x => x.NodSosireID).OnDelete(DeleteBehavior.NoAction);
        });

        mb.Entity<Statie>(e =>
        {
            e.ToTable("Statii");
            e.HasKey(x => x.StatieID);
            e.Property(x => x.Adresa).HasMaxLength(200);
            e.Property(x => x.Peron).HasMaxLength(10);
            Audit(e);
        });

        mb.Entity<Autobuz>(e =>
        {
            e.ToTable("Autobuze");
            e.HasKey(x => x.AutobuzID);
            e.Property(x => x.NrInmatriculare).HasMaxLength(15);
            e.Property(x => x.Model).HasMaxLength(100);
            e.Property(x => x.Status).HasMaxLength(20).HasConversion(Conversie<StatusAutobuz>());
            e.Property(x => x.CaleFisierJSON).HasMaxLength(260);
            Audit(e);
        });

        mb.Entity<MentenantaAutobuz>(e =>
        {
            e.ToTable("MentenantaAutobuze");
            e.HasKey(x => x.MentenantaID);
            e.Property(x => x.TipLucrare).HasMaxLength(100);
            e.Property(x => x.Observatii).HasMaxLength(500);
            e.Property(x => x.CreatLa).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(x => x.Autobuz).WithMany().HasForeignKey(x => x.AutobuzID);
        });

        mb.Entity<Sofer>(e =>
        {
            e.ToTable("Soferi");
            e.HasKey(x => x.SoferID);
            e.Property(x => x.Nume).HasMaxLength(100);
            e.Property(x => x.Prenume).HasMaxLength(100);
            e.Property(x => x.NrPermis).HasMaxLength(20);
            e.Property(x => x.Telefon).HasMaxLength(20);
            Audit(e);
            e.Ignore(x => x.NumeComplet);
        });

        mb.Entity<Traseu>(e =>
        {
            e.ToTable("Trasee");
            e.HasKey(x => x.TraseuID);
            e.Property(x => x.Denumire).HasMaxLength(150);
            Audit(e);
            e.HasMany(x => x.Opriri).WithOne(o => o.Traseu).HasForeignKey(o => o.TraseuID);
        });

        mb.Entity<TraseuOprire>(e =>
        {
            e.ToTable("TraseuOpriri");
            e.HasKey(x => x.TraseuOprireID);
            e.HasOne(x => x.Statie).WithMany().HasForeignKey(x => x.StatieID);
        });

        mb.Entity<TipReducere>(e =>
        {
            e.ToTable("TipuriReducere");
            e.HasKey(x => x.TipReducereID);
            e.Property(x => x.Denumire).HasMaxLength(50);
            e.Property(x => x.ProcentReducere).HasPrecision(5, 2);
        });

        mb.Entity<Cursa>(e =>
        {
            e.ToTable("Curse");
            e.HasKey(x => x.CursaID);
            e.Property(x => x.OraPlecare).HasColumnType("time(0)");
            e.Property(x => x.OraSosireEstimata).HasColumnType("time(0)");
            e.Property(x => x.Pret).HasPrecision(10, 2);
            e.Property(x => x.Status).HasMaxLength(20).HasConversion(Conversie<StatusCursa>());
            Audit(e);
            e.HasOne(x => x.Traseu).WithMany().HasForeignKey(x => x.TraseuID);
            e.HasOne(x => x.Autobuz).WithMany().HasForeignKey(x => x.AutobuzID);
            e.HasOne(x => x.Sofer).WithMany().HasForeignKey(x => x.SoferID);
            e.HasMany(x => x.Locuri).WithOne(l => l.Cursa).HasForeignKey(l => l.CursaID);
            e.Ignore(x => x.MomentPlecare);
        });

        mb.Entity<Loc>(e =>
        {
            e.ToTable("Locuri");
            e.HasKey(x => x.LocID);
            e.HasAlternateKey(x => new { x.LocID, x.CursaID });
            e.Property(x => x.Status).HasMaxLength(20).HasConversion(Conversie<StatusLoc>());
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        mb.Entity<RezervareProvizorie>(e =>
        {
            e.ToTable("RezervariProvizorii");
            e.HasKey(x => x.RezervareID);
            e.Property(x => x.DataCreare).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(x => x.Loc).WithMany().HasForeignKey(x => x.LocID);
        });

        mb.Entity<Bilet>(e =>
        {
            e.ToTable("Bilete");
            e.HasKey(x => x.BiletID);
            e.Property(x => x.CodBilet).HasMaxLength(20);
            e.Property(x => x.NumePasager).HasMaxLength(200);
            e.Property(x => x.TelefonPasager).HasMaxLength(20);
            e.Property(x => x.Pret).HasPrecision(10, 2);
            e.Property(x => x.Status).HasMaxLength(20).HasConversion(Conversie<StatusBilet>());
            e.Property(x => x.DataEmitere).HasDefaultValueSql("SYSUTCDATETIME()");
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasOne(x => x.Cursa).WithMany().HasForeignKey(x => x.CursaID).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Loc).WithMany()
                .HasForeignKey(x => new { x.LocID, x.CursaID })
                .HasPrincipalKey(l => new { l.LocID, l.CursaID })
                .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.TipReducere).WithMany().HasForeignKey(x => x.TipReducereID);
            e.HasOne(x => x.VanzutDe).WithMany().HasForeignKey(x => x.VanzutDeUtilizatorID);
            e.HasMany(x => x.Plati).WithOne().HasForeignKey(p => p.BiletID);
        });

        mb.Entity<Plata>(e =>
        {
            e.ToTable("Plati");
            e.HasKey(x => x.PlataID);
            e.Property(x => x.Suma).HasPrecision(10, 2);
            e.Property(x => x.MetodaPlata).HasMaxLength(10).HasConversion(Conversie<MetodaPlata>());
            e.Property(x => x.Status).HasMaxLength(20).HasConversion(Conversie<StatusPlata>());
            e.Property(x => x.NumarBonFiscal).HasMaxLength(50);
            e.Property(x => x.DataPlata).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        mb.Entity<LogAudit>(e =>
        {
            e.ToTable("LogAudit");
            e.HasKey(x => x.LogID);
            e.Property(x => x.Actiune).HasMaxLength(200);
            e.Property(x => x.Entitate).HasMaxLength(100);
            e.Property(x => x.EntitateID).HasMaxLength(50);
            e.Property(x => x.DataOra).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        mb.Entity<CursaActivaRand>().HasNoKey().ToView("vw_CurseActive");
        mb.Entity<LocDisponibilRand>().HasNoKey().ToView("vw_LocuriDisponibile");
        mb.Entity<VanzareZilnicaRand>().HasNoKey().ToView("vw_VanzariZilnice");
        mb.Entity<OcupareCursaRand>().HasNoKey().ToView("vw_OcupareCurse");
        mb.Entity<RaportComparativRand>().HasNoKey().ToView("vw_RapoarteComparative");

        foreach (var p in mb.Model.GetEntityTypes().SelectMany(t => t.GetProperties()))
        {
            if (p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?))
                p.SetColumnType("datetime2(3)");
            else if ((p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)) && p.GetPrecision() is null)
                p.SetColumnType("decimal(38, 2)"); // coloanele calculate din view-uri; doar se citesc
        }
    }

    /// <summary>CreatLa are default SYSUTCDATETIME() in SQL; ModificatLa se completeaza din cod.</summary>
    private static void Audit<T>(EntityTypeBuilder<T> e) where T : class =>
        e.Property<DateTime>("CreatLa").HasDefaultValueSql("SYSUTCDATETIME()");

    private static ValueConverter<T, string> Conversie<T>() where T : struct, Enum =>
        new(v => ValoriDb.ToDb(v), s => ValoriDb.FromDb<T>(s));

    /// <summary>
    /// Completeaza ModificatLa (UTC) pe entitatile modificate care au coloana, ca SQL-ul
    /// sa nu depinda de disciplina fiecarui serviciu.
    /// </summary>
    public override Task<int> SaveChangesAsync(bool acceptAll, CancellationToken ct = default)
    {
        var acum = DateTime.UtcNow;
        foreach (var intrare in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
        {
            var prop = intrare.Metadata.FindProperty("ModificatLa");
            if (prop is not null)
                intrare.Property("ModificatLa").CurrentValue = acum;
        }

        return base.SaveChangesAsync(acceptAll, ct);
    }
}
