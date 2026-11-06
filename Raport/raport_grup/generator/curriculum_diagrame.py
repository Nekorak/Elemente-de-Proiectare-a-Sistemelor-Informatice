# Diagramele cerute de curriculumul S.07.O.022 (produse de evaluat):
# aria de acoperire, ciclul de viata, relatia beneficiar-executant, proiectarea esalonata pe subsisteme,
# sistemul distribuit, cazurile de utilizare si secventa vanzarii (UML).
# Diagrama de clase se face separat, cu uml_clase.py (Graphviz).
# Utilizare: python3 curriculum_diagrame.py <folderul capturi>
import math
import os
import sys

import matplotlib

matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import Ellipse, FancyBboxPatch, Rectangle

OUT = sys.argv[1] if len(sys.argv) > 1 else "."
G, L, INK, INK2, GR, Y = "#1e5b46", "#dde8e2", "#1b1b1b", "#52514e", "#9a9a96", "#e8a317"
plt.rcParams["font.family"] = "DejaVu Sans"


def canvas(w, h, dpi=200):
    fig, ax = plt.subplots(figsize=(w, h), dpi=dpi)
    ax.set_xlim(0, w)
    ax.set_ylim(0, h)
    ax.axis("off")
    return fig, ax


def box(ax, x, y, w, h, text="", fc="white", ec=G, tc=INK, fs=10, bold=False, lw=1.4, ls="-", r=0.08, ha="center"):
    ax.add_patch(FancyBboxPatch((x, y), w, h, boxstyle=f"round,pad=0.02,rounding_size={r}", fc=fc, ec=ec, lw=lw, ls=ls))
    if text:
        tx = x + w / 2 if ha == "center" else x + 0.12
        ax.text(tx, y + h / 2, text, ha=ha, va="center", fontsize=fs, color=tc, fontweight="bold" if bold else "normal",
                linespacing=1.25)


def arrow(ax, p, q, text="", c=G, ls="-", fs=8.5, off=(0, 0.12), style="-|>", lw=1.4, ha="center"):
    ax.annotate("", xy=q, xytext=p, arrowprops=dict(arrowstyle=style, color=c, lw=lw, ls=ls, shrinkA=0, shrinkB=0))
    if text:
        ax.text((p[0] + q[0]) / 2 + off[0], (p[1] + q[1]) / 2 + off[1], text, ha=ha, va="center", fontsize=fs, color=INK2,
                linespacing=1.2)


def save(fig, name):
    fig.savefig(os.path.join(OUT, name + ".png"), facecolor="white", bbox_inches="tight", pad_inches=0.08)
    plt.close(fig)
    print("scris", name)


