import { mkdir, readFile, writeFile, copyFile, rm } from 'node:fs/promises';
import { createHash } from 'node:crypto';
import { fileURLToPath } from 'node:url';
import path from 'node:path';

const root = path.resolve(fileURLToPath(new URL('../', import.meta.url)));
const repo = path.resolve(root, '..');
const output = path.join(root, 'public');
if (path.dirname(output) !== root || path.basename(output) !== 'public') throw new Error('Invalid generated asset directory');
// public/ is entirely generated; original artwork remains outside this directory.
await rm(output, { recursive: true, force: true });
await mkdir(path.join(output, 'assets'), { recursive: true });
for (const name of ['classic.png', 'windows-11.png']) {
  await copyFile(path.join(repo, 'assets/screenshots/context-menu', name), path.join(output, 'assets', name));
}
await copyFile(path.join(repo, 'desktop/Duckie/EmbeddedResources/duck.svg'), path.join(output, 'assets/duck.svg'));
await writeFile(path.join(output, 'robots.txt'), 'User-agent: *\nAllow: /\nSitemap: https://duckie.cg-zhou.top/sitemap.xml\n');
await writeFile(path.join(output, 'sitemap.xml'), '<?xml version="1.0" encoding="UTF-8"?><urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><url><loc>https://duckie.cg-zhou.top/</loc></url></urlset>');

if (process.argv.includes('--preview')) {
  await writeFile(path.join(output, 'release.json'), JSON.stringify({ available: false }));
  console.log('Preview assets prepared; download links point to GitHub Releases.');
  process.exit(0);
}

const selected = JSON.parse(await readFile(path.join(root, 'release.json'), 'utf8'));
if (!/^\d+\.\d+\.\d+$/.test(selected.version) || selected.repository !== 'cg-zhou/duckie') throw new Error('Invalid release selection');
const tag = `v${selected.version}`;
const base = `https://github.com/${selected.repository}/releases/download/${tag}`;
async function load(name) {
  if (process.env.DUCKIE_RELEASE_DIR) return readFile(path.join(path.resolve(process.env.DUCKIE_RELEASE_DIR), name));
  const response = await fetch(`${base}/${name}`, { signal: AbortSignal.timeout(90000) });
  if (!response.ok) throw new Error(`Cannot download ${tag}/${name}: HTTP ${response.status}. Publish the Release first, or set DUCKIE_RELEASE_DIR for a local build.`);
  return Buffer.from(await response.arrayBuffer());
}
const release = JSON.parse((await load('release.json')).toString('utf8'));
const fileName = `Duckie-${tag}.zip`;
if (release.version !== selected.version || release.tag !== tag || release.fileName !== fileName || !/^[a-f0-9]{64}$/.test(release.sha256)) throw new Error('Release metadata does not match selected version');
const binary = await load(fileName);
if (binary.length > 25 * 1024 * 1024) throw new Error('ZIP exceeds ESA Pages single-file limit');
if (binary.length !== release.size || createHash('sha256').update(binary).digest('hex') !== release.sha256) throw new Error('Release ZIP checksum or size mismatch');
const downloads = path.join(output, 'downloads', tag);
await mkdir(downloads, { recursive: true });
await writeFile(path.join(downloads, fileName), binary);
await writeFile(path.join(downloads, 'SHA256SUMS.txt'), `${release.sha256}  ${fileName}\n`);
await writeFile(path.join(output, 'release.json'), JSON.stringify({ ...release, available: true, downloadUrl: `/downloads/${tag}/${fileName}`, checksumUrl: `/downloads/${tag}/SHA256SUMS.txt` }, null, 2));
console.log(`Verified and prepared ${fileName}: ${binary.length} bytes`);
