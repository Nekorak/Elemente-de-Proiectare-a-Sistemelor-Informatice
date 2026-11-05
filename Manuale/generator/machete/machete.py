"""Lista machetelor folosite in manuale. Utilizare: python3 machete.py <folder_html>
Datele sunt cele din 11_Seed.sql (stații, trasee, autobuze, șoferi, conturi), pe aceleași zile ca
capturile reale din raport (13–14.10.2026)."""
import os
import sys
import pagina as P

OUT = sys.argv[1] if len(sys.argv) > 1 else "html"
os.makedirs(OUT, exist_ok=True)
M = {}


def mk(cod):
    def dec(fn):
        M[cod] = fn
        return fn
    return dec


def peste(baza, dlg, x, y):
    """Dialog modal asezat deasupra ferestrei principale."""
    return [(dlg[0], x, y)]


ADMIN = ("Admin", "Sergiu Hanganu")
CASIER = ("Casier", "Tatiana Rusu")
PASAGER = ("Pasager", "Ana Popa")

# ---------------- conturi și parole ----------------
@mk("MK01_recuperare_parola")
def _():
    return P.dialog("FrmRecuperareParola", {"txtUtilizator": {"value": "ana.popa", "focus": False},
                                            "txtEmail": {"value": "ana.popa@example.com", "focus": True}})


@mk("MK02_recuperare_confirmare")
def _():
    return P.mesaj_box("Autogara", "Cererea a fost înregistrată. Un administrator vă va comunica o parolă temporară.", "info", w=420)


@mk("MK03_schimba_parola")
def _():
    return P.dialog("FrmSchimbaParola", {"txtParolaVeche": {"value": "Temp4821x"}, "txtParolaNoua": {"value": "Autogara2026"},
                                         "txtConfirmare": {"value": "Autogara2026", "focus": True}})


@mk("MK04_schimba_parola_eroare")
def _():
    return P.mesaj_box("Autogara", "Parola trebuie să aibă cel puțin 8 caractere.\nParola trebuie să conțină cel puțin o cifră.\nConfirmarea nu coincide cu parola nouă.", "avert", w=420)


# ---------------- pasager ----------------
CURSE_PASAGER = [
    ["07:30", "10:05", "Chișinău (Nord) – Orhei – Bălți", "Chișinău – Autogara Nord", "P5", "Bălți", "120,00", "31", "SMB 115", "Planificată"],
    ["12:10", "14:40", "Chișinău (Nord) – Orhei – Bălți", "Chișinău – Autogara Nord", "P5", "Bălți", "120,00", "14", "KVA 318", "Planificată"],
]


@mk("MK05_pasager_cautare")
def _():
    return P.principala("Cumpără bilet", "btnVanzare", *PASAGER, "FrmCautareCurse", {
        "cmbPlecare": {"value": "Chișinău – Autogara Nord"}, "cmbSosire": {"value": "Bălți"}, "dtpData": {"value": "14.10.2026"},
        "gridCurse": {"rows": CURSE_PASAGER}, "lblNumar": {"text": "2 curse"}})


@mk("MK06_pasager_bilet_emis")
def _():
    return P.mesaj_box("Autogara", "Biletul a fost emis.\n\nCod: AG261014-4E6D4B5F\nLocul 22, 120,00 MDL", "info", w=340)


# ---------------- casier / bilete ----------------
@mk("MK07_confirmare_anulare_bilet")
def _():
    return P.mesaj_box("Autogara", "Anulați biletul AG261014-849CF994 (Andrei Ciobanu)?\nSe rambursează 42,00 MDL (50 %).", "intreb", w=420, butoane=("Da", "Nu"))


@mk("MK08_eroare_loc_ocupat")
def _():
    return P.mesaj_box("Autogara", "Locul nu mai este liber (a fost rezervat sau vândut între timp).", "avert", w=420)


@mk("MK09_eroare_rezervare_expirata")
def _():
    return P.mesaj_box("Autogara", "Rezervarea provizorie a expirat, iar locul a fost eliberat. Alegeți din nou locul.", "avert", w=420)


@mk("MK10_eroare_server")
def _():
    return P.mesaj_box("Autogara", "Serverul bazei de date nu poate fi contactat. Verificați conexiunea la internet și încercați din nou.", "eroare", w=420)


