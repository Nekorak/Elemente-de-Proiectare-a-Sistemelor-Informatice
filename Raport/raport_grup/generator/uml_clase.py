# Diagrama de clase UML a entitatilor din Autogara.Domain, generata din codul sursa (Graphviz).
# Utilizare: python3 uml_clase.py <radacina depozitului> <fisier.png>
import os
import re
import subprocess
import sys

root, out = sys.argv[1], sys.argv[2]
d = os.path.join(root, "App", "Autogara.Domain", "Entitati")
G, L = "#1e5b46", "#dde8e2"
OMIS = {"CreatLa", "ModificatLa", "RowVersion"}
PROP = re.compile(r"public\s+([\w<>?\[\]]+)\s+(\w+)\s*\{\s*get;")

clase = {}
for f in sorted(os.listdir(d)):
    if f.endswith(".cs"):
        src = open(os.path.join(d, f), encoding="utf8").read()
        nume = re.search(r"public class (\w+)", src).group(1)
        clase[nume] = PROP.findall(src)

atribute, nav = {}, []
for c, props in clase.items():
    atribute[c] = []
    for t, n in props:
        baza = re.sub(r"[?]|List<|>", "", t)
        if baza in clase:
            if t.startswith("List<"):
                nav.append((c, baza, n, "multi"))
            else:
                nav.append((c, baza, n, "unu"))
        elif n not in OMIS:
            atribute[c].append(f"{n} : {t}")

def esc(s):
    return s.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")

lines = ['digraph clase {', 'graph [rankdir=TB, splines=true, nodesep=0.25, ranksep=0.55, fontname="DejaVu Sans", dpi=200];',
         f'node [shape=plain, fontname="DejaVu Sans", fontsize=9];', f'edge [fontname="DejaVu Sans", fontsize=8, color="{G}", arrowhead=none];']
for c, at in atribute.items():
    rows = "".join(f'<TR><TD ALIGN="LEFT" BALIGN="LEFT">{esc(a)}</TD></TR>' for a in at)
    lines.append(f'{c} [label=<<TABLE BORDER="1" CELLBORDER="0" CELLSPACING="0" CELLPADDING="2" COLOR="{G}">'
                 f'<TR><TD BGCOLOR="{L}"><B>{c}</B></TD></TR><HR/>{rows}</TABLE>>];')
# asocierile: din navigarile simple (copil -> parinte, cheia externa e la copil) si din listele fara navigare inversa.
# Statie-Nod si RezervareProvizorie-Loc sunt 1:1 (UNIQUE pe cheia externa in 03_Tables.sql).
UNU_LA_UNU = {("Statie", "Nod"), ("RezervareProvizorie", "Loc")}
perechi = set()
asocieri = []
for c, p, n, k in nav:
    if k != "unu" or (p, c) in UNU_LA_UNU:
        continue  # listele se trateaza mai jos; navigarea inversa a unei relatii 1:1 e deja desenata
    perechi.add(frozenset((c, p)))
    asocieri.append((c, p, n, "0..1" if (c, p) in UNU_LA_UNU else "*"))
for c, p, n, k in nav:
    if k == "multi" and frozenset((c, p)) not in perechi:
        perechi.add(frozenset((c, p)))
        asocieri.append((p, c, "", "*"))
for c, p, n, m in asocieri:
    lines.append(f'{c} -> {p} [taillabel="{m}", headlabel="1", label=" {n} ", labelfontsize=8];')
lines.append("}")
dot = "\n".join(lines)
open(out.replace(".png", ".dot"), "w").write(dot) if os.environ.get("PASTREAZA_DOT") else None
subprocess.run(["dot", "-Tpng", "-o", out], input=dot.encode("utf8"), check=True)
print("clase:", len(clase), "asocieri:", len(asocieri))
