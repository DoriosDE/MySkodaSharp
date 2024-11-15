using YamlDotNet.Serialization;

namespace MySkodaSharp.Api.Auth.Models
{
    internal class TemplateModel
    {
        [YamlMember(Alias = "relayState")]
        public string RelayState { get; set; }

        [YamlMember(Alias = "hmac")]
        public string Hmac { get; set; }
    }
}