# ---------------- curse ----------------
CURSE = [
    ["14.10.2026", "06:45", "Chișinău – Cimișlia – Comrat – Cahul", "SMC 604", "155,00", "21", "În desfășurare"],
    ["14.10.2026", "06:50", "Hîncești – Chișinău", "DRL 902", "38,00", "11", "Finalizată"],
    ["14.10.2026", "07:30", "Chișinău (Nord) – Orhei – Bălți", "SMB 115", "120,00", "31", "Planificată"],
    ["14.10.2026", "08:15", "Chișinău – Strășeni – Călărași – Ungheni", "DRL 902", "85,00", "9", "Planificată"],
    ["14.10.2026", "09:00", "Chișinău (Nord) – Orhei – Soroca", "TRX 771", "140,00", "28", "Planificată"],
    ["14.10.2026", "11:00", "Bălți – Edineț", "KVB 427", "60,00", "13", "Planificată"],
    ["14.10.2026", "12:10", "Chișinău (Nord) – Orhei – Bălți", "KVA 318", "120,00", "14", "Planificată"],
    ["14.10.2026", "13:30", "Edineț – Bălți", "KVB 427", "60,00", "12", "Planificată"],
    ["14.10.2026", "13:30", "Ungheni – Călărași – Strășeni – Chișinău", "DRL 902", "85,00", "10", "Planificată"],
    ["14.10.2026", "14:00", "Cahul – Comrat – Cimișlia – Chișinău", "SMC 604", "155,00", "30", "Planificată"],
    ["14.10.2026", "15:00", "Bălți – Orhei – Chișinău (Nord)", "SMB 115", "120,00", "26", "Planificată"],
    ["14.10.2026", "15:30", "Soroca – Orhei – Chișinău (Nord)", "TRX 771", "140,00", "33", "Planificată"],
    ["14.10.2026", "17:50", "Bălți – Orhei – Chișinău (Nord)", "KVA 318", "120,00", "8", "Planificată"],
    ["14.10.2026", "18:10", "Chișinău – Hîncești", "DRL 902", "38,00", "15", "Planificată"],
]
CURSE_SPEC = {"gridLista": {"rows": CURSE, "sel": 2}, "dtpDeLa": {"value": "14.10.2026"}, "dtpPanaLa": {"value": "14.10.2026"},
              "cmbFiltruTraseu": {"value": "(toate traseele)"}, "lblNumar": {"text": "14 curse"}}


@mk("MK11_cursa_noua")
def _():
    d = P.dialog("FrmEditareCursa", {"cmbTraseu": {"value": "Chișinău (Nord) – Orhei – Bălți"}, "cmbAutobuz": {"value": "SMB 115 — Setra S 415 HD (49 locuri)"},
                                     "cmbSofer": {"value": "Țurcanu Vasile"}, "dtpData": {"value": "20.10.2026"}, "dtpPlecare": {"value": "07:30"},
                                     "dtpSosire": {"value": "10:05"}, "numPret": {"value": "120,00"}})
    return P.principala("Curse", "btnCurse", *ADMIN, "FrmAdminCurse", CURSE_SPEC, overlay=peste(None, d, 430, 150))


@mk("MK12_pasagerii_cursei")
def _():
    rows = [["1", "AG261013-7A1C02D4", "Ion Moraru", "+373 69 112 908", "120,00", "—", "Numerar", "Activ", "Tatiana Rusu"],
            ["2", "AG261012-5F0B3E21", "Elena Grosu", "+373 78 330 145", "60,00", "Elev (−50 %)", "Card", "Activ", "Victoria Ceban"],
            ["4", "AG261013-9C44E1A7", "Vadim Sandu", "+373 60 774 210", "84,00", "Pensionar (−30 %)", "Numerar", "Activ", "Ion Munteanu"],
            ["5", "AG261014-0B7E9D31", "Ana Popa", "+373 69 845 302", "120,00", "—", "Card", "Activ", "Ana Popa"],
            ["6", "AG261014-849CF994", "Andrei Ciobanu", "069 412 587", "84,00", "Pensionar (−30 %)", "Numerar", "Activ", "Tatiana Rusu"],
            ["9", "AG261011-3D2F88B0", "Mihaela Coma", "", "96,00", "Abonament (−20 %)", "Card", "Activ", "Natalia Bivol"],
            ["12", "AG261013-E1F0A6C3", "Gheorghe Plesca", "+373 79 205 661", "120,00", "—", "Numerar", "Activ", "Tatiana Rusu"],
            ["15", "AG261014-62AB71E8", "Cristina Melnic", "+373 68 901 337", "120,00", "—", "Card", "Activ", "Victoria Ceban"],
            ["18", "AG261012-A7C3D915", "Pavel Smirnov", "", "120,00", "—", "Numerar", "Activ", "Ion Munteanu"]]
    return P.dialog("FrmBileteCursa", {"lblTitlu": {"text": "Chișinău (Nord) – Orhei – Bălți · 14.10.2026, 07:30 · SMB 115"},
                                        "gridBilete": {"rows": rows, "sel": -1}, "lblSumar": {"text": "18 bilete active, 2 anulate sau rambursate"}},
                    titlu="Pasagerii cursei")


