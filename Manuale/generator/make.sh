#!/bin/bash
# Construieste manualele: bash make.sh [manual ...]   (implicit toate cinci)
# Cere: node + docx (npm), python3 + Pillow + matplotlib, graphviz, LibreOffice, poppler-utils, playwright (pentru machete)
set -e
cd "$(dirname "$0")"
GEN=$(pwd); IMG=$GEN/../imagini; OUT=$GEN/../build
mkdir -p "$IMG" "$OUT"
if [ -n "$MACHETE" ]; then   # MACHETE=1 bash make.sh -> redeseneaza machetele si diagramele
  python3 machete/machete.py "$OUT/html" && node machete/foto.js "$OUT/html" "$IMG"
  python3 diagrame.py "$IMG"
  python3 optimizeaza.py "$IMG"
fi
declare -A NUME=([utilizatori]="Manual_Utilizatori_Pasageri" [casieri]="Manual_Operatori_Casieri" [manageri]="Manual_Manageri"
                 [administratori]="Manual_Administratori" [devops]="Manual_Programator_DevOps")
for m in ${@:-utilizatori casieri manageri administratori devops}; do
  W=$OUT/$m; rm -rf "$W"; mkdir -p "$W"; cd "$W"
  node "$GEN/build.js" $m pas1.docx
  python3 "$GEN/postproces.py" pas1.docx pas2.docx
  soffice --headless --convert-to pdf pas2.docx >/dev/null 2>&1
  python3 "$GEN/pagini.py" pas2.pdf pagini.json
  python3 "$GEN/postproces.py" pas1.docx "$GEN/../${NUME[$m]}.docx" pagini.json
  soffice --headless --convert-to pdf "$GEN/../${NUME[$m]}.docx" >/dev/null 2>&1
  echo "$m: $(pdfinfo ${NUME[$m]}.pdf | grep Pages)"
  cd "$GEN"
done
