using System;

namespace MySkodaSharp.Models
{
    public class VagToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IdToken { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsExpired => DateTime.Now > Expiry.AddMinutes(-3);

        public VagToken()
        {
            Expiry = DateTime.Now.AddMinutes(60); // 60 minutes expiry
        }
    }
}
