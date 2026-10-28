# Diagrama Gantt a lucrarilor, din istoricul Git: pentru fiecare element, primul si ultimul
# commit care atinge fisierele lui (fara commit-ul "recup" din 21.09, care a rescris tot depozitul).
import subprocess, sys
from datetime import datetime, timedelta
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import Patch

REPO = sys.argv[1]
OUT = sys.argv[2]
EXCLUS = {"recup"}

DEV = {  # culoarea urmeaza dezvoltatorul (paleta categoriala validata)
    "Gojinevschii Dmitri": "#2a78d6",
    "Hanganu Sergiu": "#eb6834",
    "Crivenco Alexandr": "#1baf7a",
    "Sopivnic Maxim": "#eda100",
    "Echipa": "#8a8984",
}

# (element, dezvoltator, cai, filtru pe subiectul commit-ului)
ELEMENTE = [
    ("Schema, tabelele, indicii, viziunile", "Gojinevschii Dmitri",
     ["SQL Code/02_Schema.sql", "SQL Code/03_Tables.sql", "SQL Code/04_Indexes.sql", "SQL Code/05_Functions.sql", "SQL Code/06_Views.sql"], None),
    ("Procedurile stocate", "Gojinevschii Dmitri", ["SQL Code/07_StoredProcedures.sql"], None),
    ("Securitatea: roluri, conturi, permisiuni", "Gojinevschii Dmitri", ["SQL Code/08_Security.sql", "SQL Code/12_TeamAccounts.sql"], None),
    ("Joburile SQL Server Agent", "Gojinevschii Dmitri", ["SQL Code/09_Jobs.sql"], None),
    ("Backup-ul și restaurarea", "Gojinevschii Dmitri", ["SQL Code/10_BackupRestore.sql"], None),
    ("Datele de pornire (seed)", "Gojinevschii Dmitri", ["SQL Code/11_Seed.sql"], None),
    ("Datele de volum (~10 000 de bilete)", "Gojinevschii Dmitri", ["SQL Code/12_DateVolum.sql"], None),
    ("Sarcina tehnică și planul", "Hanganu Sergiu", ["Sarcina Tehnica.docx", "plan.md"], None),
    ("Depozitul Git și README", "Hanganu Sergiu", ["README.md"], None),
    ("Serverul: SQL Server, rețea, porturi", "Hanganu Sergiu", ["Credentials.txt"], None),
    ("File server SFTP și fișierele JSON", "Hanganu Sergiu", ["File-Server", "App/Autogara.FileServer"], None),
    ("Backend (.NET, pe straturi)", "Hanganu Sergiu",
     ["App/Autogara.Domain", "App/Autogara.Common", "App/Autogara.DataAccess", "App/Autogara.Business"], None),
    ("Testele automate (xUnit)", "Hanganu Sergiu", ["App/Autogara.Tests"], None),
    ("Formularele și controalele desenate", "Crivenco Alexandr", ["App/Autogara.WinForms"], "Frontend WinForms"),
    ("Designul nou, ferestrele modale", "Crivenco Alexandr", ["App/Autogara.WinForms"], "Design nou"),
    ("Testarea funcțională (40 de cazuri)", "Sopivnic Maxim", ["Testare/Capturi", "Testare/Raport_testare.docx"], None),
    ("Programul de stres (concurență)", "Sopivnic Maxim", ["Testare/Stres"], None),
    ("Testarea extinsă și recuperarea", "Sopivnic Maxim", ["Testare/Capturi_v2"], None),
    ("Raportul de progres (raport.docx)", "Echipa", ["raport.docx"], None),
    ("Raportul de grup", "Echipa", ["Raport/raport_grup", "Raport/pachet_capturi"], None),
]


def commituri(cai, filtru):
    out = subprocess.run(["git", "-C", REPO, "log", "--no-merges", "--format=%ad\t%s", "--date=format:%Y-%m-%d %H:%M", "--", *cai],
                         capture_output=True, text=True, check=True).stdout
    r = []
    for linie in out.splitlines():
        data, subiect = linie.split("\t", 1)
        if subiect.strip() in EXCLUS or (filtru and not subiect.startswith(filtru)):
            continue
        r.append(datetime.strptime(data, "%Y-%m-%d %H:%M"))
    return sorted(r)


