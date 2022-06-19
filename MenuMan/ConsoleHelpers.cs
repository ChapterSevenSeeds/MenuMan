using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MenuMan
{
    internal static class ConsoleHelpers
    {
        internal static CompiledRegexes.ANSIRegex AnsiRegex = new CompiledRegexes.ANSIRegex();
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

        internal static void WriteWholeLine(string stuff = "", bool withNewline = true, bool hasAnsi = true)
        {
            int rawStringLength = hasAnsi ? AnsiRegex.Replace(stuff, "").Length : stuff.Length;
            Console.Write($"{stuff}{" ".Repeat(Console.WindowWidth - rawStringLength - Console.CursorLeft - 1)}{(withNewline ? Environment.NewLine : "")}");
        }

        internal static void WriteWholeLine(bool withNewLine) => WriteWholeLine("", withNewLine);

        internal static string ReadStringWithColor(string color, bool allowEmptyInput, string initialValue = "")
        {
            int stringStart = Console.CursorLeft;
            string runningString = initialValue;
            while (true)
            {
                Console.CursorLeft = stringStart;
                Console.Write($"{runningString.Pastel(color)}{" ".Repeat(runningString.Length + 1)}");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if ((allowEmptyInput && runningString == "") || runningString != "")
                    {
                        Console.WriteLine();
                        return runningString;
                    }
                    else
                    {
                        PrintError("Empty input is not allowed");
                    }
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (runningString.Length > 0) runningString = runningString.Substring(0, runningString.Length - 1);
                    
                    if (runningString.Length == 0) PrintError("Empty input is not allowed");
                }
                else
                {
                    runningString += key.KeyChar;
                    ClearError();
                }
            }
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
