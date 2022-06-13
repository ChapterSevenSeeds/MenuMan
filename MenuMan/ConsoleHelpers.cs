using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan
{
    internal static class ConsoleHelpers
    {
        internal static ConsoleKeyInfo ReadAllButKeys(params ConsoleKey[] keysToSuppress)
        {
            ConsoleKeyInfo key;
            do
            {
                key = ReadAnyKey();
            } while (keysToSuppress.Contains(key.Key));

            return key;
        }

        internal static ConsoleKeyInfo ReadCertainKeys(params ConsoleKey[] keysToAllow)
        {
            ConsoleKeyInfo key;
            do
            {
                key = ReadAnyKey();
            } while (!keysToAllow.Contains(key.Key));

            return key;
        }

        internal static ConsoleKeyInfo ReadAnyKey()
        {
            return Console.ReadKey(true);
        }

        internal static string ReadStringWithColor(string color)
        {
            var colorAnsi = "|".Pastel(color).Split('|')[0];
            Console.Write(colorAnsi);
            return Console.ReadLine();
        }
    }
}
