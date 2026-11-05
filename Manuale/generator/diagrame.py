"""Diagramele manualelor (fluxuri, stari, livrare), randate cu Graphviz si matplotlib, in stilul
diagramelor din raportul de grup. Utilizare: python3 diagrame.py <folder_png>"""
import os
import subprocess
import sys

import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import FancyBboxPatch

OUT = sys.argv[1] if len(sys.argv) > 1 else "diagrame"
os.makedirs(OUT, exist_ok=True)
G, L, Y, YL, R, INK = "#1e5b46", "#dde8e2", "#e8a317", "#fbefd2", "#b3261e", "#1b1b1b"
ANTET = f'''graph [fontname="DejaVu Sans", fontsize=18, nodesep=0.35, ranksep=0.42, pad=0.25, bgcolor="white"];
node [fontname="DejaVu Sans", fontsize=17, color="{G}", penwidth=1.5, shape=box, style="rounded,filled", fillcolor="white", margin="0.2,0.1"];
edge [fontname="DejaVu Sans", fontsize=15, color="#555555", fontcolor="#333333", arrowsize=0.8, penwidth=1.2];
'''


def dot(cod, corp, rankdir="TB"):
    src = f'digraph {cod} {{ rankdir={rankdir}; {ANTET}\n{corp}\n}}'
    subprocess.run(["dot", "-Tpng", "-Gdpi=170", "-o", os.path.join(OUT, cod + ".png")], input=src.encode("utf8"), check=True)


START = f'shape=circle, style=filled, fillcolor="{G}", label="", width=0.32, fixedsize=true'
STOP = f'shape=doublecircle, style=filled, fillcolor="{G}", label="", width=0.26, fixedsize=true'
DEC = f'shape=diamond, style="filled", fillcolor="{YL}", color="{Y}", margin="0.05,0.05"'
ACC = f'style="rounded,filled", fillcolor="{L}"'

# ---------- pasager: cumpararea unui bilet ----------
dot("DG01_flux_cumparare", f'''
s [{START}];
a [label="Autentificare cu contul de pasager"];
b [label="Cumpără bilet: stația de plecare,\\nstația de sosire, data → Caută"];
d1 [{DEC}, label="Există\\ncurse?"];
c [label="Dublu-clic pe cursă\\n(sau Alege locul)"];
l [label="Clic pe un loc liber (verde)"];
d2 [{DEC}, label="Locul a\\nfost rezervat?"];
r [{ACC}, label="Locul este rezervat 10 minute\\n(cronometrul arată timpul rămas)"];
f [label="Reducerea (dacă aveți drept)\\nplata: doar Card"];
k [label="Confirmă vânzarea (F2)"];
d3 [{DEC}, label="Confirmat\\nla timp?"];
e [{ACC}, label="Biletul este emis:\\ncod unic AG…, locul devine ocupat"];
m [label="Biletul apare în „Biletele mele”"];
x [{STOP}];
s -> a -> b -> d1;
d1 -> c [label="da"]; d1 -> b [label="nu: altă dată\\nsau altă stație"];
c -> l -> d2; d2 -> r [label="da"]; d2 -> l [label="nu: alt casier\\nl-a luat"];
r -> f -> k -> d3; d3 -> e [label="da"]; d3 -> l [label="nu: rezervarea\\na expirat"];
e -> m -> x;
''')

# ---------- starile biletului ----------
dot("DG02_stari_bilet", f'''
s [{START}];
A [{ACC}, label="Activ"];
RB [label="Rambursat\\n(s-a întors 100 % sau 50 %)"];
AN [label="Anulat\\n(fără rambursare)"];
x [{STOP}];
s -> A [label="vânzare confirmată"];
A -> RB [label="anulare cu ≥ 2 h\\nînainte de plecare"];
A -> RB [label="cursa anulată\\nde autogară (100 %)"];
A -> AN [label="anulare cu < 2 h\\nînainte de plecare"];
RB -> x; AN -> x;
{{rank=same; RB; AN}}
''', "TB")

