namespace Duckie.Shared.Utils.Localization;

public enum LocKey
{
    // Unknown
    [Cn("Unknown {0}")]
    [En("Unknown {0}")]
    UnknownKey,

    // Application Title
    [Cn("Duckie")]
    [En("Duckie")]
    AppTitle,

    [Cn("图像处理与PAC管理工具")]
    [En("Image Processing and PAC Management Tool")]
    AppDescription,

    [Cn("轻量级开源实用工具")]
    [En("A lightweight open-source utility")]
    AppSubtitle,

    // Navigation Items
    [Cn("音量")]
    [En("Volume")]
    Nav_Volume,

    [Cn("快捷键")]
    [En("Hotkey")]
    Nav_Hotkey,

    [Cn("终端")]
    [En("Terminal")]
    Nav_Terminal,

    [Cn("图像")]
    [En("Image")]
    Nav_Image,

    [Cn("PAC")]
    [En("PAC")]
    Nav_PAC,

    [Cn("音量")]
    [En("Volume")]
    Nav_VolumeControl,

    [Cn("关于")]
    [En("About")]
    Nav_About,

    [Cn("设置")]
    [En("Settings")]
    Nav_Settings,

    [Cn("语言")]
    [En("Language")]
    Nav_Language,

    // Window Titles
    [Cn("Duckie - 图像处理")]
    [En("Duckie - Image Processing")]
    Title_Image,

    [Cn("Duckie - PAC管理")]
    [En("Duckie - PAC Management")]
    Title_PAC,

    [Cn("Duckie - 音量控制")]
    [En("Duckie - Volume Control")]
    Title_VolumeControl,

    [Cn("Duckie - 热键管理")]
    [En("Duckie - Hotkey Management")]
    Title_HotkeyManagement,

    [Cn("Duckie - 网络测试")]
    [En("Duckie - Network Test")]
    Title_NetworkTest,

    [Cn("Duckie - 代码生成")]
    [En("Duckie - Code Generation")]
    Title_CodeGeneration,

    [Cn("Duckie - 关于")]
    [En("Duckie - About")]
    Title_About,

    [Cn("Duckie - 设置")]
    [En("Duckie - Settings")]
    Title_Settings,

    // Image Processing
    [Cn("打开")]
    [En("Open")]
    Btn_Open,

    [Cn("导出图标")]
    [En("Export ICO")]
    Btn_ExportICO,

    [Cn("逆时针旋转90°")]
    [En("Rotate 90° Counter-clockwise")]
    Tooltip_RotateLeft,

    [Cn("顺时针旋转90°")]
    [En("Rotate 90° Clockwise")]
    Tooltip_RotateRight,

    [Cn("水平翻转")]
    [En("Flip Horizontal")]
    Tooltip_FlipHorizontal,

    [Cn("垂直翻转")]
    [En("Flip Vertical")]
    Tooltip_FlipVertical,

    [Cn("放大")]
    [En("Zoom In")]
    Tooltip_ZoomIn,

    [Cn("缩小")]
    [En("Zoom Out")]
    Tooltip_ZoomOut,

    [Cn("适应窗口")]
    [En("Fit to Window")]
    Tooltip_FitToWindow,

    // PAC Management
    [Cn("添加代理")]
    [En("Add PAC")]
    Btn_AddPAC,

    [Cn("编辑")]
    [En("Edit")]
    Btn_Edit,

    [Cn("删除")]
    [En("Delete")]
    Btn_Delete,

    [Cn("无代理")]
    [En("No PAC")]
    Label_NoPAC,

    [Cn("直接连接")]
    [En("Direct Connection")]
    Label_DirectConnection,

    // Dialog Titles
    [Cn("添加PAC配置")]
    [En("Add PAC Configuration")]
    Dialog_AddPAC,

    [Cn("编辑PAC配置")]
    [En("Edit PAC Configuration")]
    Dialog_EditPAC,

    [Cn("语言设置")]
    [En("Language Settings")]
    Dialog_LanguageSettings,

    [Cn("确认删除")]
    [En("Confirm Delete")]
    Dialog_ConfirmDelete,

    // Form Labels
    [Cn("配置名称：")]
    [En("Configuration Name:")]
    Label_ConfigName,

    [Cn("PAC地址：")]
    [En("PAC URL:")]
    Label_PACURL,

    [Cn("支持的格式：")]
    [En("Supported formats:")]
    Label_SupportedFormats,

    [Cn("• 网络地址：http://example.com/proxy.pac")]
    [En("• Network URL: http://example.com/proxy.pac")]
    Label_NetworkURL,

    [Cn("• 本地文件：file:///C:/proxy.pac")]
    [En("• Local file: file:///C:/proxy.pac")]
    Label_LocalFile,

    [Cn("选择语言：")]
    [En("Select Language:")]
    Label_SelectLanguage,

    [Cn("注意：语言更改将立即生效。")]
    [En("Note: Language changes will take effect immediately.")]
    Label_LanguageNote,

    [Cn("某些界面元素可能需要重启应用程序才能完全更新。")]
    [En("Some UI elements may require application restart to fully update.")]
    Label_LanguageRestart,

    // Status Messages
    [Cn("就绪")]
    [En("Ready")]
    Status_Ready,

    [Cn("已加载：{0}")]
    [En("Loaded: {0}")]
    Status_Loaded,

