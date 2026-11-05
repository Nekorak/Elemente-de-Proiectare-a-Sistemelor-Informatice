"""Machete ale ferestrelor WinForms, desenate direct din fisierele *.Designer.cs.

Fisierul Designer descrie complet aspectul: controalele, pozitia, marimea, docking-ul, textele,
fonturile si culorile. Aici el este citit, asezat cu aceleasi reguli ca WinForms (Dock in ordinea
inversa adaugarii, FlowLayoutPanel, Anchor) si desenat in HTML cu culorile din Ui/Tema.cs.
Datele afisate (randurile tabelelor, valorile campurilor) vin din specificatia fiecarei machete.
"""
import html
import os
import re

ROOT = os.environ.get("APP_DIR", os.path.join(os.path.dirname(__file__), "..", "..", "..", "App", "Autogara.WinForms"))
ICON_DIR = os.path.join(ROOT, "Resurse", "Iconite")

# ---------- Tema (Ui/Tema.cs) ----------
T = dict(Primar="#1E5B46", PrimarInchis="#154535", PrimarDeschis="#DDEBE3", Semnal="#E8A317", SemnalDeschis="#F8EAC4",
         Fundal="#EEECE6", Suprafata="#FAF9F6", Bordura="#D4D0C6", Text="#22221F", TextSecundar="#6E6A60",
         Succes="#2E7D32", Avertisment="#B7791F", Eroare="#B3261E", Meniu="#1C2420", MeniuActiv="#26312B",
         MeniuText="#C9CFC8", MeniuGrup="#7F8A83")

FONT_MAP = {"Segoe UI": "'Segoe UI','Noto Sans',sans-serif", "Bahnschrift": "'Bahnschrift','Barlow Semi Condensed',sans-serif",
            "Bahnschrift SemiBold": "'Bahnschrift','Barlow Semi Condensed',sans-serif", "Consolas": "'JetBrains Mono',monospace"}


# ---------- parsarea fisierului Designer ----------
def _statements(body):
    out, cur, i, in_str = [], [], 0, False
    while i < len(body):
        c = body[i]
        if in_str:
            cur.append(c)
            if c == "\\":
                cur.append(body[i + 1]); i += 2; continue
            if c == '"':
                in_str = False
        elif c == '"':
            in_str = True; cur.append(c)
        elif c == ";":
            out.append("".join(cur).strip()); cur = []
        else:
            cur.append(c)
        i += 1
    return [re.sub(r"\s+", " ", s) for s in out if s]


def _str(s):
    return bytes(s[1:-1], "utf8").decode("unicode_escape").encode("latin1").decode("utf8") if "\\" in s else s[1:-1]


def _args(s):
    return [a.strip() for a in re.split(r",(?![^(]*\))", s)]


def value(v):
    v = v.strip()
    if v.startswith('"'):
        return _str(v)
    if v in ("true", "false"):
        return v == "true"
    m = re.match(r"new (?:System\.Drawing\.)?(Point|Size|SizeF|Padding)\((.*)\)$", v)
    if m:
        nums = [float(x.rstrip("Ff")) for x in _args(m.group(2))]
        if m.group(1) == "Padding" and len(nums) == 1:
            nums = nums * 4
        return tuple(nums)
    m = re.match(r"new (?:System\.Drawing\.)?Font\((.*)\)$", v)
    if m:
        a = _args(m.group(1))
        style = a[2] if len(a) > 2 else ""
        return {"font": _str(a[0]), "size": float(a[1].rstrip("Ff")), "bold": "Bold" in style}
    m = re.match(r"Color\.FromArgb\((.*)\)$", v)
    if m:
        n = [int(x) for x in _args(m.group(1))]
        return "#%02X%02X%02X" % tuple(n[-3:])
    m = re.match(r"(?:System\.Drawing\.)?Color\.(\w+)$", v)
    if m:
        return {"White": "#FFFFFF", "Black": "#000000", "Transparent": None}.get(m.group(1), m.group(1).lower())
    if "|" in v:
        return {p.split(".")[-1] for p in v.split("|")}
    if re.match(r"^-?\d+(\.\d+)?[FfMm]?$", v):
        return float(v.rstrip("FfMm"))
    return v.split(".")[-1]