# ---------- starile locului ----------
dot("DG04_stari_loc", f'''
s [{START}];
LI [label="Liber", fillcolor="#e3efe6", color="#2e7d32"];
RZ [label="Rezervat\\n(provizoriu, 10 min)", fillcolor="#faebc8", color="#b7791f"];
OC [label="Ocupat", fillcolor="#e2dfd8", color="#a39e93"];
s -> LI [label="crearea cursei"];
LI -> RZ [label="casierul alege locul"];
RZ -> OC [label="F2: vânzare confirmată"];
RZ -> LI [label="Esc / închiderea ferestrei /\\nexpirarea celor 10 minute"];
OC -> LI [label="biletul este anulat"];
''', "TB")

# ---------- casier: vanzarea ----------
dot("DG05_flux_vanzare", f'''
s [{START}];
a [label="Pasagerul spune destinația și data"];
b [label="Vânzare bilete: Din stația, Spre stația,\\nData → Caută"];
c [label="Dublu-clic pe cursă: se deschide\\nschema autobuzului"];
d [label="Clic pe locul ales de pasager\\n(locul se rezervă 10 minute)"];
e [label="Numele pasagerului (obligatoriu), telefonul,\\nreducerea după document"];
d1 [{DEC}, label="Plata?"];
n [label="Numerar: primește banii,\\nemite bonul fiscal,\\nscrie nr. bonului"];
k [label="Card: plata la terminal,\\nscrie nr. bonului"];
f [label="F2 — Confirmă vânzarea"];
g [{ACC}, label="Mesajul „Biletul a fost emis”:\\ncodul, locul, prețul"];
h [label="Comunică pasagerului codul\\nbiletului și peronul"];
x [{STOP}];
s -> a -> b -> c -> d -> e -> d1;
d1 -> n [label="numerar"]; d1 -> k [label="card"];
n -> f; k -> f; f -> g -> h -> x;
''')

# ---------- starile cursei ----------
dot("DG06_stari_cursa", f'''
s [{START}];
P [{ACC}, label="Planificată\\n(se vând bilete)"];
D [label="În desfășurare"];
F [label="Finalizată"];
A [label="Anulată\\n(biletele rambursate 100 %)", color="{R}"];
x [{STOP}];
s -> P [label="Adaugă cursa"];
P -> D [label="A plecat"];
D -> F [label="A ajuns"];
P -> A [label="Anulează… + motivul"];
F -> x; A -> x;
{{rank=same; D; A}}
''', "TB")

# ---------- manager: planificarea unei curse ----------
dot("DG07_planificare", f'''
t [label="Traseul există și este activ\\n(cel puțin două stații)"];
b [label="Autobuzul este Activ\\nși are ITP valabil la data cursei"];
o [label="Șoferul este activ"];
a [{ACC}, label="Curse → Adaugă: traseu, autobuz, șofer,\\ndata, ora de plecare și de sosire, prețul"];
v [{DEC}, label="Verificările\\nbackend-ului"];
e [label="Mesaj: ITP expirat, suprapunere\\ncu altă cursă, ore greșite"];
c [{ACC}, label="Cursa este salvată (Planificată);\\nlocurile 1…N se creează automat"];
z [label="Cursa apare la casieri\\nși la pasageri"];
t -> a; b -> a; o -> a; a -> v;
v -> c [label="corect"]; v -> e [label="eroare"]; e -> a [label="se corectează"];
c -> z;
''')

# ---------- admin: backup ----------
dot("DG09_backup", f'''
subgraph cluster_z {{ label="În fiecare zi"; fontsize=17; color="{G}"; style="rounded";
  c [{ACC}, label="Job_BackupComplet\\n02:00 (de mutat la 22:30)\\nBACKUP DATABASE … WITH CHECKSUM"];
  d [label="Job_BackupDiferential\\nla 4 ore, 06:00–22:00\\nWITH DIFFERENTIAL, CHECKSUM"];
  v [label="RESTORE VERIFYONLY\\nimediat după fiecare backup"];
  st [label="se șterg fișierele\\nmai vechi de 30 de zile"];
}}
subgraph cluster_l {{ label="Lunar"; fontsize=17; color="{G}"; style="rounded";
  t [label="10_BackupRestore.sql: COPY_ONLY →\\nrestaurare în autogara_TestRestaurare →\\nDBCC CHECKDB → compararea rândurilor"];
}}
f [shape=folder, fillcolor="{YL}", color="{Y}", style=filled, label="D:\\\\Backup\\\\autogara\\n*.bak"];
c -> v; d -> v; v -> f; f -> st; f -> t [style=dashed];
c -> d [style=dotted, label="baza pentru\\ndiferențiale"];
''', "TB")

