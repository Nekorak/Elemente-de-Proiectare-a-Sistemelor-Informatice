// Construieste raport_proiect_grup.docx (pasul 1; pasul 2 = postproces.py: cuprins, citari, bibliografie)
const fs = require("fs");
const {
  Document, Packer, Paragraph, TextRun, AlignmentType, Footer, PageNumber, LevelFormat,
  Table, TableRow, TableCell, WidthType, BorderStyle, TabStopType, LineRuleType,
} = require("docx");
const { state, FONT, MONO, H } = require("./lib");

const MM = (x) => Math.round(x * 56.6929);
const C = (text, o = {}) => new Paragraph({ alignment: AlignmentType.CENTER, spacing: { line: 240, before: o.before || 0, after: o.after || 0 },
  children: [new TextRun({ text, bold: o.bold, size: o.size || 28, allCaps: o.caps })] });

function foaiaDeTitlu() {
  const none = { style: BorderStyle.NONE, size: 0, color: "FFFFFF" };
  const nb = { top: none, bottom: none, left: none, right: none };
  const line = (t, bold) => new Paragraph({ spacing: { line: 360 }, children: [new TextRun({ text: t, bold, size: 28 })] });
  const lbl = (a, b) => new Paragraph({ spacing: { line: 360 }, children: [new TextRun({ text: a, size: 28 }), new TextRun({ text: b, bold: true, size: 28 })] });
  const colW = [4800, 4838];
  const bloc = new Table({
    width: { size: 9638, type: WidthType.DXA }, columnWidths: colW, borders: { top: none, bottom: none, left: none, right: none, insideHorizontal: none, insideVertical: none },
    rows: [new TableRow({ children: [
      new TableCell({ width: { size: colW[0], type: WidthType.DXA }, borders: nb, children: [
        line("Efectuat:"),
        line("Gojinevschii Dmitri", true), line("Hanganu Sergiu", true), line("Crivenco Alexandr", true), line("Sopivnic Maxim", true),
        new Paragraph({ spacing: { line: 360, before: 120 }, children: [] }),
        lbl("Grupa: ", "BD-423"),
      ] }),
      new TableCell({ width: { size: colW[1], type: WidthType.DXA }, borders: nb, children: [
        new Paragraph({ alignment: AlignmentType.RIGHT, spacing: { line: 360 }, children: [new TextRun({ text: "Verificat: ", size: 28 }), new TextRun({ text: "Vieru Viorica", bold: true, size: 28 })] }),
        new Paragraph({ spacing: { line: 360 }, children: [] }),
        new Paragraph({ spacing: { line: 360 }, children: [] }),
        new Paragraph({ alignment: AlignmentType.RIGHT, spacing: { line: 360 }, children: [new TextRun({ text: "Nota ________________", size: 28 })] }),
      ] }),
    ] })],
  });
  return [
    C("Ministerul Educaţiei și Cercetării al Republicii Moldova", { bold: false }),
    C("IP Colegiul Tehnic Feroviar din Bălţi", { before: 120 }),
    C("RAPORT", { bold: true, size: 40, before: 2600 }),
    C("la proiectul de grup", { before: 120 }),
    C("la disciplina „Elemente de proiectare a sistemelor informatice”", { before: 60, size: 24 }),
    new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 700, line: 360 }, children: [
      new TextRun({ text: "TEMA: ", bold: true, size: 28 }), new TextRun({ text: "Sistem informatic pentru gestionarea unei autogări", size: 28 })] }),
    new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 500, line: 360 }, children: [
      new TextRun({ text: "Specialitatea: ", size: 28 }), new TextRun({ text: "61220 Administrarea bazelor de date", bold: true, size: 28 })] }),
    new Paragraph({ alignment: AlignmentType.CENTER, spacing: { line: 360 }, children: [
      new TextRun({ text: "Calificarea: ", size: 28 }), new TextRun({ text: "Administrator al bazelor de date", bold: true, size: 28 })] }),
    new Paragraph({ spacing: { before: 1300 }, children: [] }),
    bloc,
    C("Bălţi, 2026", { before: 2000 }),
  ];
}

function cuprins() {
  return [
    new Paragraph({ pageBreakBefore: true, alignment: AlignmentType.CENTER, spacing: { after: 240 }, children: [new TextRun({ text: "CUPRINS", bold: true, size: 28 })] }),
    new Paragraph({ children: [new TextRun("⟦CUPRINS⟧")] }),
  ];
}

