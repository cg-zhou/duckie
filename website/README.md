# Duckie 网站

`duckie.cg-zhou.top` 的静态产品与下载页。首版仅介绍右键菜单切换，使用 web-booster；没有 API 或用户系统。

## 本地开发

需要 Node.js 22.12+：在本目录运行 `npm ci`、`npm run dev`。开发预览不下载 EXE，下载按钮指向 GitHub Releases。

## 正式构建

`release.json` 指定推荐版本。`npm run build` 从对应 GitHub Release 下载元数据和 EXE，验证版本、大小、SHA-256 后生成 `dist/`；失败则停止构建，避免部署无效下载。

Release 尚未发布时，在仓库根目录运行 `./scripts/package-release.ps1`，再使用本地包构建：

```powershell
cd website
$env:DUCKIE_RELEASE_DIR = (Resolve-Path ../artifacts/release/0.9.1).Path
npm run build
npm run preview
```

`public/`、`dist/`、二进制均为生成文件，不提交 Git。构建会重新生成 `public/`，不要把原始素材存进去。

## 首次部署阿里云 ESA Pages

1. 在 ESA「函数和 Pages」创建项目，关联 `cg-zhou/duckie`，生产分支选 `main`。
2. **根目录设为 `/website`**，Node.js 选 22。
3. `esa.jsonc` 已配置项目名 `duckie`、`npm ci`、`npm run build` 和产物目录 `./dist`。
4. 确认指定的 GitHub Release 已发布，再触发 ESA 部署。构建需要访问 GitHub 下载文件。
5. 绑定 `duckie.cg-zhou.top`，根据控制台提示配置 DNS 和 HTTPS。
6. 检查首页、EXE 下载、校验文件和 404 页面；文件校验值应与 GitHub Release 一致。

无需 KV 或额外下载密钥。当前版随 Pages 分发，历史版本链接到 GitHub Releases。单文件不能超过 25 MB，构建脚本会检查。

参考：[ESA Pages 构建配置](https://help.aliyun.com/zh/edge-security-acceleration/esa/user-guide/build-pages)。

## 后续版本

先发布桌面 Release，再更新本目录 `release.json` 并推送，触发 ESA 构建。首次接入 ESA 也应在 Release 发布后进行。

建议对版本化 `/downloads/` 使用长缓存，对 HTML 和 `/release.json` 使用短缓存；不要覆盖已发布版本的 EXE。
