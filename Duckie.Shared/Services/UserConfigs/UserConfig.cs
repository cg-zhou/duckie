namespace Duckie.Shared.Services.UserConfigs;

[Serializable]
public class UserConfig
{
    public ProxyConfig Proxy { get; set; } = new ProxyConfig();
    public PacConfig[] Pacs { get; set; } = [];
    public string Language { get; set; } = "en-US";
}
