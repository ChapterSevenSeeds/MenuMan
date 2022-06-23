using System.Collections.Generic;
using System.Text;
using System;

namespace MenuMan
{
    internal static class MiscTools
    {
        internal static string Repeat(this string val, int count)
        {
            if (count == 0) return "";
            else if (count == 1) return val;

            StringBuilder sb = new StringBuilder(val.Length * count);
            for (int i = 0; i < count; ++i) sb.Append(val);

            return sb.ToString();
        }

        internal static readonly Func<Dictionary<string, object>, bool> DefaultCondition = (Dictionary<string, object> _) => true;
    }

    public static class Helpers
    {
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            else
            {
                return default;
            }
        }
    }
}
