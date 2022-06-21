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
        public Func<Dictionary<string, object>, bool> Condition { get; }

        private readonly T? defaultValue;
        private MethodInfo parseMethod;

        internal NumberInput(string key, string questionText, T? defaultValue, Func<Dictionary<string, object>, bool> condition)
        {
            if (!TypeMap.Contains(typeof(T))) throw new ArgumentException("The type parameter for the NumberInput must be a numeric type.");

            Key = key;
            QuestionText = questionText;

            this.defaultValue = defaultValue;

            Condition = condition ?? MiscTools.DefaultCondition;
        }

        public object Ask()
        {
            int stringStart = Console.CursorLeft;
            string runningString = "";
            parseMethod = ReturnType.GetMethod("TryParse", new Type[] { typeof(string), ReturnType.MakeByRefType() });
            object parsedValue = null;

            while (true)
            {
                Console.CursorLeft = stringStart;
                if (runningString == "" && defaultValue.HasValue) ConsoleHelpers.WriteWholeLine($"({defaultValue.Value})".Pastel(Constants.INFO_TEXT), false);
                else ConsoleHelpers.WriteWholeLine(runningString.Pastel(Constants.ACTIVE_TEXT_COLOR), false);

                ConsoleKeyInfo keyInfo = ConsoleHelpers.ReadAnyKey();

                if (!NumericChars.Contains(keyInfo.KeyChar) && keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter) continue;

                if (keyInfo.Key == ConsoleKey.Backspace && runningString.Length > 0) runningString = runningString.Substring(0, runningString.Length - 1);
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (parsedValue != null || (runningString == "" && defaultValue.HasValue))
                    {
                        Console.CursorLeft = stringStart;
                        object returnValue = parsedValue ?? defaultValue.Value;
                        ConsoleHelpers.WriteWholeLine(returnValue.ToString().Pastel(Constants.ACTIVE_TEXT_COLOR));
                        return returnValue;
                    }
                }
                else if (keyInfo.Key != ConsoleKey.Backspace) runningString += keyInfo.KeyChar;

                parsedValue = TryParse(runningString);
            }
        }

        private object TryParse(string runningString)
        {
            object[] parameters = new object[] { runningString, null };
            bool parseSuccess = (bool)parseMethod.Invoke(null, parameters);
            object parsedValue = parseSuccess ? parameters[1] : null;
            if (parseSuccess) ConsoleHelpers.ClearError();
            else if (runningString != "" || !defaultValue.HasValue) ConsoleHelpers.PrintError($"Please enter a valid {ReturnType.Name}");

            return parsedValue;
        }
    }
}