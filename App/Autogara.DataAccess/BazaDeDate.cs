using Autogara.Common.Configurare;
using Microsoft.EntityFrameworkCore;

namespace Autogara.DataAccess;

/// <summary>
/// Punctul unic de acces la SQL Server pentru serviciile din Business. Fiecare operatie
/// primeste un DbContext nou (scurt, ca intr-o aplicatie desktop cu mai multe ferestre deschise)
/// si orice eroare iese deja tradusa de <see cref="EroriSql"/>.
/// </summary>
public sealed class BazaDeDate
{
    private readonly DbContextOptions<AutogaraDbContext> _optiuni;

    public BazaDeDate(string connectionString)
    {
        _optiuni = new DbContextOptionsBuilder<AutogaraDbContext>()
            .UseSqlServer(connectionString, sql =>
            {
                // Reincercare automata la erori tranzitorii de retea (conexiune prin internet).
                sql.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                sql.CommandTimeout(30);
            })
            .Options;
    }

    public BazaDeDate(AppSettings setari) : this(setari.ConnectionString()) { }

    public BazaDeDate(DbContextOptions<AutogaraDbContext> optiuni) => _optiuni = optiuni;

    public AutogaraDbContext CreeazaContext() => new(_optiuni);

    /// <summary>Citire: context fara tracking.</summary>
    public async Task<T> CitesteAsync<T>(Func<AutogaraDbContext, Task<T>> operatie)
    {
        await using var db = CreeazaContext();
        db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        try
        {
            return await operatie(db).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw EroriSql.Traduce(ex);
        }
    }

    /// <summary>Scriere simpla (un singur SaveChanges sau un singur apel de procedura).</summary>
    public async Task<T> ScrieAsync<T>(Func<AutogaraDbContext, Task<T>> operatie)
    {
        await using var db = CreeazaContext();
        try
        {
            return await operatie(db).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw EroriSql.Traduce(ex);
        }
    }

    public Task ScrieAsync(Func<AutogaraDbContext, Task> operatie) =>
        ScrieAsync(async db => { await operatie(db).ConfigureAwait(false); return true; });

    /// <summary>
    /// Mai multe scrieri care trebuie sa reuseasca impreuna. Tot blocul se reia de la capat
    /// daca pica o eroare tranzitorie de retea (de aceea operatia trebuie sa nu aiba efecte in afara SQL).
    /// </summary>
    public async Task<T> InTranzactieAsync<T>(Func<AutogaraDbContext, Task<T>> operatie)
    {
        await using var db = CreeazaContext();
        try
        {
            var strategie = db.Database.CreateExecutionStrategy();
            return await strategie.ExecuteAsync(async () =>
            {
                db.ChangeTracker.Clear();
                await using var tranzactie = await db.Database.BeginTransactionAsync().ConfigureAwait(false);
                var rezultat = await operatie(db).ConfigureAwait(false);
                await tranzactie.CommitAsync().ConfigureAwait(false);
                return rezultat;
            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw EroriSql.Traduce(ex);
        }
    }

    public Task InTranzactieAsync(Func<AutogaraDbContext, Task> operatie) =>
        InTranzactieAsync(async db => { await operatie(db).ConfigureAwait(false); return true; });

    /// <summary>Verificare rapida pentru indicatorul online/offline si wizard-ul de configurare.</summary>
    public async Task<bool> EsteDisponibilaAsync(CancellationToken ct = default)
    {
        try
        {
            await using var db = CreeazaContext();
            return await db.Database.CanConnectAsync(ct).ConfigureAwait(false);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Ca <see cref="EsteDisponibilaAsync"/>, dar arunca eroarea tradusa (pentru mesajul din wizard).</summary>
    public async Task TesteazaConexiuneAsync(CancellationToken ct = default)
    {
        try
        {
            await using var db = CreeazaContext();
            await db.Database.OpenConnectionAsync(ct).ConfigureAwait(false);
            await db.Roluri.AsNoTracking().AnyAsync(ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw EroriSql.Traduce(ex);
        }
    }
}
