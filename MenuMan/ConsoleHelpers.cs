using Pastel;
using System;
using System.Linq;

namespace MenuMan
{
    internal static class ConsoleHelpers
    {
        /// <summary>
        /// Reads only specified keyboard strokes.
        /// </summary>
        /// <param name="keysToAllow">The keyboard keys to allow.</param>
        /// <returns>The key that was pressed.</returns>
        internal static ConsoleKeyInfo ReadCertainKeys(params ConsoleKey[] keysToAllow)
        {
            ConsoleKeyInfo key;
            do
            {
                key = ReadAnyKey();
            } while (!keysToAllow.Contains(key.Key));

            return key;
        }

        /// <summary>
        /// Alias for Console.ReadKey(true);
        /// </summary>
        /// <returns>The key that was pressed.</returns>
        internal static ConsoleKeyInfo ReadAnyKey()
        {
            return Console.ReadKey(true);
        }

        /// <summary>
        /// Writes a string to the terminal and fills the rest of the terminal width with empty spaces.
        /// </summary>
        /// <param name="stuff">The input string.</param>
        /// <param name="withNewLine">If true, a newline will succeed the input string.</param>
        /// <param name="backColor">Optional background color to print for the entire line.</param>
        internal static void WriteWholeLine(string stuff = "", bool withNewLine = true, string backColor = "#000000")
        {
            Console.Write($"{stuff}{" ".Repeat(Console.WindowWidth - stuff.Length - Console.CursorLeft - 1)}{(withNewLine ? Environment.NewLine : "")}".PastelBg(backColor));
        }

        /// <summary>
        /// Writes a DelayedPastel string to the terminal and fills the rest of the terminal width with empty space.
        /// </summary>
        /// <param name="stuff">The DelayedPastel instance.</param>
        /// <param name="withNewLine">If true, a newline will succeed the input string.</param>
        /// <param name="backColor">Optional background color to print for the entire line.</param>
        internal static void WriteWholeLine(DelayedPastel stuff, bool withNewLine = true, string backColor = "#000000")
        {
            Console.Write($"{stuff}{" ".Repeat(Console.WindowWidth - stuff.Length - Console.CursorLeft - 1)}{(withNewLine ? Environment.NewLine : "")}".PastelBg(backColor));
        }

        /// <summary>
        /// Reads a string from the terminal (similar to reading from stdin but with more control).
        /// </summary>
        /// <param name="color">Color of the string to print to the terminal while the user is typing.</param>
        /// <param name="allowEmptyInput">If false, pressing enter with an empty string will print an error and not return.</param>
        /// <param name="defaultValue">The default string value to hint to the user.</param>
        /// <returns></returns>
        internal static string ReadStringWithColor(string color, bool allowEmptyInput, string defaultValue = "")
        {
            int stringStart = Console.CursorLeft;
            string runningString = "";
            while (true)
            {
                Console.CursorLeft = stringStart;

                if (defaultValue != "" && runningString == "") WriteWholeLine($"({defaultValue})".DPastel(Constants.INFO_TEXT), false);
                else WriteWholeLine(runningString.DPastel(color), false);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if ((allowEmptyInput && runningString == "") || runningString != "" || defaultValue != "")
                    {
                        Console.CursorLeft = stringStart;
                        string returnValue = runningString != "" ? runningString : defaultValue;
                        WriteWholeLine(returnValue.DPastel(Constants.ACTIVE_TEXT_COLOR));
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

        /// <summary>
        /// Prints an error to the terminal on the next line.
        /// </summary>
        /// <param name="message">The error message.</param>
        internal static void PrintError(string message)
        {
            int oldCursorPosition = Console.CursorLeft;
            ++Console.CursorTop;
            Console.CursorLeft = 0;
            WriteWholeLine($"{"»".DPastel(Constants.ERROR_TEXT)} {message}", false);
            --Console.CursorTop;
            Console.CursorLeft = oldCursorPosition;
        }

        /// <summary>
        /// Clears the next line of the terminal.
        /// </summary>
        internal static void ClearError()
        {
            int oldCursorPosition = Console.CursorLeft;
            ++Console.CursorTop;
            Console.CursorLeft = 0;
            WriteWholeLine(withNewLine: false);
            --Console.CursorTop;
            Console.CursorLeft = oldCursorPosition;
        }

        public static DelayedPastel DPastel(this string input) => new DelayedPastel(input);
        public static DelayedPastel DPastel(this string input, string color) => new DelayedPastel(input, color, true);
        public static DelayedPastel DPastelBg(this string input, string color) => new DelayedPastel(input, color, false);
    }
}
