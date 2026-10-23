# Duckie

[中文](./README.zh-CN.md)

An open-source, lightweight Windows desktop toolbox that keeps everyday actions within reach.

Duckie brings small, frequently used utilities together in a tray app. Show it when needed, use a global hotkey, and get back to work. Its focus is practical tools that reduce repetitive steps and fit naturally into a tray menu or keyboard shortcut.

## Features

- **Tray integration**: Click the tray icon to show or hide the main window. Configure startup, start minimized, and minimize to tray on close.
- **PAC management**: Add, edit, remove, and apply PAC configurations. Cycle through saved configurations with a hotkey and see the active configuration indicated by a tray icon badge.
- **Volume control**: Adjust system volume and toggle mute with global hotkeys.
- **Open terminal here**: Open a terminal at the active File Explorer folder, preferring Windows Terminal with PowerShell and CMD fallbacks.
- **Context menu style**: Switch between classic and Windows 11 context menus. This action restarts File Explorer.
- **Settings**: Configure hotkeys, switch between Chinese and English, and configure an application HTTP proxy with a connection test.

PAC management changes the current user's Windows PAC setting; it does not provide a proxy server. The HTTP proxy in Settings applies to Duckie's own HTTP requests.

## Getting started

Launch Duckie, then click its tray icon to show or hide the window. Add your PAC URLs in the PAC management page and configure startup and window behavior in Settings. To quit, choose **Exit** from the tray menu.

Default shortcuts:

| Shortcut | Action |
| --- | --- |
| Alt + Shift + E | Show or hide Duckie |
| Alt + Shift + 1 | Cycle through saved PAC configurations, then clear the PAC setting |
| Alt + Shift + 8 | Volume down |
| Alt + Shift + 9 | Volume up |
| Alt + Shift + 0 | Mute or unmute |
| Alt + Shift + ` | Open a terminal at the active File Explorer folder |
| Alt + Shift + 5 | Toggle context menu style and restart File Explorer |

Shortcuts may conflict with other applications. Review or change them in Settings.

## Development

Duckie uses C#, WPF, and .NET Framework 4.8. It is a Windows application.

| Project | Responsibility |
| --- | --- |
| `Duckie` | Application entry point, main window, and settings |
| `Duckie.Shared` | Shared controls, configuration, localization, tray, and hotkey infrastructure |
| `Duckie.Windows` | Windows utilities, including PAC, volume, and terminal tools |
| `Duckie.Pack` | MSIX packaging |

On Windows, open `Duckie.sln` in Visual Studio with the **.NET desktop development** workload and the .NET Framework 4.8 targeting pack installed. Build and run the `Duckie` project. MSIX packaging additionally requires the Windows SDK and packaging tools referenced by `Duckie.Pack`.

With a compatible .NET SDK and targeting pack, the desktop app can also be built from the repository root:

```powershell
dotnet build Duckie/Duckie.csproj -c Release
```

Navigation pages and hotkey services are discovered through the `INavView` and `IHotKeyService` interfaces. Contributions are welcome, especially small tools that save repeated steps and work well from the tray or a hotkey.

## Microsoft Store publishing

The [Windows Store workflow](./.github/workflows/windows-store.yml) supports:

- Push to `main`: Build an MSIX artifact without submitting it to the Store.
- Push a version tag such as `v0.8.3`: Build and submit a Store update.
- Manual run: Optionally specify a version and request Store submission.

Submission requires repository secrets `AZURE_AD_TENANT_ID`, `AZURE_AD_APPLICATION_CLIENT_ID`, `AZURE_AD_APPLICATION_SECRET`, and `SELLER_ID`, plus the appropriate Partner Center application access. See the [Chinese publishing guide](./README.zh-CN.md#github-actions-自动发布-microsoft-store) for setup details.
