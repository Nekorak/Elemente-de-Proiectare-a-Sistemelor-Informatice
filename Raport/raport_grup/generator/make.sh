set -e
cd /tmp/claude-0/w/gen
IMG_DIR=${IMG_DIR:-/tmp/claude-0/w/img_opt} node build.js ../raport_pas1.docx
python3 postproces.py ../raport_pas1.docx ../raport_pas2.docx
cd .. && rm -rf pdf && mkdir pdf && soffice --headless --convert-to pdf --outdir pdf raport_pas2.docx >/dev/null 2>&1
cd gen && python3 pagini.py ../pdf/raport_pas2.pdf pagini.json
python3 postproces.py ../raport_pas1.docx ../raport_final.docx pagini.json