# ---------------------------------------------------------------- aria de acoperire
def aria():
    fig, ax = canvas(15, 9.6)
    box(ax, 0.1, 0.1, 14.8, 9.3, ec=INK2, lw=1.2, r=0.15)
    ax.text(7.5, 9.05, "Sistemul informațional al autogării (toate informațiile și activitățile firmei)", ha="center",
            fontsize=12.5, fontweight="bold", color=INK)
    box(ax, 0.4, 0.45, 8.9, 8.15, fc=L, ec=G, lw=2, r=0.15)
    ax.text(4.6, 8.25, "Sistemul informatic „Autogara” (automatizat)", ha="center", fontsize=12, fontweight="bold", color=G)
    acoperite = [
        ("Vânzarea biletelor la ghișeu", "cu alegerea locului pe schemă"),
        ("Cumpărarea biletului online", "contul de pasager, plata cu cardul"),
        ("Rezervarea provizorie a locului", "eliberată automat la expirare"),
        ("Anularea și rambursarea", "100 % / 50 % / 0 % după termen"),
        ("Orarul: curse, trasee, opriri", "prețul și statusul cursei"),
        ("Harta stațiilor și distanțele", "editorul hărții"),
        ("Evidența autobuzelor", "schema locurilor (JSON), mentenanța"),
        ("Evidența șoferilor și a reducerilor", "elev, pensionar, abonament"),
        ("Rapoartele și dashboard-ul", "vânzări, ocupare, comparația lunară"),
        ("Conturile, rolurile, jurnalul de audit", "cine a făcut fiecare operație"),
        ("Copiile de rezervă și alertele", "backup, ITP, spațiul pe disc"),
        ("Datele pasagerilor", "nume și telefon, acces pe roluri"),
    ]
    for i, (t, d) in enumerate(acoperite):
        col, row = i % 2, i // 2
        x, y = 0.65 + col * 3.95, 7.2 - row * 1.12
        box(ax, x, y, 3.8, 0.9, fc=G, ec=G, r=0.06)
        ax.text(x + 0.15, y + 0.6, t, fontsize=9.6, fontweight="bold", color="white", va="center")
        ax.text(x + 0.15, y + 0.27, d, fontsize=8.4, color="white", va="center")
    # partial: pe granita
    partial = [
        ("Încasarea fiscală", "se păstrează doar nr. bonului;\nbonul îl emite casa de marcat", 5.6),
        ("ITP-ul autobuzelor", "se păstrează data și alerta;\ncontrolul îl face transportatorul", 3.0),
    ]
    for t, d, y in partial:
        box(ax, 8.6, y, 3.0, 1.25, fc="white", ec=G, ls="--", lw=1.6, r=0.06)
        ax.text(8.72, y + 0.92, t, fontsize=9.6, fontweight="bold", color=G, va="center")
        ax.text(8.72, y + 0.42, d, fontsize=8.2, color=INK2, va="center", linespacing=1.2)
    ax.text(13.3, 8.25, "În afara sistemului", ha="center", fontsize=11.5, fontweight="bold", color=INK2)
    afara = [
        ("Contabilitatea și salariile", "alt program; primește raportul zilnic"),
        ("Contractele cu transportatorii", "documente pe hârtie"),
        ("Anunțarea curselor la peron", "panoul și difuzorul autogării"),
        ("Păstrarea bagajelor", "registru separat"),
        ("Deservirea tehnică a clădirii", "nu ține de vânzarea biletelor"),
    ]
    for i, (t, d) in enumerate(afara):
        y = 7.3 - i * 0.78
        box(ax, 11.85, y, 2.9, 0.62, fc="white", ec=GR, r=0.05)
        ax.text(11.95, y + 0.41, t, fontsize=8.4, fontweight="bold", color=INK, va="center")
        ax.text(11.95, y + 0.17, d, fontsize=7.3, color=INK2, va="center")
    # legenda
    lx, ly = 11.85, 0.6
    for k, (fc, ec, ls, t) in enumerate([(G, G, "-", "automatizat complet"), ("white", G, "--", "automatizat parțial"),
                                          ("white", GR, "-", "rămâne manual / alt sistem")]):
        box(ax, lx, ly + k * 0.38, 0.45, 0.26, fc=fc, ec=ec, ls=ls, lw=1.3, r=0.03)
        ax.text(lx + 0.6, ly + k * 0.38 + 0.13, t, fontsize=8.6, va="center", color=INK)
    save(fig, "AN05_aria_acoperire")


