#!/bin/bash
# Construiește documentele individuale ale membrilor: bash make.sh [membru ...]
# Cere: node + docx (npm, NODE_PATH=$(npm root -g)), LibreOffice și poppler-utils pentru verificarea paginilor
set -e
cd "$(dirname "$0")"
export NODE_PATH=${NODE_PATH:-$(npm root -g)}
declare -A NUME=([dmitri]="Gojinevschii_Dmitri_Baza_de_date" [sergiu]="Hanganu_Sergiu_Backend_DevOps"
                 [alexandr]="Crivenco_Alexandr_Frontend" [maxim]="Sopivnic_Maxim_Testare")
OUT=$(pwd)/..; TMP=$(mktemp -d)
for m in ${@:-dmitri sergiu alexandr maxim}; do
  node build.js $m "$OUT/${NUME[$m]}.docx"
  soffice --headless --convert-to pdf --outdir "$TMP" "$OUT/${NUME[$m]}.docx" >/dev/null 2>&1
  echo "$m: $(pdfinfo "$TMP/${NUME[$m]}.pdf" | grep Pages)"
done
echo "PDF-urile de verificare: $TMP"