class Ctl:
    def __init__(self, name, typ):
        self.name, self.type, self.p, self.children, self.columns, self.items = name, typ, {}, [], [], []
        self.rect = None

    def __repr__(self):
        return f"<{self.type} {self.name}>"


def parse(form):
    src = open(os.path.join(ROOT, "Formulare", form + ".Designer.cs"), encoding="utf-8-sig").read()
    body = src[src.index("InitializeComponent()"):src.index("#endregion")]
    body = body[body.index("{") + 1:]
    body = "\n".join(l for l in body.split("\n") if not l.strip().startswith("//"))
    ctl = {"this": Ctl("this", "Form")}
    for st in _statements(body):
        m = re.match(r"^(\w+) = new ([\w.]+)\((.*)\)$", st)
        if m and m.group(1) not in ("components",) and m.group(2).split(".")[-1] not in ("Size", "Point", "SizeF", "Padding", "Font"):
            ctl[m.group(1)] = Ctl(m.group(1), m.group(2).split(".")[-1])
            continue
        m = re.match(r"^(?:(\w+)\.)?Controls\.Add\((\w+)\)$", st)
        if m:
            (ctl[m.group(1)] if m.group(1) else ctl["this"]).children.append(ctl[m.group(2)])
            continue
        m = re.match(r"^(\w+)\.TabPages\.Add\((\w+)\)$", st)
        if m:
            ctl[m.group(1)].children.append(ctl[m.group(2)])
            continue
        m = re.match(r"^(\w+)\.Columns\.AddRange\(new \w+\[\] \{(.*)\}\)$", st)
        if m:
            ctl[m.group(1)].columns = [ctl[x.strip()] for x in m.group(2).split(",") if x.strip()]
            continue
        m = re.match(r"^(\w+)\.Items\.AddRange\(new object\[\] \{(.*)\}\)$", st)
        if m:
            ctl[m.group(1)].items = [_str(x.strip()) for x in re.findall(r'"(?:[^"\\]|\\.)*"', m.group(2))]
            continue
        m = re.match(r"^(?:(\w+)\.)?([\w.]+) = (.+)$", st)
        if m:
            owner = m.group(1) or "this"
            if owner in ctl:
                try:
                    ctl[owner].p[m.group(2)] = value(m.group(3))
                except Exception:
                    ctl[owner].p[m.group(2)] = m.group(3)
    f = ctl["this"]
    f.all = ctl
    return f


# ---------- asezarea (layout) ----------
DEF_SIZE = {"Label": (100, 23), "TextBox": (100, 23), "ComboBox": (121, 23), "DateTimePicker": (200, 23),
            "NumericUpDown": (120, 23), "CheckBox": (104, 19), "RadioButton": (104, 19), "ButonIcon": (75, 23),
            "Panel": (200, 100), "FlowLayoutPanel": (200, 100), "IconImagine": (24, 24), "CardKpi": (220, 96),
            "IndicatorConexiune": (190, 24), "GridAutogara": (240, 150), "GroupBox": (200, 100), "TabControl": (200, 100),
            "LinkLabel": (100, 23), "ListBox": (120, 94)}


def font_of(c, inherited):
    f = c.p.get("Font")
    return f if isinstance(f, dict) else inherited


def text_w(text, font):
    px = font["size"] * 96 / 72
    k = 0.50 if "Bahnschrift" in font["font"] else 0.53
    return max(len(l) for l in text.split("\n")) * px * k if text else 0


