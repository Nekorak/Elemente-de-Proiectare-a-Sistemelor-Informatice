# Raport la proiectul de grup

- `raport_proiect_grup.docx` — raportul după modelul colegiului (BD-423, verificat: Vieru Viorica).
- `capturi/` — capturile de ecran folosite în raport (codul figurii = numele fișierului).
- `generator/` — sursele care construiesc raportul (docx-js + postproces.py).

Construire: `IMG_DIR=<folderul cu capturi> SKIP_MISSING=1 bash generator/make.sh`
(make.sh folosește căi din containerul în care a fost generat; ajustează `cd` și IMG_DIR.)

Varianta 1: lipsesc încă 19 capturi din aplicație (FE10–FE28), care vor fi adăugate ulterior.
