namespace Duckie.Shared.Services.UserConfigs;

[Serializable]
public class UserConfig
{
    public ProxyConfig Proxy { get; set; } = new ProxyConfig();
    public PacConfig[] Pacs { get; set; } = [];
    public string Language { get; set; } = "en-US";
    public AppSettings App { get; set; } = new AppSettings();
}

[Serializable]
public class AppSettings
{
    /// <summary>
    /// 启动时最小化到托盘
    /// </summary>
    public bool StartMinimized { get; set; } = false;
    
    /// <summary>
    /// 关闭时最小化到托盘
    /// </summary>
    public bool MinimizeToTrayOnClose { get; set; } = false;
}
