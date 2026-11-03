# Stil comun pentru diagramele de flux (notatia Gane–Sarson), randate cu Graphviz.
G, L, Y, INK = "#1e5b46", "#dde8e2", "#e8a317", "#1b1b1b"
ANTET = f'''graph [fontname="DejaVu Sans", fontsize=20, nodesep=0.3, ranksep=0.45, pad=0.2, splines=true, bgcolor="white"];
node [fontname="DejaVu Sans", fontsize=20, color="{G}", penwidth=1.4];
edge [fontname="DejaVu Sans", fontsize=17, color="#555555", fontcolor="#333333", arrowsize=0.8, penwidth=1.1];
'''
def entitate(id, text):
    return f'{id} [shape=box, style="filled", fillcolor="{L}", penwidth=2.2, label=<<B>{text}</B>>, margin="0.18,0.1"];'
def proces(id, nr, text):
    return (f'{id} [shape=plain, label=<<TABLE BORDER="1" CELLBORDER="0" CELLSPACING="0" CELLPADDING="5" STYLE="ROUNDED" COLOR="{G}">'
            f'<TR><TD BGCOLOR="{G}"><FONT COLOR="white"><B>{nr}</B></FONT></TD></TR>'
            f'<TR><TD>{text}</TD></TR></TABLE>>];')
def depozit(id, nr, text):
    return (f'{id} [shape=plain, label=<<TABLE BORDER="0" CELLBORDER="1" CELLSPACING="0" CELLPADDING="5" COLOR="{G}">'
            f'<TR><TD BGCOLOR="{L}"><B>{nr}</B></TD><TD SIDES="TBR" ALIGN="LEFT">{text}</TD></TR></TABLE>>];')
def flux(a, b, text="", extra=""):
    return f'{a} -> {b} [label="{text}"{", " + extra if extra else ""}];'