@mk("MK13_anulare_cursa_motiv")
def _():
    return P.dialog("FrmIntrebare", {"lblIntrebare": {"text": "Cursa Chișinău (Nord) – Orhei – Bălți, 14.10.2026 ora 07:30.\nToate biletele active (18) se rambursează integral. Motivul anulării:"},
                                     "txtRaspuns": {"value": "Autobuz defect, fără înlocuitor", "focus": True}},
                    titlu="Anularea cursei")


@mk("MK14_cursa_anulata")
def _():
    return P.mesaj_box("Autogara", "Cursa a fost anulată.\n18 bilete rambursate, în total 1 932,00 MDL.", "info", w=360)


# ---------------- trasee, stații ----------------
@mk("MK15_trasee")
def _():
    rows = [["Bălți – Edineț", "Bălți", "Edineț", "2", "Da"], ["Bălți – Orhei – Chișinău (Nord)", "Bălți", "Chișinău – Autogara Nord", "3", "Da"],
            ["Cahul – Comrat – Cimișlia – Chișinău", "Cahul", "Chișinău – Autogara Centrală", "4", "Da"],
            ["Chișinău (Nord) – Orhei – Bălți", "Chișinău – Autogara Nord", "Bălți", "3", "Da"],
            ["Chișinău (Nord) – Orhei – Soroca", "Chișinău – Autogara Nord", "Soroca", "3", "Da"],
            ["Chișinău – Cimișlia – Comrat – Cahul", "Chișinău – Autogara Centrală", "Cahul", "4", "Da"],
            ["Chișinău – Hîncești", "Chișinău – Autogara Centrală", "Hîncești", "2", "Da"],
            ["Chișinău – Strășeni – Călărași – Ungheni", "Chișinău – Autogara Centrală", "Ungheni", "4", "Da"],
            ["Edineț – Bălți", "Edineț", "Bălți", "2", "Da"], ["Hîncești – Chișinău", "Hîncești", "Chișinău – Autogara Centrală", "2", "Da"],
            ["Soroca – Orhei – Chișinău (Nord)", "Soroca", "Chișinău – Autogara Nord", "3", "Da"],
            ["Ungheni – Călărași – Strășeni – Chișinău", "Ungheni", "Chișinău – Autogara Centrală", "4", "Da"]]
    return P.principala("Trasee", "btnTrasee", *ADMIN, "FrmAdminTrasee", {"gridLista": {"rows": rows, "sel": 3}})


STATII = [["Bălți", "str. Ștefan cel Mare 2, Bălți", "P4", "Da"], ["Cahul", "str. Ștefan cel Mare 2A, Cahul", "P3", "Da"],
          ["Călărași", "str. Alexei Mateevici 2, Călărași", "P1", "Da"], ["Chișinău – Autogara Centrală", "str. Mitropolit Varlaam 58, Chișinău", "P3", "Da"],
          ["Chișinău – Autogara Nord", "str. Calea Moșilor 2/1, Chișinău", "P5", "Da"], ["Cimișlia", "str. Ștefan cel Mare 20, Cimișlia", "P1", "Da"],
          ["Comrat", "str. Lenin 150, Comrat", "P2", "Da"], ["Edineț", "str. Independenței 4, Edineț", "P1", "Da"],
          ["Hîncești", "str. Mihalcea Hîncu 140, Hîncești", "P1", "Da"], ["Orhei", "str. Vasile Mahu 150, Orhei", "P1", "Da"],
          ["Soroca", "str. Independenței 88, Soroca", "P2", "Da"], ["Strășeni", "str. Ștefan cel Mare 128, Strășeni", "P1", "Da"],
          ["Ungheni", "str. Națională 1, Ungheni", "P2", "Da"]]


