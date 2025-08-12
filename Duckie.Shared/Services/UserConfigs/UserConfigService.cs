using Duckie.Shared.Utils;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Duckie.Shared.Services.UserConfigs;

public class UserConfigService
{
    private static string Path => System.IO.Path.Combine(AppUtils.AppDataFolder, "user_config.xml");

    public static UserConfig Get()
    {
        if (!File.Exists(Path))
        {
            return new UserConfig();
        }

        var serializer = new XmlSerializer(typeof(UserConfig));
        using (var reader = new StreamReader(Path))
        {
            return (UserConfig)serializer.Deserialize(reader);
        }
    }

    public static void Set(UserConfig userConfig)
    {
        var serializer = new XmlSerializer(typeof(UserConfig));

        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            Indent = true
        };

        Directory.CreateDirectory(Directory.GetParent(Path).FullName);

        using (var xmlWriter = XmlWriter.Create(Path, settings))
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(xmlWriter, userConfig, namespaces);
        }
    }

    public static void Set(ProxyConfig proxy)
    {
        var config = Get();
        config.Proxy = proxy;
        Set(config);
    }

    public static void Set(PacConfig[] pacs)
    {
        var config = Get();
        config.Pacs = pacs;
        Set(config);
    }

    public static void SetLanguage(string language)
    {
        var config = Get();
        config.Language = language;
        Set(config);
    }

    public static string GetLanguage()
    {
        var config = Get();
        return config.Language ?? "en-US";
    }
}
