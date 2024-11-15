using YamlDotNet.Serialization;

namespace MySkodaSharp.Api.Auth.Models
{
    internal class CsrfState
    {
        [YamlMember(Alias = "csrf_token")]
        public string Csrf { get; set; }

        [YamlMember(Alias = "templateModel")]
        public TemplateModel TemplateModel { get; set; }
    }
}