@mk("MK16_statii")
def _():
    d = P.dialog("FrmEditareStatie", {"txtNume": {"value": "Orhei"}, "txtAdresa": {"value": "str. Vasile Mahu 150, Orhei"}, "txtPeron": {"value": "P1", "focus": True}})
    return P.principala("Stații", "btnStatii", *ADMIN, "FrmAdminStatii", {"gridLista": {"rows": STATII, "sel": 9}}, overlay=peste(None, d, 560, 250))


# ---------------- flota ----------------
AUTOBUZE = [["DRL 902", "Iveco Daily 50C18 Tourys", "22", "Activ", "12.06.2027", "Da"], ["KVA 318", "Mercedes-Benz Sprinter 516 CDI", "20", "Activ", "09.04.2027", "Da"],
            ["KVB 427", "Mercedes-Benz Sprinter 516 CDI", "20", "Activ", "23.10.2026", "Da"], ["ORH 208", "Isuzu Novo Lux", "31", "Service", "09.10.2026", "Da"],
            ["SMB 115", "Setra S 415 HD", "49", "Activ", "18.01.2027", "Da"], ["SMC 604", "Neoplan Tourliner N2216 SHD", "49", "Activ", "15.08.2027", "Da"],
            ["TRX 771", "MAN Lion's Coach R07", "51", "Activ", "16.12.2026", "Da"]]


@mk("MK17_autobuze")
def _():
    return P.principala("Autobuze", "btnAutobuze", *ADMIN, "FrmAdminAutobuze", {"gridLista": {"rows": AUTOBUZE, "sel": 4}})


@mk("MK18_autobuz_modifica")
def _():
    return P.dialog("FrmEditareAutobuz", {"lblTitlu": {"text": "Modificare autobuz"}, "txtNrInmatriculare": {"value": "SMB 115"}, "txtModel": {"value": "Setra S 415 HD"},
                                          "numCapacitate": {"value": "49"}, "cmbStatus": {"value": "Activ"}, "chkItp": {"checked": True},
                                          "dtpItp": {"value": "18.01.2027"}, "lblFisier": {"text": "Structura locurilor: Autobuze/autobuz_4.json"}})


LUCRARI = [["08.10.2026", "ORH 208", "Diagnoză motor — pierdere putere", "403 870", "Turbină defectă; piesa comandată, autobuz în service"],
           ["02.10.2026", "TRX 771", "Schimb ulei cutie de viteze", "541 330", ""],
           ["23.09.2026", "SMB 115", "Schimb ulei motor și filtre", "874 210", ""],
           ["09.09.2026", "KVB 427", "Înlocuire plăcuțe frână față", "287 930", "Discurile în toleranță"],
           ["31.08.2026", "SMC 604", "Înlocuire kit ambreiaj", "692 040", "Kit Sachs, volantă verificată"],
           ["15.08.2026", "SMC 604", "Inspecție tehnică periodică (ITP)", "689 870", "Admis"],
           ["13.08.2026", "KVA 318", "Schimb ulei motor și filtre", "312 450", "Ulei 5W-30, filtru ulei, aer și motorină"],
           ["01.08.2026", "SMB 115", "Reparație ușă pasageri spate", "866 980", "Înlocuit cilindru pneumatic"],
           ["07.07.2026", "DRL 902", "Revizie sistem climatizare", "204 300", "Încărcare freon, filtru habitaclu nou"],
           ["16.05.2026", "DRL 902", "Schimb anvelope (iarnă → vară)", "198 760", "Anvelope Continental VanContact 225/65 R16"]]


@mk("MK19_lucrare_noua")
def _():
    d = P.dialog("FrmEditareLucrare", {"cmbAutobuz": {"value": "KVB 427 — Mercedes-Benz Sprinter 516 CDI"}, "txtTipLucrare": {"value": "Schimb ulei motor și filtre"},
                                       "dtpData": {"value": "14.10.2026"}, "chkKm": {"checked": True}, "numKm": {"value": "291 640"},
                                       "txtObservatii": {"value": "Ulei 5W-30", "focus": True}})
    return P.principala("Mentenanță și ITP", "btnMentenanta", *ADMIN, "FrmAdminMentenanta", {"gridLucrari": {"rows": LUCRARI, "sel": -1}, "cmbFiltru": {"value": "(toate autobuzele)"}}, overlay=peste(None, d, 560, 160))


