using Autogara.Common;
using Autogara.Domain.Enumerari;

namespace Autogara.Business;

public sealed record UtilizatorAutentificat(
    Guid UtilizatorID,
    string NumeUtilizator,
    string Nume,
    string Prenume,
    string? Email,
    RolTip Rol)
{
    public string NumeComplet => $"{Prenume} {Nume}";
}

/// <summary>
/// Utilizatorul autentificat pe statia de lucru. Fiecare serviciu verifica aici rolul inainte
/// de operatie — ascunderea butoanelor in UI nu e suficienta.
/// </summary>
public sealed class Sesiune
{
    public UtilizatorAutentificat? Utilizator { get; private set; }

    public bool EsteAutentificat => Utilizator is not null;

    /// <summary>La autentificare si la deconectare (UI-ul reconstruieste meniul dupa rol).</summary>
    public event EventHandler? Schimbata;

    internal void Porneste(UtilizatorAutentificat utilizator)
    {
        Utilizator = utilizator;
        Schimbata?.Invoke(this, EventArgs.Empty);
    }

    internal void Inchide()
    {
        Utilizator = null;
        Schimbata?.Invoke(this, EventArgs.Empty);
    }

    public UtilizatorAutentificat CereAutentificare() =>
        Utilizator ?? throw new AccesInterzisException("Sesiunea a expirat. Autentificați-vă din nou.");

    public UtilizatorAutentificat CereRol(params RolTip[] roluri)
    {
        var u = CereAutentificare();
        if (!roluri.Contains(u.Rol))
            throw new AccesInterzisException();
        return u;
    }

    public UtilizatorAutentificat CereAdmin() => CereRol(RolTip.Admin);

    /// <summary>Admin sau Casier — personalul de la ghiseu.</summary>
    public UtilizatorAutentificat CerePersonal() => CereRol(RolTip.Admin, RolTip.Casier);

    public bool AreRol(params RolTip[] roluri) => Utilizator is { } u && roluri.Contains(u.Rol);
}
