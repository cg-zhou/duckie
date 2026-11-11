import { readFileSync, writeFileSync } from 'node:fs';

const version = process.argv[2]?.replace(/^v/, '');
if (!/^\d+\.\d+\.\d+$/.test(version ?? '')) {
  throw new Error('Usage: node scripts/release-notes.mjs <version> [output-file]');
}

const lines = readFileSync(new URL('../CHANGELOG.md', import.meta.url), 'utf8').split(/\r?\n/);
const matches = lines.flatMap((line, index) => line.trim() === `## ${version}` ? [index] : []);
if (matches.length !== 1) throw new Error(`Expected one CHANGELOG entry for ${version}, found ${matches.length}`);
const start = matches[0] + 1;
const next = lines.findIndex((line, index) => index >= start && /^##\s/.test(line));
const notes = lines.slice(start, next < 0 ? undefined : next).join('\n').trim();
if (!notes) throw new Error(`Empty release notes for ${version}`);

if (process.argv[3]) writeFileSync(process.argv[3], `${notes}\n`);
else process.stdout.write(`${notes}\n`);
