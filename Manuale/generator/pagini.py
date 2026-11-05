"""Gaseste pagina fiecarui titlu in PDF-ul randat. Utilizare: python3 pagini.py raport.pdf pagini.json"""
import json, re, subprocess, sys
pdf, out = sys.argv[1], sys.argv[2]
meta = json.load(open("meta.json"))
n = int(re.search(r"Pages:\s+(\d+)", subprocess.run(["pdfinfo", pdf], capture_output=True, text=True).stdout).group(1))
norm = lambda s: re.sub(r"\s+", " ", s).strip().lower()
texte = [norm(subprocess.run(["pdftotext", "-f", str(p), "-l", str(p), "-layout", pdf, "-"], capture_output=True, text=True).stdout) for p in range(1, n + 1)]
# sari peste foaia de titlu si cuprins: incepe dupa pagina cu "cuprins"
start = next(i for i, t in enumerate(texte) if "cuprins" in t) + 1
while start < n and "introducere" not in texte[start] or (start < n and texte[start].count("....") > 3):
    start += 1
rez, p = {}, start
for i, h in enumerate(meta["headings"]):
    cheie = norm(h["text"])[:40]
    for q in range(p, n):
        if cheie in texte[q]:
            p = q; rez[str(i)] = q + 1; break
    else:
        rez[str(i)] = "?"; print("negasit:", h["text"])
json.dump(rez, open(out, "w"))
print("pagini:", n, "primul titlu la", rez.get("0"))
