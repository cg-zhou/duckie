# Duckie

[English](./README.md) · [路线图](./ROADMAP.md)

一个轻巧的 Windows 工具箱，提供右键菜单切换、PAC 管理和快捷启动。

- 对比经典与 Windows 11 菜单，点击图片选择。
- 切换前确认，操作会重启资源管理器；可恢复系统默认。
- 无需安装，支持托盘、全局快捷键及启动与关闭行为设置。
- 单实例运行，重复打开唤起已有窗口。

面向 Windows 11，需要 .NET Framework 4.8。菜单效果取决于系统版本；便携版未进行代码签名。

## 下载

从 [Duckie 网站](https://duckie.cg-zhou.top) 下载 ZIP，解压后运行 `Duckie.exe`。校验值和历史版本见 [GitHub Releases](https://github.com/cg-zhou/duckie/releases)。

当前下载版本为 v0.10.0，提供右键菜单切换、PAC 管理、音量控制和快捷启动，见[路线图](./ROADMAP.md)。

## 开发

Windows 上安装 .NET Framework 4.8 目标包；Visual Studio 需安装“.NET 桌面开发”，打开 `desktop/Duckie.slnx`。也可以运行：

```powershell
dotnet build desktop/Duckie/Duckie.csproj -c Release
./scripts/package-release.ps1
```

`VERSION` 是桌面版本来源。便携包输出到 `artifacts/release/<版本>/`，包含 ZIP（包内仅 Duckie.exe）、`SHA256SUMS.txt` 和 `release.json`。

| 目录 | 内容 |
| --- | --- |
| `desktop/` | WPF 程序、公共组件和 MSIX 打包 |
| `website/` | web-booster 产品网站与 ESA 配置 |
| `assets/` | 共享截图 |
| `scripts/` | 便携发布脚本 |

## 跨端视觉基线（试行）

Web-z11g 工具、Duckie 网站和 Duckie 桌面端逐步采用同一套语义与交互规则。
本基线由 Duckie 仓库维护。Web 组件颜色参考 web-booster 0.4.2 的 `src/styles/web-booster.css`；桌面端通过
[共享设计资源](./desktop/Duckie.Shared/Views/Styles/Design.xaml) 映射到 WPF，保留平台原生的输入和键盘行为。
这一阶段先以 Duckie 设置页验证模板，不要求已有页面一次迁移。

| 语义 | Web 变量及当前值 | WPF 资源 |
| --- | --- | --- |
| 主操作、启用状态 | `--wb-primary` / `#1e293b` | `DesignPrimaryBrush` |
| 正文 | `--wb-text` / `#0f172a` | `DesignTextBrush` |
| 辅助说明 | `--wb-text-muted` / `#64748b` | `DesignMutedBrush` |
| 卡片边框 | `--wb-card-border` / `#e5e7eb` | `DesignBorderBrush` |
| 控件边框 | `--wb-button-border` / `#d5dde7` | `DesignInputBorderBrush` |
| 浅色交互底色 | `--wb-disabled-bg` / `#f1f5f9` | `DesignHoverBrush` |
| 容器 / 控件圆角 | `--wb-radius-lg` / `--wb-radius-md`，12 / 8 | `DesignCardRadius` / `DesignControlRadius` |

共享规则：

- 以 4 为基础间距单位，常用 8、12、16、24、32；先组合共享布局，再增加页面专属样式。
- 正文、说明、分组标题、页面标题具有固定层级。桌面试行 13 / 12 / 16 / 28 DIP；Web 保留适合浏览器的字号与留白，不机械复制尺寸。
- 设置页采用“标题 → 分组 → 卡片 → 设置行”。设置行的标题与说明在左，操作在右；空间不足时操作移到下方，文字允许换行。
- 同一组设置使用一张卡片和内部分隔线，避免重复嵌套卡片。错误提示靠近对应操作。
- 共享控件负责正常、悬停、键盘焦点、选中和禁用状态；页面不再复制控件模板。
- 验证中英文、窄窗口、系统缩放与键盘操作；控件使用最小高度，避免固定高度裁切文字。
- Logo、品牌点缀和首页展示保留产品差异；状态色只表达状态，不用于普通装饰。

## 发布

1. 更新 `VERSION`，在 [`CHANGELOG.md`](./CHANGELOG.md) 顶部补充简短更新记录；需要维护商店包时同步更新 manifest 版本。
2. 提交代码，推送与版本一致的标签，如 `v0.9.2`。
3. `portable-release.yml` 构建文件并创建 GitHub Release，使用内置 `GITHUB_TOKEN`，无需额外密钥。手动执行仅生成 Artifact。
4. Release 文件就绪后更新网站 `release.json` 并部署，见 [ESA 部署说明](./website/README.md)。

原 Microsoft Store 工作流保留，但普通版本标签不再自动提交商店。商店发布改为手动执行并选择提交，继续使用原 Partner Center Secrets。
