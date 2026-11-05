"""Micsoreaza imaginile generate (paleta de 256 de culori pentru machete si diagrame), ca manualele sa nu fie prea mari."""
import glob, os, sys
from PIL import Image
for f in glob.glob(os.path.join(sys.argv[1], "*.png")):
    im = Image.open(f).convert("RGB")
    if im.width > 2200:
        im = im.resize((2200, round(im.height * 2200 / im.width)), Image.LANCZOS)
    im.quantize(256, method=Image.Quantize.MEDIANCUT, dither=Image.Dither.NONE).save(f, optimize=True)
