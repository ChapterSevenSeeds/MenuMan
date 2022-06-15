﻿using System.Text;

namespace MenuMan
{
    internal static class MiscTools
    {
        public static string Repeat(this string val, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; ++i) sb.Append(val);

            return sb.ToString();
        }
    }
}