# ---------------------------------------------------------------- ciclul de viata
def ciclu():
    fig, ax = canvas(13.5, 9.0)
    faze = [
        ("1. Inițierea", "studiul de oportunitate,\nobiectivele", "realizat: 1.1–1.2", "ok"),
        ("2. Analiza", "studiul sistemului existent,\ncerințele, caietul de sarcini", "realizat: 1.3–1.6", "ok"),
        ("3. Proiectarea", "de ansamblu (arhitectura)\nși de detaliu (BD, programe)", "realizat: 2, 3.1, 4.1", "ok"),
        ("4. Realizarea", "scripturile SQL, aplicația,\nserverul", "realizat: 2–4", "ok"),
        ("5. Testarea și\nimplementarea", "testarea, experimentarea,\ninstruirea, recepția", "testat: 5; pregătit: 6", "partial"),
        ("6. Exploatarea și\nmentenanța", "lucrul curent, backup,\nremedierea defectelor", "planificat: 6.4, 6.8", "plan"),
        ("7. Retragerea sau\ndezvoltarea", "înlocuirea sau o versiune\nnouă (web, casa de marcat)", "propus: Concluzii", "plan"),
    ]
    cx, cy, R = 6.75, 4.45, 3.4
    n = len(faze)
    pos = []
    for i in range(n):
        a = math.pi / 2 - i * 2 * math.pi / n
        pos.append((cx + R * 1.27 * math.cos(a), cy + R * math.sin(a)))
    w, h = 2.9, 1.3
    for i, ((t, d, s, st), (x, y)) in enumerate(zip(faze, pos)):
        fc = {"ok": G, "partial": L, "plan": "white"}[st]
        tc = "white" if st == "ok" else INK
        box(ax, x - w / 2, y - h / 2, w, h, fc=fc, ec=G, lw=1.5, ls="-" if st != "plan" else "--", r=0.1)
        ax.text(x, y + 0.3, t, ha="center", va="center", fontsize=9.8, fontweight="bold", color=tc, linespacing=1.05)
        ax.text(x, y - 0.3, d, ha="center", va="center", fontsize=7.7, color=tc, linespacing=1.15)
        ax.text(x, y - h / 2 - 0.2, s, ha="center", va="center", fontsize=8, color=G, style="italic")
    # sagetile pe cerc
    for i in range(n):
        a1 = math.pi / 2 - (i + 0.33) * 2 * math.pi / n
        a2 = math.pi / 2 - (i + 0.67) * 2 * math.pi / n
        p = (cx + R * 0.80 * math.cos(a1), cy + R * 0.80 * math.sin(a1))
        q = (cx + R * 0.80 * math.cos(a2), cy + R * 0.80 * math.sin(a2))
        ax.annotate("", xy=q, xytext=p, arrowprops=dict(arrowstyle="-|>", color=G, lw=1.6,
                                                        connectionstyle="arc3,rad=-0.25"))
    ax.text(cx, cy + 0.35, "Ciclul de viață", ha="center", fontsize=13, fontweight="bold", color=G)
    ax.text(cx, cy - 0.15, "al sistemului informatic", ha="center", fontsize=10.5, color=G)
    ax.text(cx, cy - 0.7, "după faza 6, cerințele noi\nreîncep ciclul de la analiză", ha="center", fontsize=8.2,
            color=INK2, style="italic")
    for k, (fc, ls, t) in enumerate([(G, "-", "faza realizată în proiect"), (L, "-", "faza realizată parțial"),
                                     ("white", "--", "faza planificată (după predare)")]):
        box(ax, 0.2, 0.25 + k * 0.38, 0.45, 0.26, fc=fc, ec=G, ls=ls, lw=1.3, r=0.03)
        ax.text(0.8, 0.38 + k * 0.38, t, fontsize=8.6, va="center", color=INK)
    save(fig, "AN06_ciclu_viata")


# ---------------------------------------------------------------- beneficiar - executant
def beneficiar():
    fig, ax = canvas(14, 8.4)
    box(ax, 0.2, 1.3, 3.6, 6.3, fc=L, ec=G, lw=1.8, r=0.12)
    ax.text(2.0, 7.15, "BENEFICIARUL", ha="center", fontsize=12, fontweight="bold", color=G)
    ax.text(2.0, 6.75, "autogara", ha="center", fontsize=10, color=INK2)
    for i, t in enumerate(["conducerea autogării\n(aprobă, recepționează, plătește)", "dispecerul-șef\n(persoana de contact)",
                           "casierii, administratorul\n(utilizatorii finali)", "contabilitatea\n(primește rapoartele)"]):
        box(ax, 0.45, 5.45 - i * 1.15, 3.1, 0.95, text=t, fc="white", fs=8.6, r=0.06)
    box(ax, 10.2, 1.3, 3.6, 6.3, fc=L, ec=G, lw=1.8, r=0.12)
    ax.text(12.0, 7.15, "EXECUTANTUL", ha="center", fontsize=12, fontweight="bold", color=G)
    ax.text(12.0, 6.75, "echipa de proiect (grupa BD-423)", ha="center", fontsize=9.5, color=INK2)
    for i, t in enumerate(["Gojinevschii Dmitri\nbaza de date", "Hanganu Sergiu\nbackend, DevOps, coordonare",
                           "Crivenco Alexandr\ninterfața", "Sopivnic Maxim\ntestarea"]):
        box(ax, 10.45, 5.45 - i * 1.15, 3.1, 0.95, text=t, fc="white", fs=8.6, r=0.06)
    spre_exec = ["caietul de sarcini (Sarcina tehnică)", "datele inițiale: orar, trasee, autobuze",
                 "accesul la server, rețea și stații", "observațiile la experimentare", "plata etapelor recepționate"]
    spre_benef = ["planul și calendarul (diagrama Gantt)", "rapoartele de stare pe etape",
                  "versiunile sistemului și scripturile", "manualele și instruirea personalului",
                  "actele de experimentare și de recepție", "garanția: remedierea defectelor"]
    for i, t in enumerate(spre_exec):
        y = 6.9 - i * 0.55
        arrow(ax, (3.9, y), (10.1, y), t, off=(0, 0.16), fs=8.6)
    for i, t in enumerate(spre_benef):
        y = 3.95 - i * 0.5
        arrow(ax, (10.1, y), (3.9, y), t, off=(0, 0.15), fs=8.6, c=Y)
    box(ax, 3.2, 0.1, 7.6, 0.85, text="Documentele comune: contractul și caietul de sarcini (1.5), actul despre rezultatele\n"
        "experimentării (6.11.1), actul de recepție (6.11.2), documentația predată (6.10)", fc="white", ec=INK2, fs=8.4, r=0.06)
    ax.text(7.0, 7.55, "cerințe, date, resurse", ha="center", fontsize=9.5, fontweight="bold", color=G)
    ax.text(7.0, 4.45, "rezultate, documente, servicii", ha="center", fontsize=9.5, fontweight="bold", color="#b07a0c")
    save(fig, "AN07_beneficiar_executant")


