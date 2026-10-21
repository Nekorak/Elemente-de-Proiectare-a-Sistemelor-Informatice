"""Pasul 2: cuprins (camp TOC cu pagini), citari CITATION, camp BIBLIOGRAPHY si sursele Word (customXml).
Utilizare: python3 postproces.py intrare.docx iesire.docx [pagini.json]"""
import json, os, re, shutil, sys, zipfile
from xml.sax.saxutils import escape

src, dst = sys.argv[1], sys.argv[2]
pagini = json.load(open(sys.argv[3])) if len(sys.argv) > 3 and os.path.exists(sys.argv[3]) else {}
here = os.path.dirname(os.path.abspath(__file__))
meta = json.load(open(os.path.join(here, "meta.json")))
surse = json.loads(os.popen(f"node -e \"console.log(JSON.stringify(require('{here}/surse.js')))\"").read())

z = zipfile.ZipFile(src)
files = {n: z.read(n) for n in z.namelist()}
doc = files["word/document.xml"].decode("utf8")
rels = files["word/_rels/document.xml.rels"].decode("utf8")
ct = files["[Content_Types].xml"].decode("utf8")

# ---------- citari ----------
ordine = []
for k in meta["cit"]:
    if k not in ordine:
        ordine.append(k)
for k in surse:
    if k not in ordine:
        ordine.append(k)
nr = {k: i + 1 for i, k in enumerate(ordine)}

def fld_runs(instr, rezultat_xml, rpr=""):
    return (f'<w:r>{rpr}<w:fldChar w:fldCharType="begin"/></w:r>'
            f'<w:r>{rpr}<w:instrText xml:space="preserve"> {escape(instr)} </w:instrText></w:r>'
            f'<w:r>{rpr}<w:fldChar w:fldCharType="separate"/></w:r>{rezultat_xml}'
            f'<w:r>{rpr}<w:fldChar w:fldCharType="end"/></w:r>')

def cit(m):
    rpr, key = m.group(1) or "", m.group(2)
    return fld_runs(f"CITATION {key} \\l 1048", f'<w:r>{rpr}<w:t>[{nr[key]}]</w:t></w:r>', rpr)

doc = re.sub(r'<w:r>(<w:rPr>(?:(?!</w:rPr>).)*</w:rPr>)?<w:t(?: xml:space="preserve")?>⟦CIT:(\w+)⟧</w:t></w:r>', cit, doc)
assert "⟦CIT" not in doc, "citare neinlocuita"

# ---------- bibliografie ----------
rid = 9000
def hyperlink(url):
    global rels, rid
    rid += 1
    rels = rels.replace("</Relationships>",
        f'<Relationship Id="rIdB{rid}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink" Target="{escape(url)}" TargetMode="External"/></Relationships>')
    return (f'<w:hyperlink r:id="rIdB{rid}" w:history="1"><w:r><w:rPr><w:color w:val="0563C1"/><w:u w:val="single"/></w:rPr>'
            f'<w:t>{escape(url)}</w:t></w:r></w:hyperlink>')

paras = []
for i, k in enumerate(ordine):
    s = surse[k]
    text = f'[{i + 1}]\t{s["author"]}, „{s["title"]}”, {s["site"]}, {s["year"]}. [Online]. Disponibil: '
    body = f'<w:r><w:t xml:space="preserve">{escape(text)}</w:t></w:r>{hyperlink(s["url"])}<w:r><w:t xml:space="preserve">. [Accesat: 21.10.2026].</w:t></w:r>'
    paras.append(body)
ppr = '<w:pPr><w:pStyle w:val="Bibliografie"/><w:tabs><w:tab w:val="left" w:pos="567"/></w:tabs></w:pPr>'
bib = []
for i, body in enumerate(paras):
    start = ('<w:r><w:fldChar w:fldCharType="begin"/></w:r><w:r><w:instrText xml:space="preserve"> BIBLIOGRAPHY  \\l 1048 </w:instrText></w:r>'
             '<w:r><w:fldChar w:fldCharType="separate"/></w:r>') if i == 0 else ""
    end = '<w:r><w:fldChar w:fldCharType="end"/></w:r>' if i == len(paras) - 1 else ""
    bib.append(f"<w:p>{ppr}{start}{body}{end}</w:p>")
bib_xml = ('<w:sdt><w:sdtPr><w:id w:val="-1"/><w:docPartObj><w:docPartGallery w:val="Bibliographies"/><w:docPartUnique/></w:docPartObj></w:sdtPr><w:sdtContent>'
             + "".join(bib) + "</w:sdtContent></w:sdt>")
doc = re.sub(r'<w:p>(?:(?!</w:p>).)*⟦BIBLIOGRAFIE⟧(?:(?!</w:p>).)*</w:p>', lambda m: bib_xml, doc, flags=re.S)

