import 'web-booster/styles.css';
import { showMessage } from 'web-booster';
import './style.css';
import selected from '../release.json';

const download = document.querySelector('#download');
const backupUrl = `/downloads/v${selected.version}/Duckie-v${selected.version}.zip`;
download.setAttribute('href', `https://github.com/${selected.repository}/releases/download/v${selected.version}/Duckie-v${selected.version}.zip`);
document.querySelector('#backup-download').href = backupUrl;
document.querySelector('#backup-download-help').href = backupUrl;
download.addEventListener('click', () => {
  document.querySelector('#download-help').hidden = false;
});

let release;
try {
  const response = await fetch('/release.json', { cache: 'no-cache' });
  if (!response.ok) throw new Error('Release unavailable');
  const data = await response.json();
  if (data.available && data.version === selected.version
      && /^[a-f0-9]{64}$/.test(data.sha256)
      && data.downloadUrl === `/downloads/v${data.version}/Duckie-v${data.version}.zip`
      && data.checksumUrl === `/downloads/v${data.version}/SHA256SUMS.txt`) {
    release = data;
    document.querySelector('#release-note').textContent = `v${data.version} · ZIP ${(data.size / 1024).toFixed(0)} KB · Windows 11 · 解压即用`;
    document.querySelector('#checksum').textContent = data.sha256;
    document.querySelector('#checksum-link').href = data.checksumUrl;
    document.querySelector('#integrity').hidden = false;
  }
} catch {
  // Version details are optional; both links still use the build's selected release.
}

document.querySelector('#copy-checksum').addEventListener('click', async () => {
  if (!release) return;
  try {
    await navigator.clipboard.writeText(release.sha256);
    document.querySelector('#copy-status').textContent = '已复制校验值';
    showMessage({ text: '已复制校验值', type: 'success' });
  } catch {
    document.querySelector('#copy-status').textContent = '请选中校验值手动复制';
    showMessage({ text: '请选中校验值手动复制', type: 'info' });
  }
});
