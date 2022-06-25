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

        internal static void WriteWholeLine(string stuff = "", bool withNewline = true, bool hasAnsi = true, string backColor = "#000000")
        {
            int rawStringLength = hasAnsi ? AnsiRegex.Replace(stuff, "").Length : stuff.Length;
            Console.Write($"{stuff}{" ".Repeat(Console.WindowWidth - rawStringLength - Console.CursorLeft - 1)}{(withNewline ? Environment.NewLine : "")}".PastelBg(backColor));
        }

        internal static void WriteWholeLine(bool withNewLine) => WriteWholeLine("", withNewLine);

        internal static string ReadStringWithColor(string color, bool allowEmptyInput, string defaultValue = "")
        {
            int stringStart = Console.CursorLeft;
            string runningString = "";
            while (true)
            {
                Console.CursorLeft = stringStart;

                if (defaultValue != "" && runningString == "") WriteWholeLine($"({defaultValue})".Pastel(Constants.INFO_TEXT), false);
                else WriteWholeLine(runningString.Pastel(color), false);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if ((allowEmptyInput && runningString == "") || runningString != "" || defaultValue != "")
                    {
                        Console.CursorLeft = stringStart;
                        string returnValue = runningString != "" ? runningString : defaultValue;
                        WriteWholeLine(returnValue.Pastel(Constants.ACTIVE_TEXT_COLOR));
                        return returnValue;
                    }
                    else
                    {
                        PrintError("Empty input is not allowed");
                    }
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (runningString.Length > 0) runningString = runningString.Substring(0, runningString.Length - 1);
                    
                    if (runningString.Length == 0 && defaultValue == "") PrintError("Empty input is not allowed");
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

        internal static void PrintError(string message)
        {
            int oldCursorPosition = Console.CursorLeft;
            ++Console.CursorTop;
            Console.CursorLeft = 0;
            WriteWholeLine($"{"»".Pastel(Constants.ERROR_TEXT)} {message}", false);
            --Console.CursorTop;
            Console.CursorLeft = oldCursorPosition;
        }

        internal static void ClearError()
        {
            int oldCursorPosition = Console.CursorLeft;
            ++Console.CursorTop;
            Console.CursorLeft = 0;
            WriteWholeLine(withNewline: false, hasAnsi: false);
            --Console.CursorTop;
            Console.CursorLeft = oldCursorPosition;
        }
    }
}
