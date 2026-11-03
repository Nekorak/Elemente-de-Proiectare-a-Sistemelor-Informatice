import subprocess, sys
from comun import *
out_ctx, out_n1 = sys.argv[1], sys.argv[2]

ctx = ["digraph G {", ANTET, "rankdir=TB; nodesep=0.5; ranksep=0.9;",
  entitate("PS", "Pasagerul"), entitate("TR", "Transportatorul"),
  entitate("CN", "Conducerea<BR/>autogării"), entitate("CT", "Contabilitatea"),
  proces("S", "0", "Sistemul informatic<BR/>al autogării"),
  "{rank=same; PS; TR;}", "{rank=same; CN; CT;}",
  flux("PS", "S", "stațiile, data, locul ales,\\ndatele pasagerului, plata,\\ncererea de anulare"),
  flux("S", "PS", "cursele, prețul, biletul,\\nsuma rambursată"),
  flux("TR", "S", "orarul, traseele,\\nautobuzele, șoferii, ITP"),
  flux("S", "TR", "gradul de ocupare\\nal curselor"),
  flux("CN", "S", "tarifele, reducerile,\\nconturile utilizatorilor"),
  flux("S", "CN", "rapoarte de vânzări și ocupare,\\njurnalul de audit, alerte ITP"),
  flux("S", "CT", "încasările zilnice pe metode\\nde plată, rambursările (CSV)"),
  "}"]
open("dfd_context.dot", "w").write("\n".join(ctx))
subprocess.run(["dot", "-Tpng", "-Gdpi=200", "dfd_context.dot", "-o", out_ctx], check=True)

n1 = ["digraph G {", ANTET, "rankdir=TB; nodesep=0.2; ranksep=0.5; newrank=true;",
  entitate("PS", "Pasagerul"), entitate("TR", "Transportatorul"),
  entitate("CN", "Conducerea<BR/>autogării"), entitate("CT", "Contabilitatea"),
  proces("P1", "1", "Căutarea<BR/>curselor"),
  proces("P2", "2", "Rezervarea și<BR/>vânzarea biletului"),
  proces("P3", "3", "Anularea și<BR/>rambursarea"),
  proces("P4", "4", "Planificarea<BR/>traseelor și curselor"),
  proces("P5", "5", "Evidența flotei,<BR/>a șoferilor și a ITP"),
  proces("P6", "6", "Rapoarte și<BR/>jurnal de audit"),
  depozit("D1", "D1", "Trasee, stații, hartă"),
  depozit("D2", "D2", "Curse"),
  depozit("D3", "D3", "Locuri și rezervări provizorii"),
  depozit("D4", "D4", "Bilete și plăți"),
  depozit("D5", "D5", "Autobuze, mentenanță, șoferi"),
  depozit("D6", "D6", "Jurnalul de audit"),
  depozit("D7", "D7", "Fișiere JSON (file server)"),
  "{rank=same; PS; TR;}", "{rank=same; P1; P2; P3;}", "{rank=same; P4; P5;}", "{rank=same; CN; CT;}",
  flux("TR", "P4", "orarul, traseele"), flux("TR", "P5", "autobuzele, șoferii,\\ndate ITP"),
  flux("P5", "D5", "autobuz, șofer"), flux("P5", "D7", "schema locurilor"),
  flux("P4", "D1", "trasee, opriri"), flux("P4", "D2", "cursă nouă"), flux("P4", "D3", "locurile cursei"),
  flux("D5", "P4", "autobuz, șofer"),
  flux("PS", "P1", "stațiile, data"), flux("D2", "P1", ""), flux("D1", "P1", ""),
  flux("P1", "PS", "cursele, prețul,\\nlocurile libere"),
  flux("PS", "P2", "locul, datele,\\nplata"), flux("D7", "P2", "schema\\nautobuzului"),
  flux("P2", "D3", "rezervare,\\nloc ocupat"), flux("P2", "D4", "bilet, plată"),
  flux("P2", "PS", "biletul"),
  flux("PS", "P3", "codul biletului"), flux("D4", "P3", "bilet, plăți"),
  flux("P3", "D4", "rambursare"), flux("P3", "D3", "loc eliberat"), flux("P3", "PS", "suma rambursată"),
  flux("P2", "D6", "", "style=dashed"), flux("P3", "D6", "", "style=dashed"),
  flux("D4", "P6", ""), flux("D2", "P6", ""), flux("D6", "P6", ""), flux("D5", "P6", "ITP"),
  flux("P6", "CN", "rapoarte, audit,\\nalerte ITP"), flux("P6", "CT", "încasări,\\nrambursări"),
  "}"]
open("dfd_nivel1.dot", "w").write("\n".join(n1))
subprocess.run(["dot", "-Tpng", "-Gdpi=200", "dfd_nivel1.dot", "-o", out_n1], check=True)
from PIL import Image
Image.open(out_n1).rotate(90, expand=True).save(out_n1)