    [Cn("PAC配置 '{0}' 添加成功")]
    [En("PAC configuration '{0}' added successfully")]
    Status_PACAdded,

    [Cn("PAC配置 '{0}' 更新成功")]
    [En("PAC configuration '{0}' updated successfully")]
    Status_PACUpdated,

    [Cn("PAC配置 '{0}' 删除成功")]
    [En("PAC configuration '{0}' deleted successfully")]
    Status_PACDeleted,

    [Cn("已清除代理")]
    [En("PAC cleared")]
    Status_PACCleared,

    [Cn("已切换到代理：{0}")]
    [En("Switched to PAC: {0}")]
    Status_PACSwitch,

    [Cn("代理操作失败：{0}")]
    [En("PAC operation failed: {0}")]
    Status_PACFailed,

    // Error Messages
    [Cn("警告")]
    [En("Warning")]
    Error_Warning,

    [Cn("错误")]
    [En("Error")]
    Error_Error,

    [Cn("异常：{0}")]
    [En("Exception: {0}")]
    Error_Exception,

    [Cn("打开图像失败：{0}")]
    [En("Failed to open image: {0}")]
    Error_FailedToOpenImage,

    [Cn("请先打开图像")]
    [En("Please open an image")]
    Error_PleaseOpenImage,

    [Cn("保存图像")]
    [En("Save Image")]
    Error_SaveImage,

    [Cn("添加PAC失败。名称或URL可能已存在。")]
    [En("Failed to add PAC. Name or URL may already exist.")]
    Error_FailedToAddPAC,

    [Cn("更新PAC配置失败")]
    [En("Failed to update PAC configuration")]
    Error_FailedToUpdatePAC,

    [Cn("删除PAC配置失败")]
    [En("Failed to delete PAC configuration")]
    Error_FailedToDeletePAC,

    [Cn("加载PAC配置失败")]
    [En("Failed to load PAC configurations")]
    Error_FailedToLoadPAC,

    [Cn("旋转图像失败")]
    [En("Failed to rotate image")]
    Error_FailedToRotateImage,

    [Cn("翻转图像失败")]
    [En("Failed to flip image")]
    Error_FailedToFlipImage,

    // Success Messages
    [Cn("图标已导出：{0}")]
    [En("The icon has been exported: {0}")]
    Success_IconExported,

    // Common Buttons
    [Cn("确定")]
    [En("OK")]
    Btn_OK,

    [Cn("取消")]
    [En("Cancel")]
    Btn_Cancel,

    [Cn("是")]
    [En("Yes")]
    Btn_Yes,

    [Cn("否")]
    [En("No")]
    Btn_No,

    // Confirmation Messages
    [Cn("确定要删除PAC配置 '{0}' 吗？")]
    [En("Are you sure you want to delete PAC configuration '{0}'?")]
    Confirm_DeletePAC,

    // Version Info
    [Cn("版本 {0}")]
    [En("Version {0}")]
    Version,

    [Cn("MSIX包：{0}")]
    [En("MSIX Package: {0}")]
    MSIXPackage,

    [Cn("由于Microsoft Store限制，某些功能可能受限")]
    [En("Some features may be limited due to Microsoft Store restrictions")]
    MSIXRestriction,

    [Cn("设置 - 语言")]
    [En("Settings - Language")]
    Settings_Language,

    [Cn("设置 - 快捷键")]
    [En("Settings - HotKeys")]
    Settings_HotKeys,

    [Cn("设置 - 应用程序")]
    [En("Settings - Application")]
    Settings_Application,

    [Cn("开机启动")]
    [En("Start with Windows")]
    Settings_StartWithWindows,

    [Cn("启动时最小化")]
    [En("Start minimized")]
    Settings_StartMinimized,

    [Cn("关闭时最小化到托盘")]
    [En("Minimize to tray on close")]
    Settings_MinimizeToTrayOnClose,

    [Cn("应用程序将在系统启动时自动运行")]
    [En("Application will automatically run when system starts")]
    Settings_StartWithWindows_Tooltip,

    [Cn("启动时不显示主窗口，直接最小化到托盘")]
    [En("Hide main window on startup and minimize to tray")]
    Settings_StartMinimized_Tooltip,

    [Cn("点击关闭按钮时最小化到托盘而不是退出")]
    [En("Minimize to tray instead of exit when close button is clicked")]
    Settings_MinimizeToTrayOnClose_Tooltip,

    [Cn("设置 - 代理")]
    [En("Settings - Proxy")]
    Settings_Proxy,

    [Cn("HTTP代理设置")]
    [En("HTTP Proxy Settings")]
    Settings_HttpProxy,

    [Cn("代理服务器地址")]
    [En("Proxy Server")]
    Settings_ProxyServer,

    [Cn("用户名")]
    [En("Username")]
    Settings_Username,

    [Cn("密码")]
    [En("Password")]
    Settings_Password,

    [Cn("启用HTTP代理")]
    [En("Enable HTTP Proxy")]
    Settings_EnableProxy,

    [Cn("输入代理服务器地址，格式: http://host:port")]
    [En("Enter proxy server address, format: http://host:port")]
    Settings_ProxyServer_Tooltip,

    [Cn("代理服务器需要身份验证时填写")]
    [En("Fill in when proxy server requires authentication")]
    Settings_ProxyAuth_Tooltip,

    [Cn("测试网络连接")]
    [En("Test Network")]
    Settings_TestNetwork
}