# ---------- semne de carte pe titluri ----------
cnt = [0]
def bm(m):
    cnt[0] += 1
    n = cnt[0]
    return f'{m.group(1)}<w:bookmarkStart w:id="{5000 + n}" w:name="_Toc{n:04d}"/>{m.group(2)}<w:bookmarkEnd w:id="{5000 + n}"/></w:p>'
doc = re.sub(r'(<w:p><w:pPr><w:pStyle w:val="Heading[123]"/>(?:(?!</w:pPr>).)*</w:pPr>)((?:(?!</w:p>).)*)</w:p>', bm, doc)
assert cnt[0] == len(meta["headings"]), (cnt[0], len(meta["headings"]))

# ---------- cuprins ----------
toc = []
H = meta["headings"]
for i, h in enumerate(H):
    n = i + 1
    pg = str(pagini.get(str(i), "0"))
    start = ('<w:r><w:fldChar w:fldCharType="begin"/></w:r><w:r><w:instrText xml:space="preserve"> TOC \\o "1-3" \\h \\z \\u </w:instrText></w:r>'
             '<w:r><w:fldChar w:fldCharType="separate"/></w:r>') if i == 0 else ""
    end = '<w:r><w:fldChar w:fldCharType="end"/></w:r>' if i == len(H) - 1 else ""
    toc.append(f'<w:p><w:pPr><w:pStyle w:val="TOC{h["level"]}"/></w:pPr>{start}'
               f'<w:hyperlink w:anchor="_Toc{n:04d}" w:history="1"><w:r><w:t xml:space="preserve">{escape(h["text"])}</w:t></w:r>'
               f'<w:r><w:tab/></w:r><w:r><w:t>{pg}</w:t></w:r></w:hyperlink>{end}</w:p>')
doc = re.sub(r'<w:p>(?:(?!</w:p>).)*⟦CUPRINS⟧(?:(?!</w:p>).)*</w:p>', lambda m: "".join(toc), doc, flags=re.S)

# ---------- surse Word (customXml) ----------
def sursa(k):
    s = surse[k]
    return (f'<b:Source><b:Tag>{k}</b:Tag><b:SourceType>InternetSite</b:SourceType><b:Guid>{{{"%08X" % (abs(hash(k)) % 0xFFFFFFFF)}-0000-0000-0000-000000000000}}</b:Guid>'
            f'<b:Author><b:Author><b:Corporate>{escape(s["author"])}</b:Corporate></b:Author></b:Author>'
            f'<b:Title>{escape(s["title"])}</b:Title><b:InternetSiteTitle>{escape(s["site"])}</b:InternetSiteTitle>'
            f'<b:Year>{s["year"]}</b:Year><b:YearAccessed>2026</b:YearAccessed><b:MonthAccessed>10</b:MonthAccessed><b:DayAccessed>21</b:DayAccessed>'
            f'<b:URL>{escape(s["url"])}</b:URL><b:RefOrder>{nr[k]}</b:RefOrder></b:Source>')
files["customXml/item1.xml"] = ('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
    '<b:Sources SelectedStyle="\\IEEE2006OfficeOnline.xsl" StyleName="IEEE" Version="2006" '
    'xmlns:b="http://schemas.openxmlformats.org/officeDocument/2006/bibliography" '
    'xmlns="http://schemas.openxmlformats.org/officeDocument/2006/bibliography">'
    + "".join(sursa(k) for k in ordine) + "</b:Sources>").encode("utf8")
files["customXml/itemProps1.xml"] = ('<?xml version="1.0" encoding="UTF-8" standalone="no"?>'
    '<ds:datastoreItem ds:itemID="{6A1B2C3D-4E5F-4A6B-8C7D-9E0F1A2B3C4D}" xmlns:ds="http://schemas.openxmlformats.org/officeDocument/2006/customXml">'
    '<ds:schemaRefs><ds:schemaRef ds:uri="http://schemas.openxmlformats.org/officeDocument/2006/bibliography"/></ds:schemaRefs></ds:datastoreItem>').encode("utf8")
files["customXml/_rels/item1.xml.rels"] = ('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
    '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'
    '<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXmlProps" Target="itemProps1.xml"/></Relationships>').encode("utf8")
rels = rels.replace("</Relationships>", '<Relationship Id="rIdCx1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXml" Target="../customXml/item1.xml"/></Relationships>')
if "itemProps1.xml" not in ct:
    ct = ct.replace("</Types>", '<Override PartName="/customXml/itemProps1.xml" ContentType="application/vnd.openxmlformats-officedocument.customXmlProperties+xml"/></Types>')

files["word/document.xml"] = doc.encode("utf8")
files["word/_rels/document.xml.rels"] = rels.encode("utf8")
files["[Content_Types].xml"] = ct.encode("utf8")
with zipfile.ZipFile(dst, "w", zipfile.ZIP_DEFLATED) as out:
    for n in ["[Content_Types].xml"] + [n for n in files if n != "[Content_Types].xml"]:
        out.writestr(n, files[n])
print("citari:", len(meta["cit"]), "surse:", len(ordine), "titluri:", cnt[0])
