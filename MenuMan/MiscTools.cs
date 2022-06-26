using System.Collections.Generic;
using System.Text;
using System;

namespace MenuMan
{
    internal static class MiscTools
    {
        /// <summary>
        /// Repeats a string a given amount of times and returns the result.
        /// </summary>
        /// <param name="val">The string to repeat.</param>
        /// <param name="count">The amount of times to repeat the input string.</param>
        /// <returns>The input string repeated the specified amount of times.</returns>
        internal static string Repeat(this string val, int count)
        {
            if (count <= 0) return "";
            else if (count == 1) return val;

            StringBuilder sb = new StringBuilder(val.Length * count);
            for (int i = 0; i < count; ++i) sb.Append(val);

            return sb.ToString();
        }

        /// <summary>
        /// The default question condition which always returns true.
        /// </summary>
        internal static readonly Func<Dictionary<string, object>, bool> DefaultCondition = (Dictionary<string, object> _) => true;
    }

    public static class Helpers
    {
        /// <summary>
        /// Attempts to retrieve the value at the specified key in the answers dictionary, casting the result to T if successful and returning the default value for T if unsuccessful.
        /// If the cast is unsuccessful, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The type to which to cast the result.</typeparam>
        /// <param name="dictionary">The input dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value stored at the key in the dictionary (cast to type T) or the default value for T if no value exists.</returns>
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