@mk("MK20_itp_alerte")
def _():
    rows = [["ORH 208", "Isuzu Novo Lux", "Service", "09.10.2026", "−5"], ["KVB 427", "Mercedes-Benz Sprinter 516 CDI", "Activ", "23.10.2026", "9"]]
    return P.principala("Mentenanță și ITP", "btnMentenanta", *ADMIN, "FrmAdminMentenanta", {"tabLista": {"_sel": 1}, "gridItp": {"rows": rows, "sel": 1}, "cmbFiltru": {"value": "(toate autobuzele)"}})


@mk("MK21_itp_nou")
def _():
    return P.dialog("FrmInregistrareItp", {"cmbAutobuz": {"value": "KVB 427 — Mercedes-Benz Sprinter 516 CDI"}, "dtpInspectie": {"value": "14.10.2026"},
                                           "dtpExpirare": {"value": "14.10.2027"}, "chkKm": {"checked": True}, "numKm": {"value": "291 640"}})


SOFERI = [["Bivol", "Petru", "PC 0284519", "+373 69 250 667", "Da"], ["Cebotari", "Iurie", "PC 0819274", "+373 69 997 014", "Da"],
          ["Grosu", "Nicolae", "PC 0527841", "+373 79 118 450", "Da"], ["Moraru", "Serghei", "PC 0610337", "+373 68 773 902", "Da"],
          ["Rotaru", "Veaceslav", "PC 0458963", "+373 60 331 725", "Da"], ["Sîrbu", "Andrei", "PC 0745102", "+373 78 604 381", "Da"],
          ["Țurcanu", "Vasile", "PC 0391254", "+373 69 402 118", "Da"]]


@mk("MK22_soferi")
def _():
    d = P.dialog("FrmEditareSofer", {"lblTitlu": {"text": "Modificare șofer"}, "txtNume": {"value": "Moraru"}, "txtPrenume": {"value": "Serghei"},
                                     "txtNrPermis": {"value": "PC 0610337"}, "txtTelefon": {"value": "+373 68 773 902", "focus": True}})
    return P.principala("Șoferi", "btnSoferi", *ADMIN, "FrmAdminSoferi", {"gridLista": {"rows": SOFERI, "sel": 3}}, overlay=peste(None, d, 560, 200))


@mk("MK23_reduceri")
def _():
    d = P.dialog("FrmEditareReducere", {"lblTitlu": {"text": "Modificare reducere"}, "txtDenumire": {"value": "Pensionar"}, "numProcent": {"value": "30"},
                                        "chkActiv": {"checked": True}})
    rows = [["Abonament", "20", "Da"], ["Elev", "50", "Da"], ["Pensionar", "30", "Da"]]
    return P.principala("Reduceri", "btnReduceri", *ADMIN, "FrmAdminReduceri", {"gridLista": {"rows": rows, "sel": 2}}, overlay=peste(None, d, 560, 200))


# ---------------- utilizatori ----------------
CONTURI = [["a.crivenco", "Alexandr Crivenco", "Admin", "a.crivenco@autogara.local", "", "Da"],
           ["ana.popa", "Ana Popa", "Pasager", "ana.popa@example.com", "+373 69 845 302", "Da"],
           ["d.gojinevschii", "Dmitri Gojinevschii", "Admin", "d.gojinevschii@autogara.local", "", "Da"],
           ["gheorghe.lungu", "Gheorghe Lungu", "Admin", "gheorghe.lungu@autogara.local", "+373 69 214 377", "Da"],
           ["ion.munteanu", "Ion Munteanu", "Casier", "ion.munteanu@autogara.local", "+373 60 947 233", "Da"],
           ["m.sopivnic", "Maxim Sopivnic", "Admin", "m.sopivnic@autogara.local", "", "Da"],
           ["mihai.botnaru", "Mihai Botnaru", "Pasager", "mihai.botnaru@example.com", "+373 78 290 614", "Da"],
           ["natalia.bivol", "Natalia Bivol", "Casier", "natalia.bivol@autogara.local", "+373 69 356 804", "Da"],
           ["rodica.cazacu", "Rodica Cazacu", "Admin", "rodica.cazacu@autogara.local", "+373 68 530 912", "Da"],
           ["s.hanganu", "Sergiu Hanganu", "Admin", "s.hanganu@autogara.local", "", "Da"],
           ["tatiana.rusu", "Tatiana Rusu", "Casier", "tatiana.rusu@autogara.local", "+373 69 781 046", "Da"],
           ["victoria.ceban", "Victoria Ceban", "Casier", "victoria.ceban@autogara.local", "+373 78 115 629", "Da"]]


