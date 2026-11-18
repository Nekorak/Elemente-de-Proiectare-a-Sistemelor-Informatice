# Documentele individuale ale membrilor echipei

Câte un document de 5 pagini pentru fiecare membru, construit din raportul de grup:

- pagina 1: proiectul pe scurt (tema, scopul, tehnologiile, echipa);
- paginile 2–3: proiectarea teoretică și arhitecturală (comune tuturor);
- paginile 4–5: contribuția individuală a membrului, cu capturi din `../raport_grup/capturi`.

| Fișier | Membru | Compartiment |
|---|---|---|
| `Gojinevschii_Dmitri_Baza_de_date.docx` | Gojinevschii Dmitri | Baza de date |
| `Hanganu_Sergiu_Backend_DevOps.docx` | Hanganu Sergiu | Backend și DevOps |
| `Crivenco_Alexandr_Frontend.docx` | Crivenco Alexandr | Frontend |
| `Sopivnic_Maxim_Testare.docx` | Sopivnic Maxim | Testare |

Construire: `bash generator/make.sh [dmitri|sergiu|alexandr|maxim]` (node + pachetul `docx`; LibreOffice și poppler doar pentru numărarea paginilor).
Textul comun este în `generator/comun.js`, iar partea fiecărui membru în `generator/membri.js`; generatorul folosește `lib.js` din `../raport_grup/generator`.
