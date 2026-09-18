using Autogara.Business.Dto;
using Autogara.Common;
using Autogara.DataAccess;
using Autogara.FileServer;
using Serilog;

namespace Autogara.Business.Servicii;

/// <summary>
/// Verifica periodic serverul SQL si file server-ul. Evenimentul
/// <see cref="ConexiuneSchimbata"/> vine pe un thread din pool: in WinForms se foloseste
/// Control.BeginInvoke inainte de a atinge controalele.
/// </summary>
public sealed class ConexiuneStareService(BazaDeDate bd, SftpFileClient fileServer, int secundeInterval = 30) : IDisposable
{
    private readonly SemaphoreSlim _verificare = new(1, 1);
    private PeriodicTimer? _timer;
    private CancellationTokenSource? _oprire;

    public StareConexiune? StareCurenta { get; private set; }

    public event EventHandler<StareConexiune>? ConexiuneSchimbata;

    public void Porneste()
    {
        if (_timer is not null)
            return;

        _oprire = new CancellationTokenSource();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(5, secundeInterval)));
        _ = BuclaAsync(_timer, _oprire.Token);
    }

    public void Opreste()
    {
        _oprire?.Cancel();
        _timer?.Dispose();
        _timer = null;
    }

    public async Task<StareConexiune> VerificaAcumAsync(CancellationToken ct = default)
    {
        await _verificare.WaitAsync(ct);
        try
        {
            var sql = bd.EsteDisponibilaAsync(ct);
            var sftp = FileServerDisponibilAsync(ct);
            var stare = new StareConexiune(await sql, await sftp, OraLocala.Acum);

            var anterioara = StareCurenta;
            StareCurenta = stare;
            if (anterioara is null || anterioara.BazaDeDate != stare.BazaDeDate || anterioara.FileServer != stare.FileServer)
            {
                Log.Information("Stare conexiune: SQL {Sql}, file server {Sftp}", stare.BazaDeDate ? "OK" : "indisponibil",
                    stare.FileServer ? "OK" : "indisponibil");
                ConexiuneSchimbata?.Invoke(this, stare);
            }

            return stare;
        }
        finally
        {
            _verificare.Release();
        }
    }

    private async Task BuclaAsync(PeriodicTimer timer, CancellationToken ct)
    {
        try
        {
            await VerificaAcumAsync(ct);
            while (await timer.WaitForNextTickAsync(ct))
                await VerificaAcumAsync(ct);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Verificarea periodică a conexiunii s-a oprit");
        }
    }

    private async Task<bool> FileServerDisponibilAsync(CancellationToken ct)
    {
        try
        {
            return await fileServer.TesteazaConexiuneAsync(ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return false;
        }
    }

    public void Dispose()
    {
        Opreste();
        _oprire?.Dispose();
    }
}
