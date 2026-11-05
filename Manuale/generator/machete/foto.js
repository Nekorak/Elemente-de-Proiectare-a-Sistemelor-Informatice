// Fotografiaza machetele: node foto.js <folder_html> <folder_png>
const { chromium } = require("playwright");
const fs = require("fs"), path = require("path");
(async () => {
  const [src, dst] = process.argv.slice(2);
  fs.mkdirSync(dst, { recursive: true });
  const browser = await chromium.launch();
  const page = await browser.newPage({ deviceScaleFactor: 2, viewport: { width: 1700, height: 1100 } });
  for (const f of fs.readdirSync(src).filter((x) => x.endsWith(".html")).sort()) {
    await page.goto("file://" + path.resolve(src, f));
    await page.evaluate(() => document.fonts.ready);
    await page.waitForTimeout(150);
    await page.locator("#shot").screenshot({ path: path.join(dst, f.replace(".html", ".png")) });
  }
  await browser.close();
})();
