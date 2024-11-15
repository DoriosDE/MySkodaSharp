using System;

namespace MySkodaSharp.Models
{
    public class Cache<T>
    {
        public DateTimeOffset CacheTime { get; set; }
        public T Result { get; set; }

        public Cache(DateTimeOffset cacheTime, T result)
        {
            CacheTime = cacheTime;
            Result = result;
        }
    }
}
