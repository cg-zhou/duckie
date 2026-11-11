# Duckie

[English](./README.md) · [路线图](./ROADMAP.md)

一个轻巧的 Windows 小工具。当前公开版本聚焦 **Windows 11 右键菜单切换**。

- 对比经典与 Windows 11 菜单，点击图片选择。
- 切换前确认，操作会重启资源管理器；可恢复系统默认。
- 无需安装，关闭窗口即退出，不注册全局快捷键、不创建托盘图标。
- 单实例运行，重复打开唤起已有窗口。

面向 Windows 11，需要 .NET Framework 4.8。菜单效果取决于系统版本；便携版未进行代码签名。

## 下载

网站准备部署到 `https://duckie.cg-zhou.top`。正式文件、校验值和历史版本见 [GitHub Releases](https://github.com/cg-zhou/duckie/releases)。

旧工具箱代码全部保留。退出当前实例后，以 `Duckie.exe --toolbox` 启动可恢复原有导航、托盘、快捷键和关闭行为，见[工具箱模式](./docs/toolbox.md)。

## 开发

Windows 上安装 .NET Framework 4.8 目标包；Visual Studio 需安装“.NET 桌面开发”，打开 `desktop/Duckie.slnx`。也可以运行：

```powershell
dotnet build desktop/Duckie/Duckie.csproj -c Release
./scripts/package-release.ps1
```

`VERSION` 是桌面版本来源。便携包输出到 `artifacts/release/<版本>/`，包含 EXE、`SHA256SUMS.txt` 和 `release.json`。

| 目录 | 内容 |
| --- | --- |
| `desktop/` | WPF 程序、公共组件、旧工具箱和 MSIX 打包 |
| `website/` | web-booster 产品网站与 ESA 配置 |
| `assets/` | 共享截图 |
| `scripts/` | 便携发布脚本 |
| `docs/` | 使用说明和版本记录 |

## 发布

1. 更新 `VERSION`，补充 `docs/releases/v<版本>.md`；需要维护商店包时同步更新 manifest 版本。
2. 提交代码，推送与版本一致的标签，如 `v0.9.0`。
3. `portable-release.yml` 构建文件并创建 GitHub Release，使用内置 `GITHUB_TOKEN`，无需额外密钥。手动执行仅生成 Artifact。
4. Release 文件就绪后更新网站 `release.json` 并部署，见 [ESA 部署说明](./website/README.md)。

原 Microsoft Store 工作流保留，但普通版本标签不再自动提交商店。商店发布改为手动执行并选择提交，继续使用原 Partner Center Secrets。
