import subprocess, sys
from comun import *
d = ["digraph G {", ANTET, "rankdir=TB;", "{rank=same; CS; PS;}", "{rank=same; CT; CN;}",
  entitate("TR", "Transportatorul"), entitate("PS", "Pasagerul"), entitate("CN", "Conducerea<BR/>autogării"), entitate("CT", "Contabilitatea"),
  proces("DS", "Dispecerul", "orarul pe panou,<BR/>foile de parcurs"),
  proces("CS", "Casierul", "vânzarea pe bilete<BR/>tipizate"),
  depozit("R1", "H1", "Orarul curselor (hârtie / Excel)"),
  depozit("R2", "H2", "Registrul de vânzări al casei"),
  depozit("R3", "H3", "Registrul autobuzelor și ITP"),
  flux("TR", "DS", "orarul, autobuzele,\\nșoferii, actele ITP"),
  flux("DS", "R1", "orarul"), flux("DS", "R3", "date ITP"),
  flux("R1", "CS", "cursa, prețul\\n(copiat de mână)"),
  flux("PS", "CS", "cererea de bilet,\\nplata în numerar"), flux("CS", "PS", "biletul tipizat"),
  flux("CS", "R2", "seria biletului,\\nlocul, suma"),
  flux("R2", "CT", "raportul de casă\\n(la sfârșitul zilei)"),
  flux("R2", "CN", "totaluri lunare\\n(recopiate în Excel)"),
  flux("DS", "CS", "telefonic: locuri\\nvândute la alt ghișeu", 'style=dashed'),
  "}"]
open("flux_existent.dot", "w").write("\n".join(d))
subprocess.run(["dot", "-Tpng", "-Gdpi=200", "flux_existent.dot", "-o", sys.argv[1]], check=True)
