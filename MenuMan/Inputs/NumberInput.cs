using Pastel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MenuMan.Inputs
{
    internal class NumberInput<T> : IQuestion where T : struct
    {
        private static readonly HashSet<Type> TypeMap = new HashSet<Type>
        {
            typeof(sbyte),
            typeof(byte),
            typeof(ushort),
            typeof(short),
            typeof(uint),
            typeof(int),
            typeof(ulong),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        private static readonly HashSet<char> NumericChars = new HashSet<char>("0123456789.".ToCharArray());
        public Type ReturnType => typeof(T);
        public string Key { get; }
        public string QuestionText { get; }

        private string _defaultValue;
        private MethodInfo _parseMethod;

        internal NumberInput(string key, string questionText, object defaultValue)
        {
            if (!TypeMap.Contains(typeof(T))) throw new ArgumentException("The type parameter for the NumberInput must be a numeric type.");

            Key = key;
            QuestionText = questionText;

            _defaultValue = defaultValue?.ToString() ?? "";
        }

        public object Ask()
        {
            int stringStart = Console.CursorLeft;
            string runningString = _defaultValue;
            _parseMethod = ReturnType.GetMethod("TryParse", new Type[] { typeof(string), ReturnType.MakeByRefType() });
            object parsedValue = null;

            if (runningString != "") parsedValue = TryParse(runningString);
            while (true)
            {
                Console.CursorLeft = stringStart;
                Console.Write(runningString.Pastel(Constants.ACTIVE_TEXT_COLOR));
                Console.Write(" ");

                ConsoleKeyInfo keyInfo = ConsoleHelpers.ReadAnyKey();

                if (!NumericChars.Contains(keyInfo.KeyChar) && keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter) continue;

                if (keyInfo.Key == ConsoleKey.Backspace && runningString.Length > 0) runningString = runningString.Substring(0, runningString.Length - 1);
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (parsedValue != null)
                    {
                        Console.Write(Environment.NewLine);
                        return parsedValue;
                    }
                }
                else if (keyInfo.Key != ConsoleKey.Backspace) runningString += keyInfo.KeyChar;

                parsedValue = TryParse(runningString);
            }
        }

        private object TryParse(string runningString)
        {
            object[] parameters = new object[] { runningString, null };
            bool parseSuccess = (bool)_parseMethod.Invoke(null, parameters);
            object parsedValue = parseSuccess ? parameters[1] : null;
            Console.CursorLeft = 0;
            ++Console.CursorTop;
            Console.Write(parseSuccess ? " ".Repeat(30) : $"{"»".Pastel(Constants.ERROR_TEXT)} Please enter a valid {ReturnType.Name}");
            --Console.CursorTop;

            return parsedValue;
        }
    }
}