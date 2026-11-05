"""Rama ferestrelor (bara de titlu Windows 11) si compunerea machetelor: fereastra principala
cu sectiunea deschisa, dialoguri modale deasupra, mesaje de confirmare."""
import html
from winforms import T, parse, layout, render, icon, esc, cursor

FONT_LINK = ('<link rel="preconnect" href="https://fonts.gstatic.com">'
             '<link href="https://fonts.googleapis.com/css2?family=Barlow+Semi+Condensed:wght@400;600&family=Noto+Sans:wght@400;600&display=swap" rel="stylesheet">')
DEF_FONT = {"font": "Segoe UI", "size": 9.0}
TITLU_H = 31


def aplica(f, spec):
    """Suprascrierile care schimba asezarea (tab-ul ales, controale ascunse sau aratate)."""
    for k, v in spec.items():
        if k in f.all:
            for prop in ("_sel", "Visible"):
                if prop in v:
                    f.all[k].p[prop] = v[prop]


def _form(name, spec, size=None, design=None):
    f = parse(name)
    aplica(f, spec)
    cs = f.p.get("ClientSize", (800, 600))
    w, h = size or cs
    layout(f, 0, 0, w, h, DEF_FONT, design=design or cs)
    return f, render(f, spec), (w, h)


def rama(titlu, w, h, continut, dialog=True, activ=True):
    """O fereastra Windows 11: bara de titlu alba, iconita aplicatiei, butoanele din dreapta."""
    btn = ('<div style="width:46px;height:31px;display:flex;align-items:center;justify-content:center;color:#222">{}</div>')
    x = '<svg width="10" height="10" viewBox="0 0 10 10"><path d="M0.5 0.5l9 9M9.5 0.5l-9 9" stroke="#222" stroke-width="1"/></svg>'
    mn = '<svg width="10" height="10" viewBox="0 0 10 10"><path d="M0 5h10" stroke="#222" stroke-width="1"/></svg>'
    mx = '<svg width="10" height="10" viewBox="0 0 10 10"><rect x="0.5" y="0.5" width="9" height="9" fill="none" stroke="#222" stroke-width="1"/></svg>'
    butoane = (btn.format(x) if dialog else btn.format(mn) + btn.format(mx) + btn.format(x))
    ic = icon("bus-front", 16, T["Semnal"])
    return (f'<div style="position:relative;width:{w + 2}px;height:{h + TITLU_H + 2}px;box-sizing:border-box;border:1px solid {"#7A7A7A" if activ else "#AAA"};'
            f'background:#FFFFFF;box-shadow:0 8px 24px rgba(0,0,0,.28)">'
            f'<div style="height:{TITLU_H}px;display:flex;align-items:center;background:#FFFFFF;font-family:\'Segoe UI\',\'Noto Sans\',sans-serif;font-size:12px;color:#1A1A1A">'
            f'<div style="width:{0 if dialog else 26}px;padding-left:{10 if not dialog else 0}px;display:flex">{"" if dialog else ic}</div>'
            f'<div style="flex:1;padding-left:{12 if dialog else 8}px">{esc(titlu)}</div>{butoane}</div>'
            f'<div style="position:relative;width:{w}px;height:{h}px;overflow:hidden">{continut}</div></div>')


def dialog(name, spec=None, titlu=None, size=None):
    spec = spec or {}
    f, body, (w, h) = _form(name, spec, size)
    return rama(titlu or f.p.get("Text", name), w, h, body), (w + 2, h + TITLU_H + 2)


MENIU = {
    "Admin": None,
    "Casier": ["btnDashboard", "btnVanzare", "btnBilete"],
    "Pasager": ["btnVanzare", "btnBilete"],
}
ADMIN_ONLY = ["lblGrupAdministrare", "btnCurse", "btnTrasee", "btnStatii", "btnHarta", "btnAutobuze", "btnMentenanta",
              "btnSoferi", "btnReduceri", "btnUtilizatori", "lblGrupRapoarte", "btnRapoarte", "btnAudit"]


