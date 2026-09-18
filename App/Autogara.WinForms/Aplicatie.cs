using Autogara.Business;
using Autogara.Domain.Enumerari;

namespace Autogara.WinForms
{
    /// <summary>Backend-ul folosit de toate formularele, creat o singura data in Program.</summary>
    public static class Aplicatie
    {
        public static AutogaraBackend Backend { get; internal set; }

        public static UtilizatorAutentificat Utilizator => Backend?.Sesiune.Utilizator;

        public static bool EsteAdmin => Utilizator?.Rol == RolTip.Admin;

        public static bool EstePasager => Utilizator?.Rol == RolTip.Pasager;

        public static Version Versiune => typeof(Aplicatie).Assembly.GetName().Version ?? new Version(1, 0);
    }
}
