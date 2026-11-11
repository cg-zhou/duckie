# Duckie

[English](./README.md) · [路线图](./ROADMAP.md)

让日常操作更方便的 Windows 小工具。

本仓库包含 Duckie 桌面程序，以及为公开产品网站预留的目录。当前程序是基于 C#、WPF 和 .NET Framework 4.8 的托盘工具箱。后续计划提供按需运行、用完退出的独立小工具，目前尚未单独发布。

## 当前功能

- **PAC 管理**：保存和应用 PAC 地址，通过快捷键循环切换，也可以在托盘菜单直接选择。
- **音量快捷键**：调节系统音量、静音或取消静音。
- **快捷启动**：将应用启动键 2（Application 2）绑定到选定文件。
- **右键菜单切换**：切换经典和 Windows 11 右键菜单样式；操作会重启资源管理器。
- **偏好设置**：配置快捷键、开机启动、托盘行为、语言，以及 Duckie 自身请求使用的 HTTP 代理。

PAC 管理修改当前用户的 Windows PAC 设置，本身不提供代理服务器。

## 使用桌面程序

启动后，点击托盘图标显示或隐藏窗口，在程序内配置工具和快捷键。退出时选择托盘菜单的“退出”；根据设置，关闭窗口可能只是收起到托盘。

| 默认快捷键 | 操作 |
| --- | --- |
| Alt + Shift + E | 显示或隐藏 Duckie |
| Alt + Shift + 1 | 依次切换已保存的 PAC，最后清除 PAC 设置 |
| Alt + Shift + 8 / 9 / 0 | 调小音量 / 调大音量 / 静音 |
| Alt + Shift + 5 | 切换右键菜单样式并重启资源管理器 |

快捷键可在设置中修改。“快捷启动”按 Windows 文件关联打开所选文件。

## 仓库结构

```text
desktop/
  Duckie.slnx          Visual Studio 解决方案
  Directory.Build.props
  Duckie/             应用入口、主窗口和设置
  Duckie.Shared/      配置、本地化、控件、托盘和快捷键基础设施
  Duckie.Windows/     Windows 工具
  Duckie.Pack/        Microsoft Store / MSIX 打包
website/              公开产品网站预留目录
.github/workflows/    构建和发布工作流
```

## 开发

在 Windows 上，使用安装了“.NET 桌面开发”工作负载和 .NET Framework 4.8 目标包的 Visual Studio 打开 [`desktop/Duckie.slnx`](./desktop/Duckie.slnx)，构建并运行 `Duckie` 项目。Visual Studio 需支持 `.slnx` 格式。

已安装兼容的 .NET SDK 和目标包时，也可以在仓库根目录构建：

```powershell
dotnet build desktop/Duckie/Duckie.csproj -c Release
```

MSIX 打包另外需要 `desktop/Duckie.Pack` 引用的 Windows SDK 和 Desktop Bridge 打包工具。导航页面与快捷键服务通过 `INavView`、`IHotKeyService` 接口发现。

## 产品网站

网站计划使用 `duckie.cg-zhou.top`，通过阿里云 ESA Pages 静态托管，提供工具介绍、使用说明和下载入口。[`website/`](./website/) 目前只是预留目录，尚未实现页面或部署流程。

## 发布到 Microsoft Store

[`windows-store.yml`](./.github/workflows/windows-store.yml) 支持：

- 推送到 `main`：构建 MSIX 并保存为 Actions Artifact，不提交商店。
- 推送版本标签（如 `v0.8.4`）：构建并提交商店更新。
- 手动运行：可指定版本，并选择是否提交商店。

在 GitHub Repository secrets 中配置：

| Secret | 内容 |
| --- | --- |
| `AZURE_AD_TENANT_ID` | Microsoft Entra 租户 ID |
| `AZURE_AD_APPLICATION_CLIENT_ID` | Entra 应用的客户端 ID |
| `AZURE_AD_APPLICATION_SECRET` | Entra 应用的客户端密钥 |
| `SELLER_ID` | Partner Center Seller/Publisher ID |

Entra 应用还需要在 Partner Center 账户设置中获得相应访问权限。工作流默认使用现有 Store Product ID，也可通过 Repository variable `STORE_PRODUCT_ID` 覆盖。

应用首次商店资料和提交需在 Partner Center 完成。提交后仍需通过商店认证；工作流成功不表示立即上架。需要人工确认时，可为 `microsoft-store` Environment 配置 Required reviewers。

当前工作流不向 GitHub Releases 发布便携 EXE；独立工具的下载发布流程后续补充。
