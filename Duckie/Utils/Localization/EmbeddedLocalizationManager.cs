using Duckie.Services.UserConfigs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Duckie.Utils.Localization
{
    /// <summary>
    /// 嵌入式本地化管理器，所有语言资源都嵌入在主程序集中
    /// 避免生成单独的资源 DLL 文件
    /// </summary>
    public class EmbeddedLocalizationManager : INotifyPropertyChanged
    {
        private static EmbeddedLocalizationManager _instance;
        private static readonly object _lock = new object();

        private CultureInfo _currentCulture;
        private readonly Dictionary<string, Dictionary<string, string>> _resources;

        public static EmbeddedLocalizationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EmbeddedLocalizationManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private EmbeddedLocalizationManager()
        {
            _resources = new Dictionary<string, Dictionary<string, string>>();
            InitializeResources();

            // 设置默认文化
            _currentCulture = CultureInfo.CurrentUICulture;

            // 应用当前文化
            ApplyCulture(_currentCulture);
        }

        /// <summary>
        /// 当前文化信息
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    ApplyCulture(value);
                    OnPropertyChanged(nameof(CurrentCulture));
                    OnLanguageChanged();
                }
            }
        }

        /// <summary>
        /// 语言变更事件
        /// </summary>
        public event EventHandler LanguageChanged;

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="args">格式化参数</param>
        /// <returns>本地化字符串</returns>
        public string GetString(string key, params object[] args)
        {
            try
            {
                var cultureName = _currentCulture.Name;

                // 尝试获取当前文化的资源
                if (_resources.TryGetValue(cultureName, out var cultureResources) &&
                    cultureResources.TryGetValue(key, out var value))
                {
                    // 如果有格式化参数，进行格式化
                    if (args != null && args.Length > 0)
                    {
                        return string.Format(value, args);
                    }
                    return value;
                }

                // 如果当前文化没有找到，尝试英文
                if (_resources.TryGetValue("en-US", out var englishResources) &&
                    englishResources.TryGetValue(key, out var englishValue))
                {
                    if (args != null && args.Length > 0)
                    {
                        return string.Format(englishValue, args);
                    }
                    return englishValue;
                }

                // 如果都找不到，返回键名作为后备
                return $"[{key}]";
            }
            catch (Exception)
            {
                // 异常情况下返回键名
                return $"[{key}]";
            }
        }

        /// <summary>
        /// 切换到指定语言
        /// </summary>
        /// <param name="cultureName">文化名称，如 "en-US", "zh-CN"</param>
        public void SwitchLanguage(string cultureName)
        {
            try
            {
                var culture = new CultureInfo(cultureName);
                CurrentCulture = culture;

                // 保存语言偏好到用户配置
                UserConfigService.SetLanguage(cultureName);
            }
            catch (Exception)
            {
                // 如果文化不支持，使用默认文化
                CurrentCulture = new CultureInfo("en-US");
                UserConfigService.SetLanguage("en-US");
            }
        }

        /// <summary>
        /// 加载保存的语言偏好
        /// </summary>
        public void LoadSavedLanguage()
        {
            try
            {
                var savedLanguage = UserConfigService.GetLanguage();
                var culture = new CultureInfo(savedLanguage);
                CurrentCulture = culture;
            }
            catch (Exception)
            {
                // 如果加载失败，使用默认语言
                CurrentCulture = new CultureInfo("en-US");
            }
        }

        /// <summary>
        /// 获取支持的语言列表
        /// </summary>
        /// <returns>支持的语言文化信息数组</returns>
        public CultureInfo[] GetSupportedLanguages()
        {
            return new[]
            {
                new CultureInfo("en-US"), // 英语
                new CultureInfo("zh-CN")  // 简体中文
            };
        }

        /// <summary>
        /// 应用文化设置
        /// </summary>
        /// <param name="culture">文化信息</param>
        private void ApplyCulture(CultureInfo culture)
        {
            // 设置线程文化
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // 设置应用程序文化
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        /// <summary>
        /// 初始化嵌入的资源
        /// </summary>
        private void InitializeResources()
        {
            // 英文资源
            var englishResources = new Dictionary<string, string>
            {
                // Application Title
                ["AppTitle"] = "Duckie",
                ["AppDescription"] = "Image Processing and PAC Management Tool",
                ["AppSubtitle"] = "A lightweight open-source utility",

                // Navigation
                ["Nav_Image"] = "Image",
                ["Nav_PAC"] = "PAC",
                ["Nav_About"] = "About",
                ["Nav_Language"] = "Language",
                ["Settings"] = "Settings",

                // Window Titles
                ["Title_Image"] = "Duckie - Image",
                ["Title_PAC"] = "Duckie - PAC",
                ["Title_About"] = "Duckie - About",

                // Image Processing
                ["Btn_Open"] = "Open",
                ["Btn_ExportICO"] = "Export ICO",
                ["Tooltip_RotateLeft"] = "Rotate 90° Counter-clockwise",
                ["Tooltip_RotateRight"] = "Rotate 90° Clockwise",
                ["Tooltip_FlipHorizontal"] = "Flip Horizontal",
                ["Tooltip_FlipVertical"] = "Flip Vertical",
                ["Tooltip_ZoomIn"] = "Zoom In",
                ["Tooltip_ZoomOut"] = "Zoom Out",
                ["Tooltip_FitToWindow"] = "Fit to Window",

                // PAC Management
                ["Btn_AddPAC"] = "Add PAC",
                ["Btn_Edit"] = "Edit",
                ["Btn_Delete"] = "Delete",
                ["Label_NoPAC"] = "No PAC",
                ["Label_DirectConnection"] = "Direct Connection",

                // Dialog Titles
                ["Dialog_AddPAC"] = "Add PAC Configuration",
                ["Dialog_EditPAC"] = "Edit PAC Configuration",
                ["Dialog_LanguageSettings"] = "Language Settings",
                ["Dialog_ConfirmDelete"] = "Confirm Delete",

                // Form Labels
                ["Label_ConfigName"] = "Configuration Name:",
                ["Label_PACURL"] = "PAC URL:",
                ["Label_SupportedFormats"] = "Supported formats:",
                ["Label_NetworkURL"] = "• Network URL: http://example.com/proxy.pac",
                ["Label_LocalFile"] = "• Local file: file:///C:/proxy.pac",
                ["Label_SelectLanguage"] = "Select Language:",
                ["Label_LanguageNote"] = "Note: Language changes will take effect immediately.",
                ["Label_LanguageRestart"] = "Some UI elements may require application restart to fully update.",

                // Status Messages
                ["Status_Ready"] = "Ready",
                ["Status_Loaded"] = "Loaded: {0}",
                ["Status_PACAdded"] = "PAC configuration '{0}' added successfully",
                ["Status_PACUpdated"] = "PAC configuration '{0}' updated successfully",
                ["Status_PACDeleted"] = "PAC configuration '{0}' deleted successfully",
                ["Status_PACCleared"] = "PAC cleared",
                ["Status_PACSwitch"] = "Switched to PAC: {0}",
                ["Status_PACFailed"] = "PAC operation failed: {0}",

                // Error Messages
                ["Error_Warning"] = "Warning",
                ["Error_Error"] = "Error",
                ["Error_Exception"] = "Exception: {0}",
                ["Error_FailedToOpenImage"] = "Failed to open image: {0}",
                ["Error_PleaseOpenImage"] = "Please open an image",
                ["Error_SaveImage"] = "Save Image",
                ["Error_FailedToAddPAC"] = "Failed to add PAC. Name or URL may already exist.",
                ["Error_FailedToUpdatePAC"] = "Failed to update PAC configuration",
                ["Error_FailedToDeletePAC"] = "Failed to delete PAC configuration",
                ["Error_FailedToLoadPAC"] = "Failed to load PAC configurations",
                ["Error_FailedToRotateImage"] = "Failed to rotate image",
                ["Error_FailedToFlipImage"] = "Failed to flip image",

                // Success Messages
                ["Success_IconExported"] = "The icon has been exported: {0}",

                // Common Buttons
                ["Btn_OK"] = "OK",
                ["Btn_Cancel"] = "Cancel",
                ["Btn_Yes"] = "Yes",
                ["Btn_No"] = "No",

                // Confirmation Messages
                ["Confirm_DeletePAC"] = "Are you sure you want to delete PAC configuration '{0}'?",

                // Version Info
                ["Version"] = $"Version {AppUtils.GetAppVersion()}",
                ["MSIXPackage"] = "MSIX Package: {0}",
                ["MSIXRestriction"] = "Some features may be limited due to Microsoft Store restrictions"
            };

            _resources["en-US"] = englishResources;

            // 中文资源
            var chineseResources = new Dictionary<string, string>
            {
                // Application Title
                ["AppTitle"] = "Duckie",
                ["AppDescription"] = "图像处理与PAC管理工具",
                ["AppSubtitle"] = "轻量级开源实用工具",

                // Navigation
                ["Nav_Image"] = "图像",
                ["Nav_PAC"] = "代理",
                ["Nav_About"] = "关于",
                ["Nav_Language"] = "语言",
                ["Settings"] = "设置",

                // Window Titles
                ["Title_Image"] = "Duckie - 图像处理",
                ["Title_PAC"] = "Duckie - 代理管理",
                ["Title_About"] = "Duckie - 关于",

                // Image Processing
                ["Btn_Open"] = "打开",
                ["Btn_ExportICO"] = "导出图标",
                ["Tooltip_RotateLeft"] = "逆时针旋转90°",
                ["Tooltip_RotateRight"] = "顺时针旋转90°",
                ["Tooltip_FlipHorizontal"] = "水平翻转",
                ["Tooltip_FlipVertical"] = "垂直翻转",
                ["Tooltip_ZoomIn"] = "放大",
                ["Tooltip_ZoomOut"] = "缩小",
                ["Tooltip_FitToWindow"] = "适应窗口",

                // PAC Management
                ["Btn_AddPAC"] = "添加代理",
                ["Btn_Edit"] = "编辑",
                ["Btn_Delete"] = "删除",
                ["Label_NoPAC"] = "无代理",
                ["Label_DirectConnection"] = "直接连接",

                // Dialog Titles
                ["Dialog_AddPAC"] = "添加PAC配置",
                ["Dialog_EditPAC"] = "编辑PAC配置",
                ["Dialog_LanguageSettings"] = "语言设置",
                ["Dialog_ConfirmDelete"] = "确认删除",

                // Form Labels
                ["Label_ConfigName"] = "配置名称：",
                ["Label_PACURL"] = "PAC地址：",
                ["Label_SupportedFormats"] = "支持的格式：",
                ["Label_NetworkURL"] = "• 网络地址：http://example.com/proxy.pac",
                ["Label_LocalFile"] = "• 本地文件：file:///C:/proxy.pac",
                ["Label_SelectLanguage"] = "选择语言：",
                ["Label_LanguageNote"] = "注意：语言更改将立即生效。",
                ["Label_LanguageRestart"] = "某些界面元素可能需要重启应用程序才能完全更新。",

                // Status Messages
                ["Status_Ready"] = "就绪",
                ["Status_Loaded"] = "已加载：{0}",
                ["Status_PACAdded"] = "PAC配置 '{0}' 添加成功",
                ["Status_PACUpdated"] = "PAC配置 '{0}' 更新成功",
                ["Status_PACDeleted"] = "PAC配置 '{0}' 删除成功",
                ["Status_PACCleared"] = "已清除代理",
                ["Status_PACSwitch"] = "已切换到代理：{0}",
                ["Status_PACFailed"] = "代理操作失败：{0}",

                // Error Messages
                ["Error_Warning"] = "警告",
                ["Error_Error"] = "错误",
                ["Error_Exception"] = "异常：{0}",
                ["Error_FailedToOpenImage"] = "打开图像失败：{0}",
                ["Error_PleaseOpenImage"] = "请先打开图像",
                ["Error_SaveImage"] = "保存图像",
                ["Error_FailedToAddPAC"] = "添加PAC失败。名称或URL可能已存在。",
                ["Error_FailedToUpdatePAC"] = "更新PAC配置失败",
                ["Error_FailedToDeletePAC"] = "删除PAC配置失败",
                ["Error_FailedToLoadPAC"] = "加载PAC配置失败",
                ["Error_FailedToRotateImage"] = "旋转图像失败",
                ["Error_FailedToFlipImage"] = "翻转图像失败",

                // Success Messages
                ["Success_IconExported"] = "图标已导出：{0}",

                // Common Buttons
                ["Btn_OK"] = "确定",
                ["Btn_Cancel"] = "取消",
                ["Btn_Yes"] = "是",
                ["Btn_No"] = "否",

                // Confirmation Messages
                ["Confirm_DeletePAC"] = "确定要删除PAC配置 '{0}' 吗？",

                // Version Info
                ["Version"] = $"版本 {AppUtils.GetAppVersion()}",
                ["MSIXPackage"] = "MSIX包：{0}",
                ["MSIXRestriction"] = "由于Microsoft Store限制，某些功能可能受限"
            };

            _resources["zh-CN"] = chineseResources;
        }

        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 触发语言变更事件
        /// </summary>
        protected virtual void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 本地化扩展方法
    /// </summary>
    public static class EmbeddedLocalizationExtensions
    {
        /// <summary>
        /// 获取本地化字符串的扩展方法
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="args">格式化参数</param>
        /// <returns>本地化字符串</returns>
        public static string Localize(this string key, params object[] args)
        {
            return LocUtils.GetString(key, args);
        }
    }
}