const continut = [
  ...require("./c0_intro")(),
  ...require("./c1_bd")(),
  ...require("./c2_backend")(),
  ...require("./c3_frontend")(),
  ...require("./c4_testare")(),
  ...require("./c5_final")(),
  H(1, "BIBLIOGRAFIE"),
  new Paragraph({ children: [new TextRun("⟦BIBLIOGRAFIE⟧")] }),
];

const doc = new Document({
  creator: "Gojinevschii Dmitri, Hanganu Sergiu, Crivenco Alexandr, Sopivnic Maxim",
  title: "Raport la proiectul de grup — Sistem informatic pentru gestionarea unei autogări",
  description: "Grupa BD-423, Colegiul Tehnic Feroviar din Bălți",
  styles: {
    default: {
      document: { run: { font: FONT, size: 24, language: { value: "ro-RO" } },
        paragraph: { spacing: { line: 360, lineRule: LineRuleType.AUTO, after: 60 } } },
    },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: 28, bold: true, color: "000000" },
        paragraph: { alignment: AlignmentType.CENTER, spacing: { before: 0, after: 240, line: 360 }, outlineLevel: 0, keepNext: true } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: 28, bold: true, color: "000000" },
        paragraph: { spacing: { before: 280, after: 120, line: 360 }, indent: { firstLine: 567 }, outlineLevel: 1, keepNext: true } },
      { id: "Heading3", name: "Heading 3", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: 24, bold: true, color: "000000" },
        paragraph: { spacing: { before: 200, after: 80, line: 360 }, indent: { firstLine: 567 }, outlineLevel: 2, keepNext: true } },
      { id: "Cod", name: "Cod sursă", basedOn: "Normal", run: { font: MONO, size: 20 },
        paragraph: { alignment: AlignmentType.LEFT, spacing: { line: 240, before: 0, after: 0 } } },
      { id: "Legenda", name: "Legendă figură", basedOn: "Normal", next: "Normal", run: { font: FONT, size: 24 },
        paragraph: { alignment: AlignmentType.CENTER, spacing: { before: 60, after: 200, line: 240 } } },
      { id: "LegendaTabel", name: "Legendă tabel", basedOn: "Normal", next: "Normal", run: { font: FONT, size: 24, italics: true },
        paragraph: { alignment: AlignmentType.RIGHT, spacing: { before: 120, after: 60, line: 240 } } },
      ...[1, 2, 3].map((n) => ({ id: `TOC${n}`, name: `toc ${n}`, basedOn: "Normal", next: "Normal",
        run: { font: FONT, size: 24, bold: n === 1 },
        paragraph: { spacing: { before: n === 1 ? 80 : 0, after: 0, line: 300 }, indent: { left: (n - 1) * 360 },
          tabStops: [{ type: TabStopType.RIGHT, position: 9638, leader: "dot" }] } })),
      { id: "Bibliografie", name: "Bibliography", basedOn: "Normal", run: { font: FONT, size: 24 },
        paragraph: { alignment: AlignmentType.LEFT, spacing: { after: 80, line: 300 }, indent: { left: 567, hanging: 567 } } },
    ],
  },
  numbering: { config: [
    { reference: "puncte", levels: [{ level: 0, format: LevelFormat.BULLET, text: "–", alignment: AlignmentType.LEFT,
      style: { paragraph: { indent: { left: 924, hanging: 357 } } } }] },
    { reference: "numere", levels: [{ level: 0, format: LevelFormat.DECIMAL, text: "%1.", alignment: AlignmentType.LEFT,
      style: { paragraph: { indent: { left: 924, hanging: 357 } } } }] },
  ] },
  sections: [{
    properties: {
      titlePage: true,
      page: { size: { width: 11906, height: 16838 }, margin: { top: MM(20), bottom: MM(20), left: MM(30), right: MM(10), footer: MM(8) } },
    },
    footers: {
      first: new Footer({ children: [new Paragraph({ children: [] })] }),
      default: new Footer({ children: [new Paragraph({ alignment: AlignmentType.CENTER, spacing: { line: 240 },
        children: [new TextRun({ children: [PageNumber.CURRENT], size: 24 })] })] }),
    },
    children: [...foaiaDeTitlu(), ...cuprins(), ...continut],
  }],
});

Packer.toBuffer(doc).then((buf) => {
  fs.writeFileSync(process.argv[2] || "raport_pas1.docx", buf);
  fs.writeFileSync("meta.json", JSON.stringify({ headings: state.headings, cit: state.cit, missing: state.missing, used: state.used }, null, 1));
  console.log("titluri:", state.headings.length, "figuri lipsă:", state.missing.length, "folosite:", state.used.length);
});
