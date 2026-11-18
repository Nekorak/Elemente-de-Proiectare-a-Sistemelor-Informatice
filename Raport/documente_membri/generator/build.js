// Construiește documentul individual al unui membru: node build.js <dmitri|sergiu|alexandr|maxim> iesire.docx
const path = require("path");
process.env.IMG_DIR = process.env.IMG_DIR || path.join(__dirname, "..", "..", "raport_grup", "capturi");
const fs = require("fs");
const { Document, Packer, Paragraph, TextRun, AlignmentType, Footer, PageNumber, LevelFormat, LineRuleType } = require("docx");
const { FONT, MONO } = require("../../raport_grup/generator/lib");
const { ECHIPA, pagina1, pagini23 } = require("./comun");
const membri = require("./membri");

const cheie = process.argv[2];
const m = ECHIPA.find((e) => e[0] === cheie);
if (!m || !membri[cheie]) { console.error("membru necunoscut:", cheie); process.exit(1); }

const MM = (x) => Math.round(x * 56.6929);
const doc = new Document({
  creator: m[1],
  title: `Contribuția individuală — ${m[1]} (${m[2]})`,
  description: "Proiectul de grup EPSI, grupa BD-423, Colegiul Tehnic Feroviar din Bălți",
  styles: {
    default: { document: { run: { font: FONT, size: 24, language: { value: "ro-RO" } },
      paragraph: { spacing: { line: 276, lineRule: LineRuleType.AUTO, after: 60 } } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: 28, bold: true, color: "000000" },
        paragraph: { alignment: AlignmentType.CENTER, spacing: { before: 0, after: 160, line: 276 }, outlineLevel: 0, keepNext: true } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: 26, bold: true, color: "000000" },
        paragraph: { spacing: { before: 160, after: 60, line: 276 }, indent: { firstLine: 567 }, outlineLevel: 1, keepNext: true } },
      { id: "Cod", name: "Cod sursă", basedOn: "Normal", run: { font: MONO, size: 20 },
        paragraph: { alignment: AlignmentType.LEFT, spacing: { line: 240, before: 0, after: 0 } } },
      { id: "Legenda", name: "Legendă figură", basedOn: "Normal", next: "Normal", run: { font: FONT, size: 22 },
        paragraph: { alignment: AlignmentType.CENTER, spacing: { before: 40, after: 120, line: 240 } } },
      { id: "LegendaTabel", name: "Legendă tabel", basedOn: "Normal", next: "Normal", run: { font: FONT, size: 22, italics: true },
        paragraph: { alignment: AlignmentType.RIGHT, spacing: { before: 80, after: 40, line: 240 } } },
    ],
  },
  numbering: { config: [
    { reference: "puncte", levels: [{ level: 0, format: LevelFormat.BULLET, text: "–", alignment: AlignmentType.LEFT,
      style: { paragraph: { indent: { left: 924, hanging: 357 } } } }] },
    { reference: "numere", levels: [{ level: 0, format: LevelFormat.DECIMAL, text: "%1.", alignment: AlignmentType.LEFT,
      style: { paragraph: { indent: { left: 924, hanging: 357 } } } }] },
  ] },
  sections: [{
    properties: { page: { size: { width: 11906, height: 16838 },
      margin: { top: MM(20), bottom: MM(20), left: MM(30), right: MM(10), footer: MM(8) } } },
    footers: { default: new Footer({ children: [new Paragraph({ alignment: AlignmentType.CENTER, spacing: { line: 240 },
      children: [new TextRun({ children: [PageNumber.CURRENT], size: 22 })] })] }) },
    children: [...pagina1(cheie), ...pagini23(), ...membri[cheie]()],
  }],
});

Packer.toBuffer(doc).then((buf) => fs.writeFileSync(process.argv[3], buf));