@mk("MK24_utilizator_nou")
def _():
    d = P.dialog("FrmEditareUtilizator", {"txtNumeUtilizator": {"value": "elena.grosu"}, "txtNume": {"value": "Grosu"}, "txtPrenume": {"value": "Elena"},
                                          "txtEmail": {"value": "elena.grosu@autogara.local"}, "txtTelefon": {"value": "+373 69 508 221"},
                                          "cmbRol": {"value": "Casier"}, "txtParola": {"value": "Casier2026x", "focus": True}})
    return P.principala("Utilizatori", "btnUtilizatori", *ADMIN, "FrmAdminUtilizatori", {"gridLista": {"rows": CONTURI, "sel": 10}}, overlay=peste(None, d, 560, 140))


@mk("MK25_cereri_resetare")
def _():
    return P.principala("Utilizatori", "btnUtilizatori", *ADMIN, "FrmAdminUtilizatori",
                        {"tabLista": {"_sel": 1}, "tabConturi": {"_sel": 1}, "gridLista": {"rows": CONTURI},
                         "gridCereri": {"rows": [["ana.popa", "14.10.2026 09:42"], ["ion.munteanu", "13.10.2026 18:05"]]}})


@mk("MK26_parola_temporara")
def _():
    return P.mesaj_box("Autogara", "Parola temporară pentru ana.popa:\n\nK7m4-Rq2x-T9\n\nA fost copiată în clipboard. Comunicați-o utilizatorului; o poate schimba după autentificare.", "info", w=420)


@mk("MK27_confirmare_dezactivare")
def _():
    return P.mesaj_box("Autogara", "Dezactivați contul olga.cojocaru? Rezervările lui în curs se eliberează.", "intreb", w=420, butoane=("Da", "Nu"))


# ---------------- rapoarte ----------------
def raport(tip, grid, rows, total, extra=None):
    spec = {"cmbRaport": {"value": tip}, "gridVanzariZi": {"Visible": grid == "gridVanzariZi"}, grid: {"Visible": True, "rows": rows, "sel": -1},
            "lblTotal": {"text": total}, "dtpDeLa": {"value": "01.10.2026"}, "dtpPanaLa": {"value": "14.10.2026"}, "cmbTraseu": {"value": "(toate traseele)"}}
    for g in ("gridVanzariInterval", "gridOcupare", "gridComparativ"):
        if g != grid:
            spec[g] = {"Visible": False}
    spec.update(extra or {})
    return P.principala("Rapoarte", "btnRapoarte", *ADMIN, "FrmRapoarte", spec)


@mk("MK28_raport_interval")
def _():
    zile = [("01.10.2026", "Card", 96, 3, "10 512,00", "264,00"), ("01.10.2026", "Numerar", 141, 4, "15 233,00", "318,00"),
            ("02.10.2026", "Card", 104, 2, "11 380,00", "120,00"), ("02.10.2026", "Numerar", 152, 5, "16 744,00", "402,00"),
            ("03.10.2026", "Card", 121, 6, "13 905,00", "486,00"), ("03.10.2026", "Numerar", 176, 3, "19 210,00", "252,00"),
            ("04.10.2026", "Card", 133, 4, "15 020,00", "336,00"), ("04.10.2026", "Numerar", 188, 2, "20 640,00", "180,00"),
            ("05.10.2026", "Card", 98, 1, "10 864,00", "60,00"), ("05.10.2026", "Numerar", 139, 3, "15 112,00", "222,00"),
            ("06.10.2026", "Card", 101, 2, "11 145,00", "144,00"), ("06.10.2026", "Numerar", 147, 4, "16 038,00", "300,00"),
            ("07.10.2026", "Card", 109, 3, "12 076,00", "210,00"), ("07.10.2026", "Numerar", 158, 2, "17 290,00", "168,00")]
    def lei(s):
        return float(s.replace(" ", "").replace(",", "."))
    rows = [[d, p, str(b), str(r), i, rb, f"{lei(i) - lei(rb):,.2f}".replace(",", " ").replace(".", ",")] for d, p, b, r, i, rb in zile]
    net = sum(lei(i) - lei(rb) for d, p, b, r, i, rb in zile)
    tot = f"{sum(b for d, p, b, r, i, rb in zile):,}".replace(",", " ")
    return raport("Vânzări pe interval (pe zile)", "gridVanzariInterval", rows,
                  f"{tot} bilete vândute · încasări nete " + f"{net:,.2f}".replace(",", " ").replace(".", ",") + " MDL",
                  {"dtpPanaLa": {"value": "07.10.2026"}})


