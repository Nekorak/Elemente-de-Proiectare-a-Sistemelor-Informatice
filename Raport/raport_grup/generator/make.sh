#!/bin/bash
# Construieste raportul de grup: bash make.sh   ->  ../raport_proiect_grup.docx
# Cere: node + docx (npm), python3, LibreOffice, poppler-utils.
# Implicit: IMG_DIR=../capturi, SKIP_MISSING=1 (figurile fara captura sunt omise).
set -e
cd "$(dirname "$0")"
GEN=$(pwd)
export NODE_PATH=${NODE_PATH:-$(npm root -g)}
export IMG_DIR=${IMG_DIR:-$GEN/../capturi}
export SKIP_MISSING=${SKIP_MISSING-1}
W=$(mktemp -d)
node build.js "$W/raport_pas1.docx"
python3 postproces.py "$W/raport_pas1.docx" "$W/raport_pas2.docx"
soffice --headless --convert-to pdf --outdir "$W" "$W/raport_pas2.docx" >/dev/null 2>&1
python3 pagini.py "$W/raport_pas2.pdf" "$W/pagini.json"
python3 postproces.py "$W/raport_pas1.docx" "$GEN/../raport_proiect_grup.docx" "$W/pagini.json"
soffice --headless --convert-to pdf --outdir "$W" "$GEN/../raport_proiect_grup.docx" >/dev/null 2>&1
echo "raport_proiect_grup.docx: $(pdfinfo "$W/raport_proiect_grup.pdf" | grep Pages)  (fisiere de lucru: $W)"
