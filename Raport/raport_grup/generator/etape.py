# Etapele proiectarii dupa modelul cascada (figura din capitolul 1).
import sys
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import FancyBboxPatch
G, L, INK, INK2 = "#1e5b46", "#dde8e2", "#1b1b1b", "#52514e"
ETAPE = [
    ("1. Analiza", "studiul autogării, Sarcina tehnică", "17.09"),
    ("2. Proiectarea de ansamblu", "arhitectura, compartimentele", "17.09"),
    ("3. Proiectarea de detaliu", "schema BD, straturile .NET, ferestrele", "17–18.09"),
    ("4. Realizarea", "BD, backend, interfața", "17–19.09"),
    ("5. Testarea", "funcțională; stres, recuperare", "13.10; 21–22.10"),
    ("6. Punerea în funcțiune", "server, date de volum, documentația", "13.10–03.11"),
]
plt.rcParams["font.family"] = "DejaVu Sans"
fig, ax = plt.subplots(figsize=(10.5, 5.2), dpi=220)
ax.set_xlim(0, 10.5); ax.set_ylim(0.1, 6.4); ax.axis("off")
w, h, dx, dy = 3.6, 0.72, 1.35, 1.05
for i, (t, d, per) in enumerate(ETAPE):
    x, y = 0.1 + i * dx, 5.55 - i * dy
    ax.add_patch(FancyBboxPatch((x, y), w, h, boxstyle="round,pad=0.02,rounding_size=0.08",
                                fc=G if i in (0, 5) else L, ec=G, lw=1.4))
    c = "white" if i in (0, 5) else INK
    ax.text(x + 0.15, y + h * 0.66, t, fontsize=11.5, fontweight="bold", color=c, va="center")
    ax.text(x + 0.15, y + h * 0.27, d, fontsize=9.5, color=c, va="center")
    ax.text(x + w + 0.12, y + h / 2, per, fontsize=9.5, color=INK2, va="center")
    if i < len(ETAPE) - 1:
        ym = y - dy + h / 2
        ax.plot([x + 0.55, x + 0.55], [y, ym], color=G, lw=1.5, solid_capstyle="butt")
        ax.annotate("", xy=(x + dx - 0.02, ym), xytext=(x + 0.55, ym),
                    arrowprops=dict(arrowstyle="-|>", color=G, lw=1.5, shrinkA=0, shrinkB=0))
fig.savefig(sys.argv[1], facecolor="white", bbox_inches="tight")