@mk("MK29_raport_ocupare")
def _():
    rows = [["14.10.2026", "06:45", "Chișinău – Cimișlia – Comrat – Cahul", "SMC 604", "În desfășurare", "49", "28", "57,1", "4 340,00"],
            ["14.10.2026", "06:50", "Hîncești – Chișinău", "DRL 902", "Finalizată", "22", "11", "50,0", "418,00"],
            ["14.10.2026", "07:30", "Chișinău (Nord) – Orhei – Bălți", "SMB 115", "Planificată", "49", "18", "36,7", "1 932,00"],
            ["14.10.2026", "08:15", "Chișinău – Strășeni – Călărași – Ungheni", "DRL 902", "Planificată", "22", "13", "59,1", "1 105,00"],
            ["14.10.2026", "09:00", "Chișinău (Nord) – Orhei – Soroca", "TRX 771", "Planificată", "51", "23", "45,1", "3 080,00"],
            ["14.10.2026", "11:00", "Bălți – Edineț", "KVB 427", "Planificată", "20", "7", "35,0", "390,00"],
            ["14.10.2026", "12:10", "Chișinău (Nord) – Orhei – Bălți", "KVA 318", "Planificată", "20", "6", "30,0", "720,00"],
            ["14.10.2026", "13:30", "Edineț – Bălți", "KVB 427", "Planificată", "20", "8", "40,0", "450,00"],
            ["14.10.2026", "14:00", "Cahul – Comrat – Cimișlia – Chișinău", "SMC 604", "Planificată", "49", "19", "38,8", "2 790,00"],
            ["14.10.2026", "15:00", "Bălți – Orhei – Chișinău (Nord)", "SMB 115", "Planificată", "49", "23", "46,9", "2 616,00"],
            ["14.10.2026", "15:30", "Soroca – Orhei – Chișinău (Nord)", "TRX 771", "Planificată", "51", "18", "35,3", "2 380,00"],
            ["14.10.2026", "17:50", "Bălți – Orhei – Chișinău (Nord)", "KVA 318", "Planificată", "20", "12", "60,0", "1 368,00"]]
    medie = sum(float(r[7].replace(",", ".")) for r in rows) / len(rows)
    inc = sum(float(r[8].replace(" ", "").replace(",", ".")) for r in rows)
    return raport("Ocuparea curselor", "gridOcupare", rows,
                  f"{len(rows)} curse · grad mediu de ocupare {medie:.1f}%".replace(".", ",") + " · încasări " + f"{inc:,.2f}".replace(",", " ").replace(".", ",") + " MDL",
                  {"dtpDeLa": {"value": "14.10.2026"}})


