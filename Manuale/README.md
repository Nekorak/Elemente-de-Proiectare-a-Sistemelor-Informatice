# Manualele sistemului „Autogara”

| Fișierul | Pentru cine |
|---|---|
| `Manual_Utilizatori_Pasageri.docx` | pasagerii cu cont (cumpărarea și anularea biletelor) |
| `Manual_Operatori_Casieri.docx` | casierii de la ghișee |
| `Manual_Manageri.docx` | conducerea: curse, flotă, tarife, rapoarte, audit |
| `Manual_Administratori.docx` | administratorul: server, stații, conturi, joburi, backup |
| `Manual_Programator_DevOps.docx` | programatorii: cod, teste, Git, livrare |

Formatarea este cea a raportului de grup (A4, Times New Roman 12, 1,5 rânduri, cuprins automat,
figuri și tabele numerotate pe capitole, bibliografie IEEE).

## Regenerarea

```
cd Manuale/generator
npm install docx@9.7.1
bash make.sh                 # toate cinci; sau: bash make.sh casieri manageri
MACHETE=1 bash make.sh       # redesenează și machetele (din *.Designer.cs) și diagramele
```

Necesită Node.js, Python 3 (Pillow, matplotlib), Graphviz, LibreOffice, poppler-utils și Playwright (pentru machete).
Imaginile: capturile reale din `Raport/raport_grup/capturi`, iar machetele (`MK*`) și diagramele (`DG*`) din `Manuale/imagini`.