randuri = []
for nume, dev, cai, filtru in ELEMENTE:
    c = commituri(cai, filtru)
    if not c:
        print("fara commit-uri:", nume, file=sys.stderr)
        continue
    randuri.append((nume, dev, c[0], c[-1], len(c)))
    print(f"{dev:20} | {nume:48} | {c[0]:%d.%m %H:%M} -> {c[-1]:%d.%m %H:%M} | {len(c)} commit-uri")

# Axa timpului e intrerupta: lucrul s-a facut in trei perioade, intre ele nu exista commit-uri.
PERIOADE = [
    (datetime(2026, 9, 17, 13), datetime(2026, 9, 19, 16), "17–19 septembrie", 6),
    (datetime(2026, 10, 13, 15), datetime(2026, 10, 13, 17), "13 octombrie", 1.5),
    (datetime(2026, 10, 21, 12), datetime(2026, 10, 22, 1), "21–22 octombrie", 3),
]

plt.rcParams["font.family"] = "DejaVu Sans"
INK, INK2, GRID = "#1b1b1b", "#52514e", "#e4e3df"
n = len(randuri)
fig, axe = plt.subplots(1, 3, figsize=(10.5, 0.36 * n + 1.9), dpi=220, sharey=True,
                        gridspec_kw={"width_ratios": [p[3] for p in PERIOADE], "wspace": 0.05})
for ax, (a, b, titlu, lat) in zip(axe, PERIOADE):
    span = (b - a).total_seconds() / 3600
    minim = timedelta(hours=span * 0.2 / lat)  # aceeasi latime fizica minima in toate perioadele
    for i, (nume, dev, s, f, k) in enumerate(randuri):
        y = n - 1 - i
        if f < a or s > b:
            continue
        s2, f2 = max(s, a), min(f, b)
        if f2 - s2 < minim:
            s2 = s2 - minim / 2
            f2 = s2 + minim
        ax.barh(y, (f2 - s2).total_seconds() / 86400, left=matplotlib.dates.date2num(s2), height=0.62,
                color=DEV[dev], edgecolor="white", linewidth=1.5)
    ax.set_xlim(matplotlib.dates.date2num(a), matplotlib.dates.date2num(b))
    ax.set_title(titlu, fontsize=11.5, color=INK, pad=8)
    if span > 24:
        ax.xaxis.set_major_locator(matplotlib.dates.HourLocator(byhour=[0, 12]))
        ax.xaxis.set_major_formatter(matplotlib.dates.DateFormatter("%d.%m\n%H:%M"))
    else:
        ax.xaxis.set_major_locator(matplotlib.dates.HourLocator(byhour=[16] if span <= 3 else [15, 21]))
        ax.xaxis.set_major_formatter(matplotlib.dates.DateFormatter("%H:%M"))
    ax.tick_params(axis="x", labelsize=9.5, colors=INK2, length=0)
    ax.tick_params(axis="y", length=0)
    ax.grid(axis="x", color=GRID, linewidth=0.8)
    ax.set_axisbelow(True)
    for i in range(1, n):  # separator intre dezvoltatori
        if randuri[i][1] != randuri[i - 1][1]:
            ax.axhline(n - 0.5 - i, color="#c9c8c2", linewidth=0.8)
    for s in ["top", "right", "left"]:
        ax.spines[s].set_visible(False)
    ax.spines["bottom"].set_color("#b5b4ae")
# Separatoare „//” intre perioade
for ax in axe[:-1]:
    ax.plot([1, 1], [0, 1], transform=ax.transAxes, color="#b5b4ae", lw=0.8, ls=(0, (3, 3)), clip_on=False)

axe[0].set_yticks(range(n))
axe[0].set_yticklabels([r[0] for r in reversed(randuri)], fontsize=11, color=INK)
axe[0].set_ylim(-0.6, n - 0.4)
fig.legend(handles=[Patch(color=c, label=d) for d, c in DEV.items()], loc="upper center", ncol=5, frameon=False,
           fontsize=10.5, bbox_to_anchor=(0.62, 1.0), handlelength=1.0, columnspacing=1.2)
fig.text(0.62, 0.012, "Timpul (data și ora commit-urilor în Git); axa este întreruptă între perioadele de lucru",
         ha="center", fontsize=10, color=INK2)
fig.subplots_adjust(left=0.36, right=0.99, top=1 - 0.85 / (0.36 * n + 1.9), bottom=0.8 / (0.36 * n + 1.9))
fig.savefig(OUT, facecolor="white")
print("salvat", OUT)
