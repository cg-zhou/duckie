using System.Xml.Serialization;

namespace Duckie.Services.UserConfigs
{
    public class ProxyConfig
    {
        [XmlAttribute]
        public string Username { get; set; }

        [XmlAttribute]
        public string Password { get; set; }

        [XmlAttribute]
        public string ProxyUri { get; set; }
    }
}