# ---------------------------------------------------------------- proiectarea esalonata pe subsisteme
def subsisteme():
    fig, ax = canvas(14, 8.0)
    etape = ["Eșalonul I", "Eșalonul II", "Eșalonul III", "Eșalonul IV", "Eșalonul V"]
    for i, t in enumerate(etape):
        ax.text(1.2 + i * 2.75, 7.65, t, ha="center", fontsize=10.5, fontweight="bold", color=G)
        if i:
            ax.plot([i * 2.75 - 0.18, i * 2.75 - 0.18], [0.9, 7.4], color=GR, lw=0.8, ls=":")
    S = {
        "DOM": (0.05, 5.9, "Domain + Common\nentitățile, configurarea,\njurnalul (folosite de\ntoate straturile .NET)", "Hanganu S."),
        "BD": (0.05, 4.0, "Subsistemul BD\nschema, constrângeri,\nproceduri, securitate", "Gojinevschii D."),
        "SRV": (0.05, 2.1, "Serverul (DevOps)\nSQL Server :14330,\nOpenSSH :44545", "Hanganu S."),
        "JOB": (2.8, 5.9, "Joburile și backup-ul\nSQL Server Agent,\nrestaurarea", "Gojinevschii D."),
        "DA": (2.8, 4.0, "DataAccess\nEF Core, procedurile\nstocate", "Hanganu S."),
        "FS": (2.8, 2.1, "FileServer\nSFTP, fișierele JSON\nale autobuzelor și hărții", "Hanganu S."),
        "BUS": (5.55, 4.0, "Business\nserviciile, regulile,\nrolurile, cache-ul", "Hanganu S."),
        "UI": (8.3, 4.0, "Interfața WinForms\nferestrele, editoarele,\nschema locurilor", "Crivenco A."),
        "TST": (11.05, 4.0, "Testarea și\npunerea în funcțiune\nstres, recuperare, manuale", "Sopivnic M., echipa"),
    }
    w, h = 2.45, 1.45
    for k, (x, y, t, r) in S.items():
        fc = G if k in ("BD", "TST") else L
        tc = "white" if fc == G else INK
        box(ax, x, y, w, h, fc=fc, ec=G, r=0.08)
        ax.text(x + w / 2, y + h / 2 + 0.12, t, ha="center", va="center", fontsize=8.3, color=tc, linespacing=1.2)
        ax.text(x + w / 2, y + 0.16, r, ha="center", va="center", fontsize=7.4, color=tc, style="italic")
    def c(k, side, dy=0.0):
        x, y, *_ = S[k]
        return {"r": (x + w, y + h / 2 + dy), "l": (x, y + h / 2 + dy)}[side]
    for a, b, d1, d2 in [("BD", "DA", 0, 0), ("BD", "JOB", 0.45, -0.3), ("SRV", "FS", 0, 0), ("DA", "BUS", 0, 0),
                         ("FS", "BUS", 0.3, -0.45), ("BUS", "UI", 0, 0), ("UI", "TST", 0, 0)]:
        ax.annotate("", xy=c(b, "l", d2), xytext=c(a, "r", d1), arrowprops=dict(arrowstyle="-|>", color=G, lw=1.3))
    ax.text(7.0, 0.55, "Săgeata arată ordinea: subsistemul din dreapta se proiectează după cel din stânga și îl folosește prin interfața lui.\n"
            "Subsistemele din același eșalon s-au lucrat în paralel, de membri diferiți ai echipei.", ha="center",
            fontsize=8.6, color=INK2, linespacing=1.3)
    save(fig, "AN08_subsisteme")