# ---------- admin: restaurarea ----------
dot("DG13_restaurare", f'''
s [{START}];
a [label="Stațiile se opresc sau rămân în modul offline"];
b [label="RESTORE … ultimul backup complet\\nWITH NORECOVERY"];
c [label="RESTORE … ultimul backup diferențial\\nWITH RECOVERY"];
d [label="DBCC CHECKDB + numărul de rânduri\\ncomparat cu ultimul raport"];
d1 [{DEC}, label="Corect?"];
e [{ACC}, label="Se pornesc stațiile: revin singure\\nonline, fără reautentificare"];
f [label="Se încearcă backup-ul anterior"];
x [{STOP}];
s -> a -> b -> c -> d -> d1; d1 -> e [label="da"]; d1 -> f [label="nu"]; f -> b; e -> x;
''')

# ---------- devops: straturile solutiei ----------
dot("DG10_straturi", f'''
W [label="Autogara.WinForms\\nformulare, controale, iconițe", fillcolor="{G}", fontcolor="white"];
B [{ACC}, label="Autogara.Business\\nAutogaraBackend, servicii, reguli, securitate"];
D [label="Autogara.DataAccess\\nEF Core, proceduri stocate, erori SQL"];
F [label="Autogara.FileServer\\nSFTP (SSH.NET), JSON + validare"];
DM [label="Autogara.Domain\\nentități, enumerări"];
C [label="Autogara.Common\\nconfigurare DPAPI, jurnal, excepții"];
T [label="Autogara.Tests\\nxUnit, 100 de cazuri", style="rounded,dashed"];
SQL [shape=cylinder, fillcolor="{YL}", color="{Y}", style=filled, label="SQL Server\\nautogara"];
SF [shape=folder, fillcolor="{YL}", color="{Y}", style=filled, label="File-Server\\n(SFTP)"];
W -> B; B -> D; B -> F; D -> DM; D -> C; F -> DM; F -> C; B -> DM; B -> C;
D -> SQL [style=dashed, label="TDS criptat"]; F -> SF [style=dashed, label="SFTP"];
T -> B [style=dashed]; T -> F [style=dashed];
{{rank=same; D; F}} {{rank=same; DM; C}}
''')

# ---------- devops: livrarea unei versiuni ----------
dot("DG12_livrare", f'''
a [label="Modificare în cod\\n(ramură nouă)"];
b [label="dotnet build\\ndotnet test"];
c [label="commit + push\\npull request pe GitHub"];
d [label="revizuire și merge\\nîn main"];
e [{ACC}, label="dotnet publish -c Release\\n-r win-x64"];
f [label="arhiva pachetului în\\nFile-Server/Actualizari/"];
g [label="version.json:\\nversiunea + numele fișierului"];
h [{ACC}, label="la deschiderea login-ului, fiecare stație\\ncompară versiunea și descarcă pachetul"];
a -> b -> c -> d -> e -> f -> g -> h;

''', "TB")


