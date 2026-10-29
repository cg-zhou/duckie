using Duckie.Shared.Utils;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Duckie.Shared.Services.UserConfigs;

public class UserConfigService
{
    private static string Path => System.IO.Path.Combine(AppUtils.AppDataFolder, "user_config.xml");

    private static readonly object Sync = new();

    private static UserConfig TryRead(string path)
    {
        try
        {
            using var reader = new StreamReader(path);
            return (UserConfig)new XmlSerializer(typeof(UserConfig)).Deserialize(reader);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (DirectoryNotFoundException)
        {
            return null;
        }
        catch (InvalidOperationException ex)
        {
            System.Diagnostics.Trace.TraceWarning($"Invalid configuration {path}: {ex}");
            return null;
        }
    }

    public static UserConfig Get()
    {
        lock (Sync)
        {
            return TryRead(Path) ?? TryRead(Path + ".bak") ?? new UserConfig();
        }
    }

    public static void Set(UserConfig userConfig)
    {
        lock (Sync)
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
            var temporaryPath = Path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true,
                    CloseOutput = false
                };
                using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write))
                {
                    using (var writer = XmlWriter.Create(stream, settings))
                    {
                        var namespaces = new XmlSerializerNamespaces();
                        namespaces.Add(string.Empty, string.Empty);
                        new XmlSerializer(typeof(UserConfig)).Serialize(writer, userConfig, namespaces);
                    }
                    stream.Flush(true);
                }

                if (File.Exists(Path))
                {
                    // Do not overwrite a valid backup with a damaged configuration.
                    File.Replace(temporaryPath, Path, TryRead(Path) != null ? Path + ".bak" : null);
                }
                else
                {
                    File.Move(temporaryPath, Path);
                }
            }
            finally
            {
                if (File.Exists(temporaryPath))
                {
                    File.Delete(temporaryPath);
                }
            }
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

    public static void SetAppSettings(AppSettings appSettings)
    {
        var config = Get();
        config.App = appSettings;
        Set(config);
    }

    public static AppSettings GetAppSettings()
    {
        var config = Get();
        return config.App ?? new AppSettings();
    }
}