# ---------------------------------------------------------------- sistemul distribuit
def distribuit():
    fig, ax = canvas(14, 8.6)
    # server
    box(ax, 4.6, 4.9, 4.8, 3.5, fc=L, ec=G, lw=1.8, r=0.12)
    ax.text(7.0, 8.05, "Nodul central: calculatorul-server", ha="center", fontsize=11, fontweight="bold", color=G)
    box(ax, 4.8, 5.1, 2.25, 2.55, text="SQL Server\ninstanța EPSI\n(TCP 14330)\n\nbaza autogara:\ncurse, locuri, bilete,\nplăți, utilizatori,\njurnalul de audit\n(sursa unică)", fc="white", fs=8.2, r=0.06)
    box(ax, 7.15, 5.1, 2.05, 2.55, text="OpenSSH / SFTP\n(TCP 44545)\n\nAutobuze/*.json\nHarta/harta.json\nBackup/", fc="white", fs=8.2, r=0.06)
    # statii
    st = [(0.2, "Stația ghișeului 1", "casier"), (4.8, "Stația ghișeului 2", "casier"), (9.4, "Stația administratorului", "admin / manager")]
    for i, (x, t, r) in enumerate(st):
        box(ax, x, 0.35, 4.4, 2.75, fc="white", ec=G, lw=1.5, r=0.1)
        ax.text(x + 2.2, 2.82, f"{t} ({r})", ha="center", fontsize=9.5, fontweight="bold", color=G)
        box(ax, x + 0.2, 1.75, 4.0, 0.8, text="aplicația Autogara (.NET 10)\nlogica Business, validările", fc=L, fs=8.2, r=0.05)
        box(ax, x + 0.2, 0.55, 4.0, 1.0, text="%LocalAppData%\\Autogara: configurarea\n(parole DPAPI), Cache/ (curse, stații,\n"
            "hartă, autobuze), Logs/", fc="white", ec=GR, fs=7.8, r=0.05)
        arrow(ax, (x + 1.5, 3.12), (5.3 + i * 0.6, 5.08), "", c=G, lw=1.3)
        arrow(ax, (x + 2.9, 3.12), (7.6 + i * 0.6, 5.08), "", c=Y, lw=1.3, ls="--")
    box(ax, 0.2, 4.4, 3.9, 3.0, fc="white", ec=INK2, r=0.08)
    ax.text(2.15, 7.1, "Cum circulă datele", ha="center", fontsize=9.8, fontweight="bold", color=INK)
    for i, (c, ls, t) in enumerate([(G, "-", "TDS criptat: proceduri stocate,\ntranzacții, blocări UPDLOCK"),
                                     (Y, "--", "SFTP: fișierele JSON ale\nautobuzelor și ale hărții")]):
        ax.plot([0.4, 1.0], [6.45 - i * 0.85, 6.45 - i * 0.85], color=c, lw=1.6, ls=ls)
        ax.text(1.15, 6.45 - i * 0.85, t, fontsize=8, va="center", color=INK, linespacing=1.15)
    ax.text(0.4, 4.85, "Fără server: stațiile arată datele\ndin Cache, doar pentru citire.", fontsize=8, color=INK2,
            linespacing=1.2, va="center")
    box(ax, 9.9, 4.9, 3.9, 2.5, fc="white", ec=INK2, r=0.08)
    ax.text(11.85, 7.1, "Internet / router", ha="center", fontsize=9.8, fontweight="bold", color=INK)
    ax.text(10.1, 5.95, "doar porturile 14330 și 44545\nredirecționate spre server;\nfirewall pe server; stațiile\n"
            "se conectează din orice rețea", fontsize=8, color=INK, linespacing=1.25, va="center")
    save(fig, "DS01_sistem_distribuit")