# ---------- regula rambursarii (axa timpului) ----------
def rambursare(cod):
    plt.rcParams["font.family"] = "DejaVu Sans"
    fig, ax = plt.subplots(figsize=(10, 2.6), dpi=200)
    ax.set_xlim(0, 10); ax.set_ylim(0, 2.6); ax.axis("off")
    zone = [(0.2, 5.6, "#2e7d32", "#e3efe6", "100 %", "cu cel puțin 24 h\nînainte de plecare"),
            (5.6, 8.4, "#b7791f", "#faebc8", "50 %", "între 24 h și 2 h"),
            (8.4, 9.5, R, "#f6dedb", "0 %", "sub 2 h")]
    for a, b, c, f, p, t in zone:
        ax.add_patch(FancyBboxPatch((a, 1.0), b - a - 0.04, 0.9, boxstyle="square,pad=0", fc=f, ec=c, lw=1.4))
        ax.text((a + b) / 2, 1.58, p, ha="center", va="center", fontsize=15, fontweight="bold", color=c)
        ax.text((a + b) / 2, 1.2, t, ha="center", va="center", fontsize=8.6, color=INK)
    for x, t in [(5.6, "−24 h"), (8.4, "−2 h")]:
        ax.plot([x, x], [0.78, 1.0], color=INK, lw=1)
        ax.text(x, 0.6, t, ha="center", fontsize=10, color=INK)
    ax.annotate("", xy=(9.85, 0.88), xytext=(0.2, 0.88), arrowprops=dict(arrowstyle="-|>", color=INK, lw=1.2))
    ax.plot([9.5, 9.5], [0.7, 2.05], color=G, lw=2.4)
    ax.text(9.5, 2.25, "plecarea", ha="center", fontsize=10.5, fontweight="bold", color=G)
    ax.text(0.2, 0.45, "momentul anulării →", fontsize=9.5, color="#52514e")
    ax.text(9.62, 1.45, "anularea nu mai\nse poate face", fontsize=8, color=R, va="center")
    fig.savefig(os.path.join(OUT, cod + ".png"), facecolor="white", bbox_inches="tight")
    plt.close(fig)


rambursare("DG03_rambursare")


# ---------- secventa unei vanzari ----------
def secventa(cod):
    plt.rcParams["font.family"] = "DejaVu Sans"
    actori = ["FrmVanzareBilet", "RezervareService\nBiletService", "ProceduriStocate", "SQL Server\n(proceduri)"]
    mesaje = [(0, 1, "RezervaLocAsync(locId)"), (1, 2, "sp_RezervaLoc"), (2, 3, "UPDLOCK, HOLDLOCK → Rezervat"),
              (3, 0, "RezervareID, SecundeRamase", True),
              (0, 1, "ConfirmaVanzareAsync(...)\nvalidare: nume, telefon"), (1, 2, "sp_ConfirmaVanzareBilet"),
              (2, 3, "fn_CalculeazaPret, cod unic,\nBilete + Plati + LogAudit"), (3, 0, "BiletEmis (cod, loc, preț)", True)]
    fig, ax = plt.subplots(figsize=(11, 6.2), dpi=200)
    xs = [1.2, 4.0, 6.8, 9.6]
    ax.set_xlim(0, 10.8); ax.set_ylim(0, len(mesaje) + 1.6); ax.axis("off")
    top = len(mesaje) + 1.0
    for x, a in zip(xs, actori):
        ax.add_patch(FancyBboxPatch((x - 1.05, top), 2.1, 0.6, boxstyle="round,pad=0.02,rounding_size=0.06", fc=L, ec=G, lw=1.4))
        ax.text(x, top + 0.3, a, ha="center", va="center", fontsize=9.5, fontweight="bold", color=INK)
        ax.plot([x, x], [0.2, top], color="#999999", lw=1, ls="--")
    for i, m in enumerate(mesaje):
        a, b, t = m[0], m[1], m[2]
        ret = len(m) > 3
        y = top - 0.6 - i * 1.0
        if a == b:
            ax.annotate("", xy=(xs[a] + 0.05, y - 0.3), xytext=(xs[a] + 0.05, y),
                        arrowprops=dict(arrowstyle="-|>", color=G, lw=1.2, connectionstyle="arc3,rad=-1.2"))
            ax.text(xs[a] + 0.45, y - 0.15, t, fontsize=8.8, va="center", color=INK)
            continue
        ax.annotate("", xy=(xs[b], y), xytext=(xs[a], y),
                    arrowprops=dict(arrowstyle="-|>", color="#777777" if ret else G, lw=1.2, ls="--" if ret else "-"))
        ax.text((xs[a] + xs[b]) / 2, y + 0.12, t, ha="center", va="bottom", fontsize=8.8, color=INK)
    fig.savefig(os.path.join(OUT, cod + ".png"), facecolor="white", bbox_inches="tight")
    plt.close(fig)


secventa("DG11_secventa_vanzare")
print("diagrame gata")
