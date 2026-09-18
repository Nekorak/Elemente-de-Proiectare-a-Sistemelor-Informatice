using Serilog;

namespace Autogara.Common;

/// <summary>
/// Punctul central prin care UI-ul transforma orice exceptie in mesaj pentru utilizator.
/// Exceptiile neasteptate se logheaza complet; utilizatorul vede doar un mesaj scurt.
/// </summary>
public static class ExceptionHandler
{
    public static string MesajPrietenos(Exception ex)
    {
        if (ex is AggregateException { InnerExceptions.Count: 1 } agregat)
            ex = agregat.InnerExceptions[0];

        switch (ex)
        {
            case AutogaraException:
                Log.Information("Eroare afișată utilizatorului: {Mesaj}", ex.Message);
                return ex.Message;

            case OperationCanceledException:
                return "Operația a fost anulată.";

            case TimeoutException:
                Log.Warning(ex, "Timeout");
                return "Serverul nu a răspuns la timp. Verificați conexiunea la internet și încercați din nou.";

            default:
                Log.Error(ex, "Eroare neașteptată");
                return "A apărut o eroare neașteptată. Detaliile au fost salvate în jurnalul aplicației.";
        }
    }
}