@mk("MK30_raport_comparativ")
def _():
    rows = [["Bălți – Edineț", "10.2026", "28", "212", "198", "11 980,00", "+7,1", "37,9", "+2,4"],
            ["Bălți – Orhei – Chișinău (Nord)", "10.2026", "56", "1 214", "1 140", "136 210,00", "+6,5", "52,3", "+3,1"],
            ["Cahul – Comrat – Cimișlia – Chișinău", "10.2026", "28", "734", "761", "107 420,00", "−3,5", "53,5", "−1,9"],
            ["Chișinău (Nord) – Orhei – Bălți", "10.2026", "56", "1 302", "1 188", "146 040,00", "+9,6", "55,0", "+4,8"],
            ["Chișinău (Nord) – Orhei – Soroca", "10.2026", "28", "690", "702", "91 700,00", "−1,7", "48,3", "−0,8"],
            ["Chișinău – Cimișlia – Comrat – Cahul", "10.2026", "28", "751", "720", "110 205,00", "+4,3", "54,7", "+2,2"],
            ["Chișinău – Hîncești", "10.2026", "28", "336", "301", "12 168,00", "+11,6", "54,5", "+5,7"],
            ["Chișinău – Strășeni – Călărași – Ungheni", "10.2026", "28", "402", "415", "32 470,00", "−3,1", "65,3", "−2,1"],
            ["Edineț – Bălți", "10.2026", "28", "198", "190", "11 220,00", "+4,2", "35,4", "+1,4"],
            ["Hîncești – Chișinău", "10.2026", "28", "355", "322", "12 920,00", "+10,2", "57,6", "+5,4"]]
    return raport("Comparație lunară pe trasee", "gridComparativ", rows, f"{len(rows)} rânduri (traseu × lună) pentru 2026",
                  {"dtpDeLa": {"value": "01.10.2026"}, "dtpPanaLa": {"value": "31.10.2026"}})


@mk("MK31_raport_salvat")
def _():
    return P.mesaj_box("Autogara", "Raportul a fost salvat. Îl deschideți acum (Excel)?", "intreb", w=380, butoane=("Da", "Nu"))


@mk("MK32_raport_arhivat")
def _():
    return P.mesaj_box("Autogara", "Raportul a fost arhivat pe file server:\nRapoarte/2026/10/vanzari-2026-10-14.csv", "info", w=420)


@mk("MK33_traseu_editare")
def _():
    d = P.dialog("FrmEditareTraseu", {"lblTitlu": {"text": "Modificare traseu — Chișinău (Nord) – Orhei – Bălți"},
                                      "txtDenumire": {"value": "Chișinău (Nord) – Orhei – Bălți"},
                                      "lstOpriri": {"items": ["Chișinău – Autogara Nord", "Orhei   (+44 km)", "Bălți   (+92 km)"], "selected": 1},
                                      "cmbStatie": {"value": "Soroca"}, "lblDistanta": {"text": "Distanța totală pe hartă: 136 km"}},
                 titlu="Modificare traseu — Chișinău (Nord) – Orhei – Bălți")
    return d


@mk("MK34_audit_itp")
def _():
    rows = [["14.10.2026 07:00:02", "", "ALERTĂ ITP: KVB 427 (Mercedes-Benz Sprinter 516 CDI) expiră la 23.10.2026 — 9 zile rămase", "Autobuze", "3"],
            ["14.10.2026 07:00:02", "", "ALERTĂ ITP expirat: ORH 208 (Isuzu Novo Lux), expirat la 09.10.2026", "Autobuze", "5"],
            ["13.10.2026 07:00:01", "", "ALERTĂ ITP: KVB 427 (Mercedes-Benz Sprinter 516 CDI) expiră la 23.10.2026 — 10 zile rămase", "Autobuze", "3"],
            ["13.10.2026 07:00:01", "", "ALERTĂ ITP expirat: ORH 208 (Isuzu Novo Lux), expirat la 09.10.2026", "Autobuze", "5"],
            ["12.10.2026 07:00:03", "", "ALERTĂ ITP: KVB 427 (Mercedes-Benz Sprinter 516 CDI) expiră la 23.10.2026 — 11 zile rămase", "Autobuze", "3"],
            ["12.10.2026 07:00:03", "", "ALERTĂ ITP expirat: ORH 208 (Isuzu Novo Lux), expirat la 09.10.2026", "Autobuze", "5"]]
    return P.principala("Jurnal audit", "btnAudit", *ADMIN, "FrmAudit",
                        {"gridAudit": {"rows": rows, "sel": -1}, "dtpDeLa": {"value": "12.10.2026"}, "dtpPanaLa": {"value": "14.10.2026"},
                         "txtText": {"value": "ITP"}, "numMax": {"value": "500"}, "lblNumar": {"text": "6 înregistrări"}})


if __name__ == "__main__":
    only = sys.argv[2:] or list(M)
    for cod in only:
        h, (w, hh) = M[cod]()
        open(os.path.join(OUT, cod + ".html"), "w", encoding="utf8").write(P.pagina_html(h, w, hh))
    print("machete:", len(only))
