// Helpers pentru raport: stiluri dupa modelul colegiului si blocuri de continut.
const fs = require("fs");
const path = require("path");
const {
  Paragraph, TextRun, ImageRun, Table, TableRow, TableCell, AlignmentType, WidthType,
  BorderStyle, ShadingType, HeadingLevel, PageBreak, TabStopType, LevelFormat, VerticalAlign,
} = require("docx");
const { imageSize } = require("./imgsize");

const IMG_DIR = process.env.IMG_DIR || path.join(__dirname, "..", "img");
const FONT = "Times New Roman";
const MONO = "Courier New";
const CONTENT_W = 9638; // 170 mm in twips
const MAX_W_PX = 640;
const MAX_H_PX = 820;

const state = { chapter: 0, fig: 0, tab: 0, missing: [], used: [], headings: [], cit: [] };

// **bold**, *italic*, `code`
function runs(text, base = {}) {
  const out = [];
  const re = /(\[\[\w+\]\]|\*\*[^*]+\*\*|`[^`]+`|\*[^*]+\*)/g;
  let last = 0, m;
  while ((m = re.exec(text))) {
    if (m.index > last) out.push(new TextRun({ text: text.slice(last, m.index), ...base }));
    const t = m[0];
    if (t.startsWith("[[")) { state.cit.push(t.slice(2, -2)); out.push(new TextRun({ text: "⟦CIT:" + t.slice(2, -2) + "⟧", ...base })); }
    else if (t.startsWith("**")) out.push(new TextRun({ text: t.slice(2, -2), bold: true, ...base }));
    else if (t.startsWith("`")) out.push(new TextRun({ text: t.slice(1, -1), font: MONO, size: 20, ...base }));
    else out.push(new TextRun({ text: t.slice(1, -1), italics: true, ...base }));
    last = m.index + t.length;
  }
  if (last < text.length) out.push(new TextRun({ text: text.slice(last), ...base }));
  return out;
}

const P = (text, opts = {}) =>
  new Paragraph({ children: runs(text), alignment: AlignmentType.JUSTIFIED, indent: { firstLine: 567 }, ...opts });

function H(level, text) {
  if (level === 1) { state.chapter += 0; }
  state.headings.push({ level, text });
  const heading = [HeadingLevel.HEADING_1, HeadingLevel.HEADING_2, HeadingLevel.HEADING_3][level - 1];
  return new Paragraph({ heading, children: [new TextRun(text)], pageBreakBefore: level === 1 });
}

function startChapter(n) { state.chapter = n; state.fig = 0; state.tab = 0; }

const bullets = (items) =>
  items.map((t) => new Paragraph({ children: runs(t), numbering: { reference: "puncte", level: 0 }, alignment: AlignmentType.JUSTIFIED }));
const numbered = (items, ref = "numere") =>
  items.map((t) => new Paragraph({ children: runs(t), numbering: { reference: ref, level: 0 }, alignment: AlignmentType.JUSTIFIED }));

function code(text) {
  return text.replace(/\n$/, "").split("\n").map((line, i, arr) => new Paragraph({
    style: "Cod",
    children: [new TextRun({ text: line.length ? line : " " })],
    shading: { type: ShadingType.CLEAR, color: "auto", fill: "F3F5F4" },
    spacing: { before: i === 0 ? 120 : 0, after: i === arr.length - 1 ? 160 : 0 },
  }));
}

function figNo() { state.fig += 1; return state.chapter ? `${state.chapter}.${state.fig}` : `${state.fig}`; }

// fig(cod, legenda): cod = numele fisierului fara extensie din IMG_DIR
function fig(cod, caption, opts = {}) {
  const file = [".png", ".jpg"].map((e) => path.join(IMG_DIR, cod + e)).find((f) => fs.existsSync(f));
  // SKIP_MISSING=1: figurile fara captura sunt omise (varianta intermediara a raportului)
  if (!file && process.env.SKIP_MISSING) { state.missing.push(`- ${cod}`); return []; }
  const no = figNo();
  const out = [];
  if (!file) {
    state.missing.push(`${no} ${cod}`);
    out.push(new Paragraph({ alignment: AlignmentType.CENTER, keepNext: true, spacing: { before: 120 },
      children: [new TextRun({ text: `[CAPTURĂ LIPSĂ: ${cod}]`, color: "C00000", bold: true })] }));
  } else {
    state.used.push(cod);
    const { width, height } = imageSize(file);
    let w = Math.min(opts.maxW || MAX_W_PX, width);
    let h = Math.round((height * w) / width);
    const maxH = opts.maxH || MAX_H_PX;
    if (h > maxH) { h = maxH; w = Math.round((width * h) / height); }
    out.push(new Paragraph({ alignment: AlignmentType.CENTER, keepNext: true, spacing: { before: 120, after: 0, line: 240, lineRule: "auto" },
      children: [new ImageRun({ type: file.endsWith(".png") ? "png" : "jpg", data: fs.readFileSync(file),
        transformation: { width: w, height: h }, altText: { title: caption, description: caption, name: cod } })] }));
  }
  out.push(new Paragraph({ style: "Legenda", children: runs(`Figura ${no}. ${caption}`) }));
  return out;
}

function table(header, rows, widths, opts = {}) {
  const total = widths.reduce((a, b) => a + b, 0);
  const scale = CONTENT_W / total;
  const w = widths.map((x) => Math.round(x * scale));
  w[w.length - 1] += CONTENT_W - w.reduce((a, b) => a + b, 0);
  const border = { style: BorderStyle.SINGLE, size: 4, color: "808080" };
  const borders = { top: border, bottom: border, left: border, right: border };
  const fs_ = opts.size || 22;
  const cell = (text, i, head) => new TableCell({
    width: { size: w[i], type: WidthType.DXA }, borders, verticalAlign: VerticalAlign.CENTER,
    shading: head ? { type: ShadingType.CLEAR, color: "auto", fill: "DDE8E2" } : undefined,
    margins: { top: 40, bottom: 40, left: 80, right: 80 },
    children: String(text).split("\n").map((t) => new Paragraph({ spacing: { line: 240, before: 0, after: 0 },
      alignment: AlignmentType.LEFT, children: runs(t, { bold: head ? true : undefined, size: fs_ }) })),
  });
  const out = [];
  if (opts.caption) {
    state.tab += 1;
    const no = state.chapter ? `${state.chapter}.${state.tab}` : `${state.tab}`;
    out.push(new Paragraph({ style: "LegendaTabel", keepNext: true, children: runs(`Tabelul ${no}. ${opts.caption}`) }));
  }
  out.push(new Table({
    width: { size: CONTENT_W, type: WidthType.DXA }, columnWidths: w,
    rows: [new TableRow({ tableHeader: true, children: header.map((t, i) => cell(t, i, true)) }),
      ...rows.map((r) => new TableRow({ cantSplit: true, children: r.map((t, i) => cell(t, i, false)) }))],
  }));
  out.push(new Paragraph({ spacing: { before: 0, after: 0, line: 240 }, children: [] }));
  return out;
}

const pageBreak = () => new Paragraph({ children: [new PageBreak()] });

module.exports = { P, H, bullets, numbered, code, fig, table, pageBreak, runs, state, startChapter, FONT, MONO, CONTENT_W };
