# Duckie

[中文](./README.zh-CN.md) · [Roadmap](./ROADMAP.md)

A small Windows toolbox for context menu switching, PAC management and quick launch.

- Compare menu previews and click to select a style.
- Confirm before restarting File Explorer; restore system defaults at any time.
- Portable EXE with a tray icon, global hotkeys and configurable startup and close behavior.
- Single instance: opening again activates the existing window.

Requires Windows 11 and .NET Framework 4.8. Actual menu behavior depends on the Windows version. The portable binary is unsigned.

## Downloads

Download the ZIP from the [Duckie website](https://duckie.cg-zhou.top), extract it and run `Duckie.exe`. Checksums and historical versions are available in [GitHub Releases](https://github.com/cg-zhou/duckie/releases).

The current download, v0.9.2, provides context menu switching. The main branch is being organized into a full toolbox. See the [roadmap](./ROADMAP.md).

## Development

On Windows, install the .NET Framework 4.8 targeting pack. Open `desktop/Duckie.slnx` in Visual Studio with the .NET desktop workload, or build:

```powershell
dotnet build desktop/Duckie/Duckie.csproj -c Release
./scripts/package-release.ps1
```

`VERSION` supplies the desktop version. Packaging produces a ZIP containing only `Duckie.exe`, plus separate `SHA256SUMS.txt` and `release.json` files in `artifacts/release/<version>/`.

| Directory | Contents |
| --- | --- |
| `desktop/` | WPF application, shared components, MSIX packaging |
| `website/` | web-booster product website and ESA Pages configuration |
| `assets/` | Shared screenshots |
| `scripts/` | Portable packaging |

## Publishing

Push a `vMAJOR.MINOR.PATCH` tag matching `VERSION`, with a matching entry in [`CHANGELOG.md`](./CHANGELOG.md). GitHub Actions publishes the portable release using its built-in token. Manual workflow runs only build an artifact.

After release files exist, update `website/release.json` and deploy the site. The build verifies the exact release binary before hosting it. See the [ESA deployment guide](./website/README.md).

The Microsoft Store workflow is retained; Store submission is manual so portable tags do not also publish Store updates.
