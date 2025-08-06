namespace Duckie.Services.UserConfigs;

[Serializable]
public class UserConfig
{
    public ProxyConfig Proxy { get; set; } = new ProxyConfig();
    public PacConfig[] Pacs { get; set; } = new PacConfig[0];
    public string Language { get; set; } = "en-US";
}
