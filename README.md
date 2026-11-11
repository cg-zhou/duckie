# Duckie

[中文](./README.zh-CN.md) · [Roadmap](./ROADMAP.md)

A small Windows utility. The public release focuses on switching between classic and Windows 11 context menus.

- Compare menu previews and click to select a style.
- Confirm before restarting File Explorer; restore system defaults at any time.
- Portable EXE: no installation, tray icon, or global hotkeys in the default mode. Closing the window exits.
- Single instance: opening again activates the existing window.

Requires Windows 11 and .NET Framework 4.8. Actual menu behavior depends on the Windows version. The portable binary is unsigned.

## Downloads

The website is prepared for `https://duckie.cg-zhou.top`. Versioned binaries, checksums and history live in [GitHub Releases](https://github.com/cg-zhou/duckie/releases).

All original toolbox code is retained. Exit the current instance and run `Duckie.exe --toolbox` to restore the original navigation, tray, hotkeys and close behavior. See [toolbox mode](./docs/toolbox.md).

## Development

On Windows, install the .NET Framework 4.8 targeting pack. Open `desktop/Duckie.slnx` in Visual Studio with the .NET desktop workload, or build:

```powershell
dotnet build desktop/Duckie/Duckie.csproj -c Release
./scripts/package-release.ps1
```

`VERSION` supplies the desktop version. Packaging produces the standalone EXE, `SHA256SUMS.txt`, and `release.json` in `artifacts/release/<version>/`.

| Directory | Contents |
| --- | --- |
| `desktop/` | WPF application, shared components, original toolbox, MSIX packaging |
| `website/` | web-booster product website and ESA Pages configuration |
| `assets/` | Shared screenshots |
| `scripts/` | Portable packaging |
| `docs/` | Usage and release notes |

## Publishing

Push a `vMAJOR.MINOR.PATCH` tag matching `VERSION`, with notes at `docs/releases/<tag>.md`. GitHub Actions publishes the portable release using its built-in token. Manual workflow runs only build an artifact.

After release files exist, update `website/release.json` and deploy the site. The build verifies the exact release binary before hosting it. See the [ESA deployment guide](./website/README.md).

The Microsoft Store workflow is retained; Store submission is manual so portable tags do not also publish Store updates.
