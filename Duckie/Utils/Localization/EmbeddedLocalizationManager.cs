using Duckie.Services.UserConfigs;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Duckie.Utils.Localization;

/// <summary>
/// 嵌入式本地化管理器，所有语言资源都嵌入在主程序集中
/// 避免生成单独的资源 DLL 文件
/// </summary>
public class EmbeddedLocalizationManager : INotifyPropertyChanged
{
    private static EmbeddedLocalizationManager _instance;
    private static readonly object _lock = new object();

    private CultureInfo _currentCulture;
    private readonly Dictionary<string, Dictionary<LocKey, string>> _resources;

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
        _resources = new Dictionary<string, Dictionary<LocKey, string>>();
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
    public string GetString(LocKey key, params object[] args)
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
        


        var fields = typeof(LocKey).GetFields().Where(x => x.FieldType == typeof(LocKey));
        foreach (var field in fields)
        {
            var locKey = (LocKey)field.GetValue(null);

            foreach (var attribute in field.GetCustomAttributes())
            {
                if (attribute is ILocTextProvider locTextProvider)
                {
                    if (!_resources.TryGetValue(locTextProvider.CultureName, out var values))
                    {
                        _resources[locTextProvider.CultureName] = new Dictionary<LocKey, string>();
                    }

                    _resources[locTextProvider.CultureName][locKey] = locTextProvider.Text;
                }
            }
        }
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
    public static string Localize(this LocKey key, params object[] args)
    {
        return LocUtils.GetString(key, args);
    }
}
