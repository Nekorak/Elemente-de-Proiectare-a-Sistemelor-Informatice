# Raport la proiectul de grup

- `raport_proiect_grup.docx` — raportul după modelul colegiului (BD-423, verificat: Vieru Viorica).
- `capturi/` — capturile de ecran folosite în raport (codul figurii = numele fișierului).
- `generator/` — sursele care construiesc raportul (docx-js + postproces.py).

Construire: `IMG_DIR=<folderul cu capturi> SKIP_MISSING=1 bash generator/make.sh`
(make.sh folosește căi din containerul în care a fost generat; ajustează `cd` și IMG_DIR.)
`tabele_vechi.json` conține tabelele preluate din `raport.docx` (mediul de testare și cazurile de test), citite de `c4_testare.js`.
Diagrama Gantt: `python3 generator/gantt.py <rădăcina depozitului> capturi/GANTT_lucrari.png`.
Diagramele cerute de curriculumul S.07.O.022 (aria de acoperire, ciclul de viață, beneficiar–executant, subsistemele, sistemul distribuit, cazurile de utilizare și secvența vânzării): `python3 generator/curriculum_diagrame.py capturi` (matplotlib).
Diagrama de clase UML, generată din `App/Autogara.Domain`: `python3 generator/uml_clase.py <rădăcina depozitului> capturi/UML02_clase_domain.png` (Graphviz).

Varianta 1: lipsesc încă 19 capturi din aplicație (FE10–FE28), care vor fi adăugate ulterior.
