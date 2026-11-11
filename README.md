# Duckie

[中文](./README.zh-CN.md)

Small Windows utilities for everyday tasks.

This repository contains the Duckie desktop application and a directory reserved for its public website. The current application is a tray toolbox built with C#, WPF, and .NET Framework 4.8. Standalone utilities that run only when needed are a planned direction; they are not shipped separately yet.

## Current features

- **PAC management**: Save and apply PAC URLs, cycle through them with a hotkey, and select configurations from the tray menu.
- **Volume shortcuts**: Adjust system volume and toggle mute.
- **Quick launch**: Bind Application 2 to a selected file.
- **Context menu switching**: Switch between classic and Windows 11 context menu styles. This restarts File Explorer.
- **Preferences**: Configure hotkeys, startup and tray behavior, language, and an HTTP proxy for Duckie's own requests.

PAC management changes the current user's Windows PAC setting; it does not provide a proxy server.

## Using the desktop app

Launch Duckie and click the tray icon to show or hide its window. Configure tools and shortcuts in the application. Choose **Exit** in the tray menu to quit; closing the window may hide it to the tray according to your settings.

| Default shortcut | Action |
| --- | --- |
| Alt + Shift + E | Show or hide Duckie |
| Alt + Shift + 1 | Cycle through saved PAC URLs, then clear the PAC setting |
| Alt + Shift + 8 / 9 / 0 | Volume down / up / mute |
| Alt + Shift + 5 | Toggle context menu style and restart File Explorer |

Shortcuts can be changed in Settings. Quick Launch uses the Windows file association to open the selected file.

## Repository layout

```text
desktop/
  Duckie.slnx          Visual Studio solution
  Directory.Build.props
  Duckie/             Application entry point, window, and settings
  Duckie.Shared/      Configuration, localization, controls, tray, and hotkeys
  Duckie.Windows/     Windows-specific tools
  Duckie.Pack/        Microsoft Store / MSIX packaging
website/              Public website placeholder
.github/workflows/    Build and publishing workflows
```

## Development

On Windows, open [`desktop/Duckie.slnx`](./desktop/Duckie.slnx) in Visual Studio with the **.NET desktop development** workload and .NET Framework 4.8 targeting pack installed. Build and run `Duckie`. Use a Visual Studio version that supports `.slnx` solutions.

With a compatible .NET SDK and targeting pack, build from the repository root:

```powershell
dotnet build desktop/Duckie/Duckie.csproj -c Release
```

MSIX packaging also requires the Windows SDK and Desktop Bridge packaging tools referenced by `desktop/Duckie.Pack`. Navigation pages and hotkey services are discovered through `INavView` and `IHotKeyService`.

## Website

The product website is planned at `duckie.cg-zhou.top`, with static hosting on Alibaba Cloud ESA Pages. It will provide tool descriptions, guides, and download links. The [`website/`](./website/) directory is a placeholder; no website deployment is included yet.

## Microsoft Store publishing

The [Store workflow](./.github/workflows/windows-store.yml) builds an MSIX artifact on pushes to `main`. Version tags such as `v0.8.4` build and submit an update to Microsoft Store. Manual runs can optionally submit a specified version.

Submission requires GitHub repository secrets and Partner Center access. See the [Chinese setup guide](./README.zh-CN.md#发布到-microsoft-store). This workflow does not publish a portable EXE to GitHub Releases.
