# Duckie

[English](./README.md)

一个开源的 Windows 轻量桌面工具箱，让常用操作触手可及。

Duckie 把日常高频的小工具集中到系统托盘中：需要时打开窗口，或用一个全局快捷键完成操作，然后继续手头的工作。功能围绕一个简单的标准取舍：减少重复操作，适合通过托盘或热键随手调用。

## 功能

- **托盘常驻**：点击托盘图标显示或隐藏主窗口，可配置开机启动、启动时最小化和关闭时收起到托盘。
- **PAC 管理**：添加、编辑、删除和应用 PAC 配置，支持热键轮换，托盘图标徽章提示当前配置。
- **音量控制**：使用全局快捷键调节系统音量、静音或取消静音。
- **快捷启动**：将应用启动键 2（Application 2）绑定到一个文件，通过命令行 `start` 打开。
- **右键菜单样式切换**：切换经典和 Windows 11 右键菜单样式；操作会重启资源管理器。
- **偏好设置**：配置热键、切换中英文、设置应用 HTTP 代理并测试连接。

PAC 管理修改当前用户的 Windows PAC 设置，本身不提供代理服务器。设置页中的 HTTP 代理用于 Duckie 自身的 HTTP 请求。

## 开始使用

启动 Duckie 后，点击托盘图标显示或隐藏窗口。在 PAC 管理页面添加自己的 PAC 地址，在设置中调整启动和窗口行为。需要退出时，选择托盘菜单中的 **Exit**。

默认快捷键：

| 快捷键 | 操作 |
| --- | --- |
| Alt + Shift + E | 显示或隐藏 Duckie |
| Alt + Shift + 1 | 依次切换已保存的 PAC 配置，最后清除 PAC 设置 |
| Alt + Shift + 8 | 调小音量 |
| Alt + Shift + 9 | 调大音量 |
| Alt + Shift + 0 | 静音或取消静音 |
| Alt + Shift + 5 | 切换右键菜单样式并重启资源管理器 |

快捷键可能与其他应用冲突，可在设置中查看或调整。

在“快捷启动”中选择一个文件，启用并保存后立即生效。Duckie 调用 `cmd /d /v:off /c start "" "文件路径"`，具体打开方式由系统文件关联决定，不创建快捷方式文件。禁用功能或退出 Duckie 后释放按键。

## 开发

项目使用 C#、WPF 和 .NET Framework 4.8，仅面向 Windows。

| 项目 | 职责 |
| --- | --- |
| `Duckie` | 应用入口、主窗口和设置 |
| `Duckie.Shared` | 公共控件、配置、本地化、托盘和热键基础设施 |
| `Duckie.Windows` | PAC、音量、快捷启动等 Windows 工具 |
| `Duckie.Pack` | MSIX 打包 |

在 Windows 上，使用安装了 **.NET 桌面开发**工作负载和 .NET Framework 4.8 目标包的 Visual Studio 打开 `Duckie.slnx`，构建并运行 `Duckie` 项目。MSIX 打包还需安装 `Duckie.Pack` 引用的 Windows SDK 和打包工具。

已安装兼容的 .NET SDK 和目标包时，也可以在仓库根目录构建桌面应用：

```powershell
dotnet build Duckie/Duckie.csproj -c Release
```

导航页面与热键服务分别通过 `INavView`、`IHotKeyService` 接口自动发现。欢迎贡献能减少重复操作、适合托盘或快捷键调用的小工具。

## GitHub Actions 自动发布 Microsoft Store

仓库包含 [`windows-store.yml`](./.github/workflows/windows-store.yml) 工作流：

- 推送到 `main`：构建 MSIX 并上传为 GitHub Actions Artifact，不提交商店
- 推送版本标签（例如 `v0.8.3`）：构建并自动提交到 Microsoft Store
- 手动运行工作流：可输入版本号，并选择是否提交商店

首次使用前，需要在 GitHub 仓库的 `Settings → Secrets and variables → Actions` 中添加以下 Repository secrets：

| Secret | 来源 |
|--------|------|
| `AZURE_AD_TENANT_ID` | Microsoft Entra 租户 ID |
| `AZURE_AD_APPLICATION_CLIENT_ID` | Entra 应用的 Application (client) ID |
| `AZURE_AD_APPLICATION_SECRET` | Entra 应用创建的 Client secret 值 |
| `SELLER_ID` | Partner Center 的 Seller/Publisher ID |

Entra 应用还必须在 Partner Center 的账户设置中添加，并至少授予 `Manager` 角色。默认 Store Product ID 来自 `Package.StoreAssociation.xml`；如果应用变更，可在 GitHub Actions Variables 中设置 `STORE_PRODUCT_ID` 覆盖它。

应用需要先在 Partner Center 完成首次提交并处于可更新状态；如果还没有上架过，首次提交仍需手动完成商店资料和认证。若希望每次标签发布前人工确认，可在 GitHub 创建名为 `microsoft-store` 的 Environment，并添加 Required reviewers。

注意：Microsoft Store 仍会执行认证审核；工作流完成表示已提交，不代表立即对所有用户上架。

