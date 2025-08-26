using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Ui;
using System.Net;
using System.Net.Http;

namespace Duckie.Services.Clipboard;

public static class HttpUtils
{
    private static ProxyConfig config = new ProxyConfig();

    public static void SetProxy(ProxyConfig config)
    {
        HttpUtils.config = config;
    }

    public static async Task TestNetworkAsync()
    {
        var uri = "https://baidu.com";
        try
        {
            var response = await GetAsync(uri);
            if (response.Length > 0)
            {
                NotifyIconUtils.Notify("Access network successfully.", "Access Network Test");
            }
        }
        catch (Exception e)
        {
            NotifyIconUtils.Notify(
                $"Failed to access {uri}, proxy=[{config.ProxyUri}], usename=[{config.Username}] : {e.Message}",
                "Access Network Test");
        }
    }

    public static async Task<string> GetAsync(string url)
    {
        using (var client = CreateClient())
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}");
            }

            var text = await response.Content.ReadAsStringAsync();
            return text;
        }
    }

    public static async Task<string> PostAsync(string url, MultipartFormDataContent formData)
    {
        using (var client = CreateClient())
        {
            var response = await client.PostAsync(url, formData);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}");
            }

            var text = await response.Content.ReadAsStringAsync();
            return text;
        }
    }

    private static HttpClient CreateClient()
    {
        if (string.IsNullOrWhiteSpace(config.ProxyUri))
        {
            return new HttpClient();
        }
        else
        {
            var webProxy = new WebProxy
            {
                Address = new Uri(config.ProxyUri),
                Credentials = new NetworkCredential(config.Username, config.Password)
            };

            var handler = new HttpClientHandler
            {
                Proxy = webProxy
            };
            return new HttpClient(handler);
        }
    }
}
