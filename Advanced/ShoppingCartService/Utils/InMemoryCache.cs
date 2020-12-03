using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShoppingCartService.Utils
{
    public class InMemoryCache : IMemoryCache
    {
        private readonly ConcurrentDictionary<object, object> _data = new ConcurrentDictionary<object, object>();

        #region ICacheEntry Implementation
        public class DataEntry : ICacheEntry
        {
            private readonly Action<object> setValueCallback;
            private object value;

            public DataEntry(Action<object> setValueCallback)
            {
                this.setValueCallback = setValueCallback;
            }

            public DateTimeOffset? AbsoluteExpiration { get; set; }
            public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

            public IList<IChangeToken> ExpirationTokens => new List<IChangeToken>();

            public object Key { get; set; }

            public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();

            public CacheItemPriority Priority { get; set; }
            public long? Size { get; set; }
            public TimeSpan? SlidingExpiration { get; set; }
            
            public object Value
            {
                get { return value; }
                set 
                { 
                    this.value = value;
                    setValueCallback(value);
                }
            }

            public void Dispose()
            {
            }
        }
        #endregion

        public ICacheEntry CreateEntry(object key) => 
            new DataEntry(val => _data.TryAdd(key, val))
            {
                Key = key
            };

        public void Dispose()
        {
        }

        public void Remove(object key)
        {
            //TODO: do not forget to implement this! (and of course, I forgot :))
        }

        public bool TryGetValue(object key, out object value) => _data.TryGetValue(key, out value);
    }
}