# ---------------------------------------------------------------- UML: cazuri de utilizare
def actor(ax, x, y, name):
    ax.add_patch(plt.Circle((x, y + 0.75), 0.17, fc="white", ec=INK, lw=1.3))
    ax.plot([x, x], [y + 0.58, y + 0.15], color=INK, lw=1.3)
    ax.plot([x - 0.28, x + 0.28], [y + 0.45, y + 0.45], color=INK, lw=1.3)
    ax.plot([x, x - 0.22], [y + 0.15, y - 0.2], color=INK, lw=1.3)
    ax.plot([x, x + 0.22], [y + 0.15, y - 0.2], color=INK, lw=1.3)
    ax.text(x, y - 0.42, name, ha="center", va="center", fontsize=9.5, fontweight="bold", color=INK)


def cazuri():
    fig, ax = canvas(14.5, 10.0)
    ax.add_patch(Rectangle((2.3, 0.25), 9.9, 9.45, fc="white", ec=INK, lw=1.3))
    ax.text(7.25, 9.4, "Sistemul informatic „Autogara”", ha="center", fontsize=11.5, fontweight="bold", color=INK)
    ax.text(7.25, 9.0, "fiecare caz de utilizare al unui actor uman începe cu autentificarea (precondiție)",
            ha="center", fontsize=8.4, color=INK2, style="italic")
    W = 3.3
    UC = {
        "cauta": (5.0, 8.2, "Caută curse"),
        "rez": (5.0, 7.15, "Rezervă locul"),
        "vinde": (5.0, 6.1, "Vinde biletul la ghișeu"),
        "online": (5.0, 5.05, "Cumpără biletul online"),
        "anul": (5.0, 4.0, "Anulează biletul\ncu rambursare"),
        "mele": (5.0, 2.95, "Vede biletele proprii"),
        "dash": (5.0, 1.9, "Vede dashboard-ul zilei"),
        "pret": (9.55, 7.6, "Calculează prețul\ncu reducerea"),
        "orar": (9.55, 6.3, "Gestionează cursele,\ntraseele și stațiile"),
        "harta": (9.55, 5.3, "Editează harta"),
        "autob": (9.55, 4.3, "Gestionează autobuzele,\nșoferii, mentenanța"),
        "util": (9.55, 3.3, "Gestionează utilizatorii\nși reducerile"),
        "rap": (9.55, 2.3, "Vede rapoartele și\njurnalul de audit"),
        "elib": (9.55, 0.95, "Eliberează rezervările\nexpirate (la fiecare minut)"),
    }
    for k, (x, y, t) in UC.items():
        ax.add_patch(Ellipse((x, y), W, 0.82, fc=L, ec=G, lw=1.3))
        ax.text(x, y, t, ha="center", va="center", fontsize=8.6, color=INK, linespacing=1.1)
    actor(ax, 0.9, 6.6, "Casier")
    actor(ax, 0.9, 2.9, "Pasager")
    actor(ax, 13.5, 4.3, "Administrator")
    actor(ax, 13.5, 0.75, "SQL Server Agent")
    def ln(a, k, side="l"):
        x, y, _ = UC[k]
        ex = x - W / 2 if side == "l" else x + W / 2
        ax.plot([a[0], ex], [a[1], y], color=INK2, lw=0.9)
    cas, pas, adm, agt = (1.25, 7.05), (1.25, 3.35), (13.15, 4.75), (13.15, 0.95)
    for k in ["cauta", "rez", "vinde", "anul", "dash"]:
        ln(cas, k)
    for k in ["cauta", "online", "mele", "anul"]:
        ln(pas, k)
    for k in ["orar", "harta", "autob", "util", "rap"]:
        ln(adm, k, "r")
    ln(agt, "elib", "r")
    def inc(p, q, tx, ty):
        ax.annotate("", xy=q, xytext=p, arrowprops=dict(arrowstyle="->", color=G, lw=1.0, ls="--"))
        ax.text(tx, ty, "«include»", fontsize=7.6, color=G, ha="center", va="center")
    inc((5.0, 6.1 + 0.41), (5.0, 7.15 - 0.41), 5.6, 6.62)
    inc((5.0, 7.15 + 0.41), (5.0, 8.2 - 0.41), 5.6, 7.67)
    inc((5.0 + W / 2, 6.1), (9.55 - W / 2, 7.55), 7.35, 7.0)
    inc((5.0 + W / 2, 5.05), (9.55 - W / 2, 7.45), 7.35, 5.95)
    save(fig, "UML01_cazuri_utilizare")


