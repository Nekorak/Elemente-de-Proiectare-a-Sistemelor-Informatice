# Documentele individuale ale membrilor echipei

Câte un document de 6 pagini pentru fiecare membru, construit din raportul de grup:

- pagina 1: proiectul pe scurt (tema, scopul, tehnologiile, echipa);
- paginile 2–4: proiectarea teoretică și arhitecturală (comune tuturor), pe scurt după cele 32 de puncte ale scenariului de proiectare;
- paginile 5–6: contribuția individuală a membrului, cu capturi din `../raport_grup/capturi`.

Corespondența cu scenariul de proiectare (numerele punctelor apar și în subtitluri):

| Subcapitolul | Punctele scenariului |
|---|---|
| 2.1. Obiectivele, etapele și metodologia | 1, 2, 3 |
| 2.2. Firma și studiul sistemului existent | 7, 8, 10, 11, 12, 13, 14 |
| 2.3. Caietul de sarcini | 9, 19, 20 |
| 2.4. Intrările și ieșirile | 21, 22 |
| 2.5. Proiectul de ansamblu și de detaliu | 15, 16, 18, 23 |
| 2.6. Riscurile, monitorizarea și eficiența economică | 4, 5, 6, 17 |
| 2.7. Punerea în funcțiune și documentația | 24–32 |

| Fișier | Membru | Compartiment |
|---|---|---|
| `Gojinevschii_Dmitri_Baza_de_date.docx` | Gojinevschii Dmitri | Baza de date |
| `Hanganu_Sergiu_Backend_DevOps.docx` | Hanganu Sergiu | Backend și DevOps |
| `Crivenco_Alexandr_Frontend.docx` | Crivenco Alexandr | Frontend |
| `Sopivnic_Maxim_Testare.docx` | Sopivnic Maxim | Testare |

Construire: `bash generator/make.sh [dmitri|sergiu|alexandr|maxim]` (node + pachetul `docx`; LibreOffice și poppler doar pentru numărarea paginilor).
Textul comun este în `generator/comun.js`, iar partea fiecărui membru în `generator/membri.js`; generatorul folosește `lib.js` din `../raport_grup/generator`.
