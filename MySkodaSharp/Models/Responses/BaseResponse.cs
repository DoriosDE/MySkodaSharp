using System;

namespace MySkodaSharp.Models.Responses
{
    public class BaseResponse
    {
        public DateTime LastChanged { get; private set; }

        public BaseResponse()
        {
            LastChanged = DateTime.UtcNow;
        }
    }
}
