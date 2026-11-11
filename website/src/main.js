import 'web-booster/styles.css';
import { showMessage } from 'web-booster';
import './style.css';

let release;
try {
  const response = await fetch('/release.json', { cache: 'no-cache' });
  if (!response.ok) throw new Error('Release unavailable');
  const data = await response.json();
  if (data.available && /^\d+\.\d+\.\d+$/.test(data.version)
      && /^[a-f0-9]{64}$/.test(data.sha256)
      && data.downloadUrl === `/downloads/v${data.version}/Duckie-v${data.version}.exe`
      && data.checksumUrl === `/downloads/v${data.version}/SHA256SUMS.txt`) {
    release = data;
    const download = document.querySelector('#download');
    download.setAttribute('href', data.downloadUrl);
    download.textContent = '下载 Windows 版 ↓';
    document.querySelector('#release-note').textContent = `v${data.version} · ${(data.size / 1024).toFixed(0)} KB · Windows 11 · 无需安装`;
    document.querySelector('#checksum').textContent = data.sha256;
    document.querySelector('#checksum-link').href = data.checksumUrl;
    document.querySelector('#integrity').hidden = false;
  }
} catch {
  // The fallback is an actual releases page, never a broken or placeholder download.
  document.querySelector('#release-note').textContent = '本站下载暂不可用，可前往 GitHub 发布页 · Windows 11';
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