def principala(sectiune, buton, rol, utilizator, copil=None, spec=None, stare="Online", size=(1280, 760), overlay=None):
    """Fereastra principala FrmMain cu sectiunea `copil` deschisa in zona de continut."""
    spec = spec or {}
    ms = {"lblSectiune": {"text": sectiune.upper()}, "lblUtilizator": {"text": f"{utilizator}\n{rol}"},
          "indicatorConexiune": {"stare": stare, "text": {"Online": "Online", "Offline": "Mod local (offline)"}.get(stare, stare)},
          buton: {"activ": True}}
    if rol != "Admin":
        for b in ADMIN_ONLY:
            ms[b] = {"Visible": False}
    if rol == "Pasager":
        ms["btnDashboard"] = {"Visible": False}
        ms["btnVanzare"] = dict(ms.get("btnVanzare", {}), text=" Cumpără bilet")
        ms["btnBilete"] = dict(ms.get("btnBilete", {}), text=" Biletele mele")
    f = parse("FrmMain")
    for b, v in ms.items():
        if v.get("Visible") is False and b in f.all:
            f.all[b].p["Visible"] = False
    W, H = size
    layout(f, 0, 0, W, H, DEF_FONT)
    body = render(f, ms)
    if copil:
        pc = f.all["pnlContinut"]
        x, y, w, h = pc.rect
        pad = 20
        c = parse(copil)
        aplica(c, spec)
        cw, ch = w - 2 * pad, h - 2 * pad
        layout(c, 0, 0, cw, ch, DEF_FONT, design=c.p.get("ClientSize", (cw, ch)))
        inner = render(c, spec)
        body += f'<div style="position:absolute;left:{x + pad}px;top:{y + pad}px;width:{cw}px;height:{ch}px;overflow:hidden">{inner}</div>'
    for ov in overlay or []:
        body += f'<div style="position:absolute;left:{ov[1]}px;top:{ov[2]}px;z-index:30">{ov[0]}</div>'
    return rama("Autogara", W, H, body, dialog=False), (W + 2, H + TITLU_H + 2)


def intrebare(titlu, mesaj, confirma="Confirmă", icon_name="circle-alert", culoare=None, w=440, h=170, pericol=False):
    """Fereastra de intrebare comuna (FrmIntrebare)."""
    spec = {"lblMesaj": {"text": mesaj}, "btnConfirma": {"text": " " + confirma}}
    f, body, (fw, fh) = _form("FrmIntrebare", spec)
    return rama(titlu, fw, fh, body), (fw + 2, fh + TITLU_H + 2)


def mesaj_box(titlu, mesaj, tip="info", w=380, butoane=("OK",)):
    """MessageBox standard Windows (informare / eroare / avertisment)."""
    culoare = {"info": "#0063B1", "eroare": "#C42B1C", "avert": "#C19C00", "intreb": "#0063B1"}[tip]
    simbol = {"info": "i", "eroare": "✕", "avert": "!", "intreb": "?"}[tip]
    lines = sum(max(1, -(-len(l) // 50)) for l in mesaj.split("\n"))
    h = 60 + lines * 18 + 50
    body = (f'<div style="position:absolute;inset:0 0 50px 0;background:#FFFFFF;display:flex;gap:12px;padding:22px 20px;box-sizing:border-box;'
            f'font-family:\'Segoe UI\',\'Noto Sans\',sans-serif;font-size:12px;color:#1A1A1A;line-height:18px">'
            f'<div style="flex:none;width:32px;height:32px;border-radius:16px;background:{culoare};color:white;font:bold 18px sans-serif;display:flex;align-items:center;justify-content:center">{simbol}</div>'
            f'<div style="white-space:pre-wrap">{html.escape(mesaj)}</div></div>'
            f'<div style="position:absolute;left:0;right:0;bottom:0;height:50px;background:#F0F0F0;display:flex;justify-content:flex-end;align-items:center;padding-right:12px">'
            + "".join(f'<div style="width:86px;height:24px;margin-left:8px;border:1px solid {"#0067C0" if i == len(butoane) - 1 else "#ADADAD"};background:#FDFDFD;display:flex;align-items:center;justify-content:center;font-family:\'Segoe UI\',\'Noto Sans\',sans-serif;font-size:12px">{b}</div>' for i, b in enumerate(butoane)) + '</div>')
    return rama(titlu, w, h, body), (w + 2, h + TITLU_H + 2)


def pagina_html(continut, w, h, fundal="#FFFFFF", pad=14):
    return (f'<!doctype html><html><head><meta charset="utf-8">{FONT_LINK}<style>body{{margin:0;background:{fundal}}}'
            f'#shot{{position:relative;display:inline-block;padding:{pad}px}}</style></head><body>'
            f'<div id="shot"><div style="position:relative;width:{w}px;height:{h}px">{continut}</div></div></body></html>')