# ---------------------------------------------------------------- UML: secventa vanzarii
def secventa():
    fig, ax = canvas(15.4, 12.4)
    linii = [("Casier", 1.0), (":FrmVanzareBilet", 3.6), (":RezervareService", 6.3), (":BiletService", 8.9),
             (":ProceduriStocate", 11.4), ("SQL Server\n(baza autogara)", 13.9)]
    top = 11.8
    for t, x in linii:
        if t == "Casier":
            actor(ax, x, top - 0.55, "Casier")
        else:
            box(ax, x - 1.15, top - 0.75, 2.3, 0.7, text=t, fc=L, fs=8.8, bold=True, r=0.04)
        ax.plot([x, x], [top - 0.95 if t != "Casier" else top - 1.1, 0.3], color=GR, lw=1, ls="--")
    X = {t.split("\n")[0].strip(":"): x for t, x in linii}
    X["SQL"] = linii[-1][1]
    msgs = [
        ("Casier", "FrmVanzareBilet", "1: alege locul pe schema autobuzului", "m"),
        ("FrmVanzareBilet", "RezervareService", "2: RezervaLocAsync(locId)", "m"),
        ("RezervareService", "ProceduriStocate", "3: RezervaLocAsync(locId, utilizator, durata)", "m"),
        ("ProceduriStocate", "SQL", "4: EXEC sp_RezervaLoc", "m"),
        ("SQL", "SQL", "5: UPDLOCK pe loc; Status = Rezervat;\n    rând în RezervariProvizorii", "s"),
        ("SQL", "ProceduriStocate", "6: RezervareID, DataExpirare", "r"),
        ("ProceduriStocate", "RezervareService", "7: (RezervareID, DataExpirare)", "r"),
        ("RezervareService", "FrmVanzareBilet", "8: RezervareDto (pornește cronometrul)", "r"),
        ("Casier", "FrmVanzareBilet", "9: numele, telefonul, reducerea, plata; F2", "m"),
        ("FrmVanzareBilet", "BiletService", "10: ConfirmaVanzareAsync(rezervareId, date)", "m"),
        ("BiletService", "BiletService", "11: validări (nume obligatoriu,\n      format telefon, rol)", "s"),
        ("BiletService", "ProceduriStocate", "12: ConfirmaVanzareBiletAsync(...)", "m"),
        ("ProceduriStocate", "SQL", "13: EXEC sp_ConfirmaVanzareBilet", "m"),
        ("SQL", "SQL", "14: tranzacție: Bilet + Plată,\n      Loc = Ocupat, LogAudit", "s"),
        ("SQL", "ProceduriStocate", "15: BiletID, CodBilet, PretFinal", "r"),
        ("ProceduriStocate", "BiletService", "16: (BiletID, CodBilet, PretFinal)", "r"),
        ("BiletService", "FrmVanzareBilet", "17: BiletEmis", "r"),
        ("FrmVanzareBilet", "Casier", "18: mesajul cu codul biletului", "r"),
    ]
    y = top - 1.55
    for a, b, t, k in msgs:
        xa, xb = X[a], X[b]
        if k == "s":
            ax.annotate("", xy=(xa + 0.05, y - 0.35), xytext=(xa + 0.05, y),
                        arrowprops=dict(arrowstyle="-|>", color=G, lw=1.2, connectionstyle="bar,fraction=-0.9"))
            ax.text(xa - 0.2, y - 0.17, t, fontsize=8, ha="right", va="center", color=INK, linespacing=1.15)
            y -= 0.78
            continue
        ax.annotate("", xy=(xb, y), xytext=(xa, y), arrowprops=dict(arrowstyle="-|>" if k == "m" else "->", color=G if k == "m" else INK2,
                                                                     lw=1.2, ls="-" if k == "m" else "--"))
        ax.text((xa + xb) / 2, y + 0.14, t, fontsize=8, ha="center", va="center", color=INK)
        y -= 0.54
    save(fig, "UML03_secventa_vanzare")


if __name__ == "__main__":
    aria()
    ciclu()
    beneficiar()
    subsisteme()
    distribuit()
    cazuri()
    secventa()
