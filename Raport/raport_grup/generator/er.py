import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import FancyBboxPatch

T = {
 "Roluri": (["RolID PK", "Denumire UQ"], (10.5, 16.4)),
 "Utilizatori": (["UtilizatorID PK (GUID)", "NumeUtilizator UQ", "ParolaHash", "RolID FK", "Activ, RowVersion"], (10.5, 13.0)),
 "LogAudit": (["LogID PK", "UtilizatorID FK", "Actiune, Entitate", "DataOra"], (13.9, 13.0)),
 "Noduri": (["NodID PK", "Tip, Nume", "CoordX, CoordY"], (3.7, 16.4)),
 "Conexiuni": (["ConexiuneID PK", "NodPlecareID FK", "NodSosireID FK", "DistantaKm"], (0.3, 16.4)),
 "Statii": (["StatieID PK", "NodID FK UQ", "Adresa, Peron"], (7.1, 16.4)),
 "TraseuOpriri": (["TraseuOprireID PK", "TraseuID FK", "StatieID FK", "Ordine"], (7.1, 13.0)),
 "Trasee": (["TraseuID PK", "Denumire UQ"], (3.7, 13.0)),
 "TipuriReducere": (["TipReducereID PK", "Denumire UQ", "ProcentReducere"], (7.1, 1.6)),
 "Autobuze": (["AutobuzID PK", "NrInmatriculare UQ", "CapacitateLocuri", "CaleFisierJSON", "DataExpirareITP"], (0.3, 9.0)),
 "MentenantaAutobuze": (["MentenantaID PK", "AutobuzID FK", "TipLucrare, Data"], (0.3, 13.0)),
 "Soferi": (["SoferID PK", "NrPermis UQ", "Nume, Prenume"], (0.3, 5.0)),
 "Curse": (["CursaID PK", "TraseuID FK", "AutobuzID FK", "SoferID FK", "DataCursa, OraPlecare", "Pret, Status"], (3.7, 9.0)),
 "Locuri": (["LocID PK", "CursaID FK", "NumarLoc", "Status, RowVersion"], (3.7, 5.0)),
 "RezervariProvizorii": (["RezervareID PK (GUID)", "LocID FK UQ", "UtilizatorID FK", "DataExpirare"], (7.1, 9.0)),
 "Bilete": (["BiletID PK (GUID)", "CodBilet UQ", "CursaID, LocID FK", "TipReducereID FK", "VanzutDeUtilizatorID FK", "Pret, Status"], (7.1, 5.0)),
 "Plati": (["PlataID PK", "BiletID FK", "Suma, MetodaPlata", "Status"], (10.5, 5.0)),
}
W = 2.95; LH = 0.36; HH = 0.45
def box(name):
    cols, (x, y) = T[name]
    h = HH + LH * len(cols) + 0.1
    return x, y, W, h  # y = top
FK = [("Utilizatori","Roluri"),("LogAudit","Utilizatori"),("Conexiuni","Noduri"),("Statii","Noduri"),
      ("TraseuOpriri","Trasee"),("TraseuOpriri","Statii"),("MentenantaAutobuze","Autobuze"),
      ("Curse","Trasee"),("Curse","Autobuze"),("Curse","Soferi"),("Locuri","Curse"),
      ("RezervariProvizorii","Locuri"),("RezervariProvizorii","Utilizatori"),("Bilete","Curse"),("Bilete","Locuri"),
      ("Bilete","TipuriReducere"),("Bilete","Utilizatori"),("Plati","Bilete")]
fig, ax = plt.subplots(figsize=(17, 17.5), dpi=130)
ax.set_xlim(0, 17.0); ax.set_ylim(-0.6, 16.8); ax.axis("off")
def centre(n):
    x, y, w, h = box(n); return x + w/2, y - h/2
def edge_pt(n, tx, ty):
    x, y, w, h = box(n); cx, cy = x + w/2, y - h/2
    dx, dy = tx - cx, ty - cy
    if dx == 0 and dy == 0: return cx, cy
    sx = (w/2)/abs(dx) if dx else 1e9; sy = (h/2)/abs(dy) if dy else 1e9
    s = min(sx, sy); return cx + dx*s, cy + dy*s
for a, b in FK:
    ax_, ay_ = centre(a); bx_, by_ = centre(b)
    if a == "Bilete" and b == "Utilizatori":
        xb, yb, wb, hb = box(a); xu, yu, wu, hu = box(b)
        p1 = (xb + wb - 0.05, yb); p2 = (xu + 1.3, yu - hu)
    else:
        p1 = edge_pt(a, bx_, by_); p2 = edge_pt(b, ax_, ay_)
    ax.annotate("", xy=p2, xytext=p1, arrowprops=dict(arrowstyle="-|>", color="#555", lw=1.2))
for n, (cols, (x, y)) in T.items():
    _, _, w, h = box(n)
    ax.add_patch(FancyBboxPatch((x, y - h), w, h, boxstyle="round,pad=0.02", fc="white", ec="#1e5b46", lw=1.4, zorder=3))
    ax.add_patch(FancyBboxPatch((x, y - HH), w, HH, boxstyle="round,pad=0.02", fc="#1e5b46", ec="#1e5b46", lw=1.4, zorder=4))
    ax.text(x + w/2, y - HH/2, n, ha="center", va="center", color="white", fontsize=10.5, fontweight="bold", zorder=5, family="DejaVu Sans")
    for i, c in enumerate(cols):
        bold = "PK" in c
        ax.text(x + 0.12, y - HH - LH*(i+0.5) - 0.05, c, ha="left", va="center", fontsize=9.5, zorder=5,
                fontweight="bold" if bold else "normal", color="#1b1b1b", family="DejaVu Sans")
ax.text(0.3, -0.45, "Săgeata merge de la tabelul cu cheia externă (FK) spre tabelul referit (PK). PK = cheie primară, FK = cheie externă, UQ = unic.",
        fontsize=10, color="#444", family="DejaVu Sans")
plt.savefig("ER_diagrama.png", bbox_inches="tight", facecolor="white")
