namespace Autogara.Domain.Enumerari;

// Valorile sunt sincronizate 1:1 cu CHECK constraints din 03_Tables.sql.
// Textul exact din baza de date se obtine prin ValoriDb.

public enum RolTip { Admin, Casier, Pasager }

public enum StatusLoc { Liber, Rezervat, Ocupat }

public enum StatusBilet { Activ, Anulat, Rambursat }

public enum StatusCursa { Planificata, InDesfasurare, Finalizata, Anulata }

public enum StatusPlata { Finalizata, Rambursata }

public enum MetodaPlata { Numerar, Card }

public enum StatusAutobuz { Activ, Service, ScosDinUz }

public enum TipNod { Statie, Intersectie }
