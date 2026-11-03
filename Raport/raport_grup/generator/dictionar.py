# Extrage din 03_Tables.sql structura tabelelor pentru dictionarul de date (JSON).
import re, sys, json
src = open(sys.argv[1], encoding="utf-8-sig").read()
out = []
for m in re.finditer(r"CREATE TABLE autogara\.(\w+)\s*\((.*?)\n\);", src, re.S):
    nume, corp = m.group(1), m.group(2)
    cols, cons = [], []
    for linie in corp.splitlines():
        l = linie.strip().rstrip(",")
        if not l or l.startswith("--"):
            continue
        if l.startswith("CONSTRAINT"):
            cons.append(l)
            continue
        c = re.match(r"(\w+)\s+([A-Z][A-Z0-9]*(?:\([^)]*\))?(?:\s+IDENTITY\(\d+,\s*\d+\))?)\s+(NOT NULL|NULL)?(.*)", l)
        if c:
            dflt = re.search(r"DEFAULT\s+(.+)$", c.group(4))
            cols.append({"col": c.group(1), "tip": re.sub(r"\s+", " ", c.group(2)), "null": c.group(3) != "NOT NULL",
                         "default": dflt.group(1).strip() if dflt else None})
    pk, fk, uq, ck = set(), {}, [], []
    for k in cons:
        if " PRIMARY KEY" in k:
            pk |= {x.strip() for x in re.search(r"PRIMARY KEY\s*(?:CLUSTERED\s*)?\(([^)]*)\)", k).group(1).split(",")}
        elif "FOREIGN KEY" in k:
            f = re.search(r"FOREIGN KEY\s*\(([^)]*)\)\s*REFERENCES autogara\.(\w+)", k)
            for x in f.group(1).split(","):
                fk.setdefault(x.strip(), [])
                if f.group(2) not in fk[x.strip()]: fk[x.strip()].append(f.group(2))
        elif " UNIQUE" in k:
            uq.append([x.strip() for x in re.search(r"UNIQUE\s*\(([^)]*)\)", k).group(1).split(",")])
        elif " CHECK" in k:
            ck.append(re.search(r"CHECK\s*\((.*)\)\s*$", k).group(1))
    for c in cols:
        chei = []
        if c["col"] in pk: chei.append("PK")
        if c["col"] in fk: chei.append("FK → " + ", ".join(fk[c["col"]]))
        if any(u == [c["col"]] for u in uq): chei.append("UQ")
        c["chei"] = ", ".join(chei)
        c["check"] = [x for x in ck if re.search(r"\b" + c["col"] + r"\b", x)]
    out.append({"tabel": nume, "coloane": cols, "uq_compuse": [u for u in uq if len(u) > 1]})
json.dump(out, open(sys.argv[2], "w"), ensure_ascii=False, indent=1)
print(len(out), "tabele,", sum(len(t["coloane"]) for t in out), "coloane")
