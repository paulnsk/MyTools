using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{
    public static class CollectionUtils
    {

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> createNew)
        {
            if (!dict.TryGetValue(key, out var val))
            {
                val = createNew();
                dict.Add(key, val);
            }

            return val;
        }
    }
}
