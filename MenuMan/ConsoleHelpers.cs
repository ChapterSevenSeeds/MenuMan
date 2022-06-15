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

        internal static bool RunPredicateValidation<T>(Func<T, Dictionary<string, object>, bool> predicate, string message, T value, Dictionary<string, object> answers)
        {
            bool isValid = predicate(value, answers);
            if (isValid) PrintError(message);
            else ClearError();

            return isValid;
        }

        internal static bool RunCustomMessageValidation<T>(Func<T, Dictionary<string, object>, string> predicate, T value, Dictionary<string, object> answers)
        {
            string message = predicate(value, answers).Trim();
            if (message != "") PrintError(message);
            else ClearError();

            return message == "";
        }

        private static int _previousMessageLength = 0;
        internal static void PrintError(string message)
        {
            int oldCursorPosition = Console.CursorLeft;
            ++Console.CursorTop;
            Console.CursorLeft = 0;
            Console.Write($"{"»".Pastel(Constants.ERROR_TEXT)} {message}");
            --Console.CursorTop;
            Console.CursorLeft = oldCursorPosition;

            _previousMessageLength = message.Length;
        }

        internal static void ClearError()
        {
            int oldCursorPosition = Console.CursorLeft;
            ++Console.CursorTop;
            Console.CursorLeft = 0;
            Console.Write(" ".Repeat(_previousMessageLength + 2));
            --Console.CursorTop;
            Console.CursorLeft = oldCursorPosition;
        }
    }
}
