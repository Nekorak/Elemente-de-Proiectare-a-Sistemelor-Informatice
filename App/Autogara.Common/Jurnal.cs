using Autogara.Common.Configurare;
using Serilog;
using Serilog.Events;

namespace Autogara.Common;

/// <summary>
/// Log local per statie: %LocalAppData%\Autogara\Logs\autogara-AAAALLZZ.log, pastrat 30 de zile.
/// Se configureaza o singura data, la pornirea aplicatiei; apoi se foloseste direct Serilog.Log.
/// </summary>
public static class Jurnal
{
    public static void Configureaza(SetariAplicatie? setari = null, string? folder = null)
    {
        var nivel = Enum.TryParse<LogEventLevel>(setari?.NivelLog, ignoreCase: true, out var n) ? n : LogEventLevel.Information;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(nivel)
            .Enrich.WithProperty("Statie", setari?.NumeStatie ?? Environment.MachineName)
            .WriteTo.File(
                Path.Combine(folder ?? CaiLocale.FolderLoguri, "autogara-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Statie} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    public static void Inchide() => Log.CloseAndFlush();
}
