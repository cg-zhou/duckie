using System.Xml.Serialization;

namespace Duckie.Shared.Services.UserConfigs;

public class PacConfig
{
    [XmlAttribute]
    public string Name { get; set; }

    [XmlAttribute]
    public string Uri { get; set; }
}