def pref_size(c, font):
    """Marimea preferata a unui control AutoSize (aproximare dupa lungimea textului)."""
    w, h = c.p.get("Size", DEF_SIZE.get(c.type, (100, 23)))
    f = font_of(c, font)
    if c.p.get("AutoSize") and c.type in ("Label", "LinkLabel", "CheckBox", "RadioButton"):
        pad = c.p.get("Padding", (0, 0, 0, 0))
        tw = text_w(c.p.get("Text", ""), f) + (18 if c.type in ("CheckBox", "RadioButton") else 0) + pad[0] + pad[2]
        lines = c.p.get("Text", "").count("\n") + 1
        h = round(f["size"] * 96 / 72 * 1.35 * lines) + pad[1] + pad[3]
        w = round(tw) + 4
        if c.p.get("MaximumSize") and c.p["MaximumSize"][0] > 0:
            mw = c.p["MaximumSize"][0]
            if w > mw:
                lines += int(w // mw)
                w = mw
                h = round(f["size"] * 96 / 72 * 1.35 * lines) + pad[1] + pad[3]
    if c.type == "FlowLayoutPanel" and c.p.get("AutoSize"):
        vis = [k for k in c.children if visible(k)]
        pad = c.p.get("Padding", (0, 0, 0, 0))
        sizes = [(pref_size(k, f), k.p.get("Margin", (3, 3, 3, 3))) for k in vis]
        if c.p.get("FlowDirection") in ("TopDown", "BottomUp"):
            w = max([s[0] + m[0] + m[2] for s, m in sizes] or [0]) + pad[0] + pad[2]
            h = sum(s[1] + m[1] + m[3] for s, m in sizes) + pad[1] + pad[3]
        else:
            w = sum(s[0] + m[0] + m[2] for s, m in sizes) + pad[0] + pad[2]
            h = max([s[1] + m[1] + m[3] for s, m in sizes] or [0]) + pad[1] + pad[3]
    return (w, h)


def visible(c):
    return c.p.get("Visible", True) is not False


def layout(c, x, y, w, h, font, design=None):
    c.rect = (x, y, w, h)
    c.font = font_of(c, font)
    pad = c.p.get("Padding", (0, 0, 0, 0)) if c.type in ("Form", "Panel", "FlowLayoutPanel", "TabPage", "GroupBox") else (0, 0, 0, 0)
    cx, cy, cw, ch = x + pad[0], y + pad[1], w - pad[0] - pad[2], h - pad[1] - pad[3]
    if c.type == "GroupBox":
        cy += 16; ch -= 16
    if c.type == "TabControl":
        sel = c.p.get("_sel", 0)
        for i, pg in enumerate(c.children):
            pg.p["Visible"] = i == sel
            layout(pg, x + 4, y + 26, w - 8, h - 30, c.font)
        return
    # marimea din Designer a containerului (pentru Anchor)
    dw, dh = design if design else c.p.get("Size", (w, h))[:2]
    kids = [k for k in c.children if visible(k)]
    if c.type == "FlowLayoutPanel":
        direction = c.p.get("FlowDirection", "LeftToRight")
        px, py = (cx + cw if direction == "RightToLeft" else cx), cy
        rowh = 0
        for k in kids:
            m = k.p.get("Margin", (3, 3, 3, 3))
            kw, kh = pref_size(k, c.font)
            if direction == "TopDown":
                layout(k, px + m[0], py + m[1], kw, kh, c.font)
                py += m[1] + kh + m[3]
            elif direction == "RightToLeft":
                px -= m[2] + kw
                layout(k, px, py + m[1], kw, kh, c.font)
                px -= m[0]
            else:
                if c.p.get("WrapContents", True) and px + m[0] + kw > cx + cw and px > cx:
                    px, py = cx, py + rowh; rowh = 0
                layout(k, px + m[0], py + m[1], kw, kh, c.font)
                px += m[0] + kw + m[2]; rowh = max(rowh, m[1] + kh + m[3])
        return
    rem = [cx, cy, cw, ch]
    for k in reversed(kids):
        d = k.p.get("Dock")
        if d in (None, "None"):
            continue
        kw, kh = pref_size(k, c.font)
        rx, ry, rw, rh = rem
        if d == "Top":
            layout(k, rx, ry, rw, kh, c.font); rem = [rx, ry + kh, rw, rh - kh]
        elif d == "Bottom":
            layout(k, rx, ry + rh - kh, rw, kh, c.font); rem = [rx, ry, rw, rh - kh]
        elif d == "Left":
            layout(k, rx, ry, kw, rh, c.font); rem = [rx + kw, ry, rw - kw, rh]
        elif d == "Right":
            layout(k, rx + rw - kw, ry, kw, rh, c.font); rem = [rx, ry, rw - kw, rh]
        elif d == "Fill":
            layout(k, rx, ry, rw, rh, c.font)
    for k in kids:
        if k.p.get("Dock") not in (None, "None"):
            continue
        lx, ly = k.p.get("Location", (0, 0))
        kw, kh = pref_size(k, c.font)
        an = k.p.get("Anchor", {"Top", "Left"})
        an = an if isinstance(an, set) else {an}
        ddx, ddy = (w - dw), (h - dh)
        if "Right" in an and "Left" in an:
            kw += ddx
        elif "Right" in an:
            lx += ddx
        if "Bottom" in an and "Top" in an:
            kh += ddy
        elif "Bottom" in an:
            ly += ddy
        layout(k, x + lx, y + ly, kw, kh, c.font)


# ---------- desenarea ----------
_icon_cache = {}


def icon(name, size, color):
    if not name:
        return ""
    if name not in _icon_cache:
        p = os.path.join(ICON_DIR, name + ".svg")
        if not os.path.exists(p):
            return ""
        s = open(p, encoding="utf8").read()
        s = re.sub(r"<!--.*?-->", "", s, flags=re.S)
        _icon_cache[name] = s
    s = _icon_cache[name]
    s = re.sub(r'width="24"', f'width="{size}"', s, 1)
    s = re.sub(r'height="24"', f'height="{size}"', s, 1)
    return s.replace("currentColor", color).replace('stroke-width="2"', 'stroke-width="2"')


def css_font(f):
    fam = FONT_MAP.get(f["font"], "'Segoe UI','Noto Sans',sans-serif")
    weight = 600 if ("SemiBold" in f["font"] or f.get("bold")) else 400
    return f"font-family:{fam};font-size:{f['size'] * 96 / 72:.2f}px;font-weight:{weight};"


def esc(t):
    return html.escape(str(t)).replace("\n", "<br>")


ALIGN = {"TopLeft": ("flex-start", "flex-start", "left"), "MiddleLeft": ("center", "flex-start", "left"),
         "BottomLeft": ("flex-end", "flex-start", "left"), "MiddleCenter": ("center", "center", "center"),
         "TopCenter": ("flex-start", "center", "center"), "MiddleRight": ("center", "flex-end", "right"),
         "TopRight": ("flex-start", "flex-end", "right"), "BottomCenter": ("flex-end", "center", "center")}


def render(c, spec, ox=0, oy=0, back=None):
    """Intoarce HTML pentru control si copiii lui. spec = suprascrierile din machetă."""
    if not visible(c):
        return ""
    s = spec.get(c.name, {})
    if s.get("Visible") is False:
        return ""
    x, y, w, h = c.rect
    p = dict(c.p, **{k: v for k, v in s.items() if k[0].isupper()})
    f = c.font if "Font" not in s else s["Font"]
    bg = p.get("BackColor") if isinstance(p.get("BackColor"), str) and p.get("BackColor", "").startswith("#") else None
    fg = p.get("ForeColor") if isinstance(p.get("ForeColor"), str) and str(p.get("ForeColor")).startswith("#") else T["Text"]
    text = p.get("Text", "")
    if "text" in s:
        text = s["text"]
    pos = f"position:absolute;left:{x - ox}px;top:{y - oy}px;width:{w}px;height:{h}px;"
    out = []
    t = c.type
    if t in ("Form", "Panel", "TabPage", "FlowLayoutPanel", "GroupBox"):
        style = pos + (f"background:{bg};" if bg else "") + ("box-sizing:border-box;border:1px solid " + T["Bordura"] + ";" if p.get("BorderStyle") == "FixedSingle" else "")
        if t == "GroupBox":
            out.append(f'<div style="{pos}"><fieldset style="position:absolute;inset:6px 0 0 0;border:1px solid {T["Bordura"]};margin:0;padding:0"></fieldset>'
                       f'<div style="position:absolute;left:8px;top:-1px;padding:0 3px;background:{back or T["Suprafata"]};{css_font(f)}color:{fg}">{esc(text)}</div></div>')
        else:
            out.append(f'<div style="{style}"></div>')
        for k in reversed(c.children):
            out.append(render(k, spec, ox, oy, bg or back))
        return "".join(out)
    if t == "TabControl":
        sel = p.get("_sel", 0)
        tabs, tx = [], x
        for i, pg in enumerate(c.children):
            tw = text_w(pg.p.get("Text", ""), f) + 18
            on = i == sel
            tabs.append(f'<div style="position:absolute;left:{tx - ox + 2}px;top:{y - oy + (0 if on else 2)}px;width:{tw}px;height:{(24 if on else 21)}px;box-sizing:border-box;'
                        f'border:1px solid {T["Bordura"]};border-bottom:{"none" if on else "1px solid " + T["Bordura"]};background:{"#FFFFFF" if on else "#F0F0F0"};'
                        f'{css_font(f)}color:{T["Text"]};display:flex;align-items:center;justify-content:center;z-index:2">{esc(pg.p.get("Text", ""))}</div>')
            tx += tw
        out.append(f'<div style="position:absolute;left:{x - ox}px;top:{y - oy + 23}px;width:{w}px;height:{h - 23}px;box-sizing:border-box;border:1px solid {T["Bordura"]};background:#FFFFFF"></div>')
        out.extend(tabs)
        for pg in c.children:
            out.append(render(pg, spec, ox, oy, back))
        return "".join(out)
    if t in ("Label", "LinkLabel"):
        if t == "LinkLabel":
            fg = p.get("LinkColor", T["Primar"])
        al = ALIGN.get(p.get("TextAlign", "TopLeft"), ALIGN["TopLeft"])
        pad = p.get("Padding", (0, 0, 0, 0))
        auto = p.get("AutoSize") and not p.get("MaximumSize")
        ws = "white-space:nowrap;" if auto else "white-space:pre-wrap;overflow:hidden;"
        icon_html = ""
        if s.get("icon"):
            icon_html = icon(s["icon"], 14, s.get("iconColor", fg)) + "&nbsp;"
        out.append(f'<div style="{pos}display:flex;align-items:{al[0]};justify-content:{al[1]};text-align:{al[2]};{css_font(f)}color:{s.get("color", fg)};'
                   f'{ws}line-height:1.3;padding:{pad[1]}px {pad[2]}px {pad[3]}px {pad[0]}px;box-sizing:border-box;'
                   f'{"text-decoration:underline;" if t == "LinkLabel" else ""}{("background:" + bg + ";") if bg else ""}"><span>{icon_html}{esc(text)}</span></div>')
        return "".join(out)
    if t in ("TextBox", "ComboBox", "DateTimePicker", "NumericUpDown", "ListBox"):
        val = s.get("value", "")
        if t == "ComboBox" and not val and c.items:
            val = c.items[0]
        if t == "TextBox" and p.get("UseSystemPasswordChar") and val:
            val = "●" * len(val)
        err = s.get("error")
        border = T["Eroare"] if err else "#8A8886"
        ro = p.get("ReadOnly") is True
        bgc = "#F3F3F3" if ro or p.get("Enabled") is False else "#FFFFFF"
        if t == "ComboBox" and p.get("DropDownStyle") == "DropDownList":
            bgc = "#FFFFFF"
        hh = h if t in ("ListBox",) or p.get("Multiline") else max(h, 23)
        style = (pos + f"height:{hh}px;box-sizing:border-box;border:1px solid {border};background:{bgc};{css_font(f)}color:{T['Text']};"
                 "display:flex;align-items:" + ("flex-start;padding:3px 4px" if t == "ListBox" or p.get("Multiline") else "center;padding:0 4px") + ";overflow:hidden;white-space:pre;")
        extra = ""
        if t == "ComboBox":
            extra = f'<div style="position:absolute;right:4px;top:50%;transform:translateY(-50%)">{icon_chevron()}</div>'
        if t == "DateTimePicker":
            extra = f'<div style="position:absolute;right:5px;top:50%;transform:translateY(-50%)">{icon("calendar", 13, "#444")}</div>'
        if t == "NumericUpDown":
            extra = f'<div style="position:absolute;right:0;top:0;bottom:0;width:16px;border-left:1px solid #CCC;font-size:7px;line-height:10px;text-align:center;color:#444">▲<br>▼</div>'
            if not val:
                val = str(int(p.get("Value", 0))) if isinstance(p.get("Value"), float) else str(p.get("Value", "0"))
        if t == "DateTimePicker" and not val:
            val = "13.10.2026"
        if t == "ListBox":
            items = s.get("items", c.items)
            sel = s.get("selected", -1)
            val = "".join(f'<div style="height:17px;{("background:#0078D7;color:white;" if i == sel else "")}">{esc(it)}</div>' for i, it in enumerate(items))
            out.append(f'<div style="{style}"><div style="width:100%">{val}</div></div>')
        else:
            align = "right" if p.get("TextAlign") == "Right" else "left"
            caret = '<span style="display:inline-block;width:1px;height:14px;background:#000;vertical-align:middle;margin-left:1px"></span>' if s.get("focus") else ""
            out.append(f'<div style="{style}"><span style="flex:1;text-align:{align};overflow:hidden">{esc(val)}{caret}</span>{extra}</div>')
        if err:
            out.append(f'<div style="position:absolute;left:{x - ox + w + 4}px;top:{y - oy + (hh - 14) / 2}px;width:14px;height:14px;border-radius:7px;background:{T["Eroare"]};'
                       f'color:white;font:bold 10px sans-serif;display:flex;align-items:center;justify-content:center">!</div>')
            if s.get("tip"):
                out.append(tooltip(x - ox + w + 10, y - oy + hh + 2, err))
        return "".join(out)
    if t in ("CheckBox", "RadioButton"):
        chk = s.get("checked", p.get("Checked", False))
        box = (f'<span style="display:inline-block;width:13px;height:13px;box-sizing:border-box;border:1px solid #333;'
               f'{"border-radius:7px;" if t == "RadioButton" else "border-radius:2px;"}'
               f'background:{"#FFFFFF" if not chk or t == "RadioButton" else "#0067C0"};position:relative;margin-right:5px;flex:none">'
               + ((f'<span style="position:absolute;left:2px;top:2px;width:7px;height:7px;border-radius:4px;background:#222"></span>' if t == "RadioButton" else
                   '<svg width="11" height="11" viewBox="0 0 12 12" style="position:absolute;left:0;top:0"><path d="M2.5 6.2l2.4 2.4 4.8-5" stroke="white" stroke-width="1.6" fill="none"/></svg>') if chk else "")
               + "</span>")
        out.append(f'<div style="{pos}display:flex;align-items:center;{css_font(f)}color:{fg};white-space:nowrap">{box}{esc(text)}</div>')
        return "".join(out)
    if t == "ButonIcon":
        stil = p.get("Stil", "Secundar")
        en = s.get("enabled", p.get("Enabled", True)) is not False
        activ = s.get("activ", p.get("Activ", False))
        fundal, txt, bord = {
            "Primar": (T["Primar"], "#FFFFFF", T["Primar"]), "Pericol": (T["Suprafata"], T["Eroare"], T["Eroare"]),
            "Transparent": (T["Suprafata"], T["Text"], T["Suprafata"]),
            "Meniu": (T["MeniuActiv"], "#FFFFFF", T["MeniuActiv"]) if activ else (T["Meniu"], T["MeniuText"], T["Meniu"]),
        }.get(stil, (T["Suprafata"], T["Text"], T["Bordura"]))
        if not en:
            txt = T["TextSecundar"]
            if stil == "Primar":
                fundal = bord = T["Bordura"]
        bw = 0 if stil in ("Transparent", "Meniu") else 1
        al = p.get("TextAlign", "MiddleCenter")
        pad = p.get("Padding", (0, 0, 0, 0))
        just = "flex-start" if "Left" in al else "center"
        ic = icon(p.get("Icon", ""), int(p.get("MarimeIcon", 16)), txt)
        extra = ""
        if stil == "Meniu" and activ:
            extra = f'<div style="position:absolute;left:0;top:0;width:4px;height:100%;background:{T["Semnal"]}"></div>'
        elif stil == "Primar" and en:
            extra = f'<div style="position:absolute;left:0;bottom:0;width:100%;height:3px;background:{T["PrimarInchis"]}"></div>'
        label = esc(text.strip()) if text.strip() else ""
        gap = 6 if label and ic else 0
        if s.get("hover"):
            fundal = T["Fundal"] if stil == "Secundar" else fundal
        out.append(f'<div style="{pos}box-sizing:border-box;border:{bw}px solid {bord};background:{fundal};color:{txt};{css_font(f)}'
                   f'display:flex;align-items:center;justify-content:{just};padding-left:{pad[0] + (4 if just == "flex-start" else 0)}px;gap:{gap}px;white-space:nowrap;overflow:hidden">'
                   f'{extra}{ic}<span>{label}</span></div>')
        if s.get("cursor"):
            out.append(cursor(x - ox + w * 0.6, y - oy + h * 0.55))
        return "".join(out)
    if t == "IconImagine":
        col = p.get("Culoare", T["Primar"]) if str(p.get("Culoare", "")).startswith("#") else T["Primar"]
        out.append(f'<div style="{pos}">{icon(s.get("icon", p.get("Icon", "")), int(min(w, h)), s.get("color", col))}</div>')
        return "".join(out)
    if t == "CardKpi":
        acc = p.get("Accent", T["Primar"]) if str(p.get("Accent", "")).startswith("#") else T["Primar"]
        val = s.get("value", p.get("Valoare", "–"))
        out.append(f'<div style="{pos}box-sizing:border-box;border:1px solid {T["Bordura"]};background:{T["Suprafata"]}">'
                   f'<div style="position:absolute;left:0;top:0;width:6px;height:100%;background:{acc}"></div>'
                   f'<div style="position:absolute;left:20px;top:14px;right:36px;{css_font(dict(font="Bahnschrift SemiBold", size=8))}color:{T["TextSecundar"]};white-space:nowrap;overflow:hidden">{esc(str(p.get("Titlu", "")).upper())}</div>'
                   f'<div style="position:absolute;right:14px;top:14px">{icon(p.get("Icon", ""), 16, T["TextSecundar"])}</div>'
                   f'<div style="position:absolute;left:20px;bottom:12px;{css_font(dict(font="Bahnschrift SemiBold", size=20))}color:{T["Text"]};white-space:nowrap">{esc(val)}</div></div>')
        return "".join(out)
    if t == "IndicatorConexiune":
        stare = s.get("stare", "Online")
        col = {"Online": T["Succes"], "Offline": T["Eroare"], "Partial": T["Avertisment"]}.get(stare.split()[0], T["Succes"])
        lbl = s.get("text", stare)
        out.append(f'<div style="{pos}display:flex;align-items:center;gap:6px;{css_font(f)}color:{fg}"><span style="width:8px;height:8px;background:{col};display:inline-block"></span>{esc(lbl)}</div>')
        return "".join(out)
    if t == "GridAutogara":
        out.append(grid(c, s, pos, w, h))
        return "".join(out)
    out.append(f'<div style="{pos}border:1px dashed #999;font:10px sans-serif;color:#999">{t}</div>')
    return "".join(out)


def icon_chevron():
    return '<svg width="10" height="10" viewBox="0 0 10 10"><path d="M1.5 3.5l3.5 3.5 3.5-3.5" stroke="#444" stroke-width="1.3" fill="none"/></svg>'


def cursor(x, y):
    return (f'<svg style="position:absolute;left:{x}px;top:{y}px;z-index:50" width="16" height="22" viewBox="0 0 16 22">'
            '<path d="M1 1 L1 17 L5 13 L8 20 L11 19 L8 12 L14 12 Z" fill="white" stroke="black" stroke-width="1.2"/></svg>')


def tooltip(x, y, text):
    return (f'<div style="position:absolute;left:{x}px;top:{y}px;z-index:40;background:#FFFFFF;border:1px solid #767676;padding:3px 7px;'
            f'font-family:\'Segoe UI\',\'Noto Sans\',sans-serif;font-size:12px;color:#222;white-space:nowrap;box-shadow:1px 2px 4px rgba(0,0,0,.25)">{esc(text)}</div>')


def grid(c, s, pos, w, h):
    cols = [k for k in c.columns if k.p.get("Visible", True) is not False and s.get("hide_" + k.name) is not True]
    fixed = sum(k.p.get("Width", 100) for k in cols if k.p.get("AutoSizeMode") != "Fill")
    fills = [k for k in cols if k.p.get("AutoSizeMode") == "Fill"]
    avail = max(w - fixed - (17 if s.get("scroll") else 0), 0)
    tw = sum(k.p.get("FillWeight", 100) for k in fills) or 1
    widths = [(avail * k.p.get("FillWeight", 100) / tw if k.p.get("AutoSizeMode") == "Fill" else k.p.get("Width", 100)) for k in cols]
    rows = s.get("rows", [])
    sel = s.get("sel", 0 if rows else -1)
    hf = css_font(dict(font="Bahnschrift SemiBold", size=9.5))
    cf = css_font(dict(font="Segoe UI", size=9))
    ht = s.get("header_h", 36)
    rh = s.get("row_h", 32)
    o = [f'<div style="{pos}background:{T["Suprafata"]};overflow:hidden">']
    o.append(f'<div style="display:flex;height:{ht}px;background:{T["Meniu"]};color:{T["MeniuText"]};{hf}">')
    for k, cw in zip(cols, widths):
        right = k.p.get("DefaultCellStyle.Alignment", "") in ("MiddleRight",) or k.name in s.get("right", [])
        o.append(f'<div style="width:{cw}px;flex:none;padding:0 6px;box-sizing:border-box;display:flex;align-items:center;'
                 f'justify-content:{"flex-end" if right else "flex-start"};white-space:nowrap;overflow:hidden">{esc(k.p.get("HeaderText", ""))}</div>')
    o.append("</div>")
    for i, r in enumerate(rows):
        bgc = T["SemnalDeschis"] if i == sel else (T["Fundal"] if i % 2 else T["Suprafata"])
        o.append(f'<div style="display:flex;height:{rh - 1}px;background:{bgc};border-bottom:1px solid {T["Bordura"]};color:{T["Text"]};{cf}">')
        for j, (k, cw) in enumerate(zip(cols, widths)):
            v = r[j] if j < len(r) else ""
            right = k.name in s.get("right", [])
            o.append(f'<div style="width:{cw}px;flex:none;padding:0 6px;box-sizing:border-box;display:flex;align-items:center;'
                     f'justify-content:{"flex-end" if right else "flex-start"};white-space:nowrap;overflow:hidden;text-overflow:ellipsis">{esc(v)}</div>')
        o.append("</div>")
    if s.get("scroll") and rows:
        o.append(f'<div style="position:absolute;right:0;top:{ht}px;width:17px;bottom:0;background:#F0F0F0">'
                 f'<div style="position:absolute;left:4px;right:4px;top:4px;height:40%;background:#C1C1C1;border-radius:4px"></div></div>')
    if s.get("empty_text") and not rows:
        o.append(f'<div style="padding:16px;{cf}color:{T["TextSecundar"]}">{esc(s["empty_text"])}</div>')
    o.append("</div>")
    return "".join(o)
