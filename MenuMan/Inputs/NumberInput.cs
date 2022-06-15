using Pastel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MenuMan.Inputs
{
    internal class NumberInput : IQuestion
    {
        private static readonly Dictionary<NumberInputType, Type> TypeMap = new Dictionary<NumberInputType, Type>
        {
            { NumberInputType.UInt8, typeof(sbyte) },
            { NumberInputType.Int8, typeof(byte) },
            { NumberInputType.UInt16, typeof(ushort) },
            { NumberInputType.Int16, typeof(short) },
            { NumberInputType.UInt32, typeof(uint) },
            { NumberInputType.Int32, typeof(int) },
            { NumberInputType.UInt64, typeof(ulong) },
            { NumberInputType.Int64, typeof(long) },
            { NumberInputType.Float, typeof(float) },
            { NumberInputType.Double, typeof(double) },
            { NumberInputType.Decimal, typeof(decimal) }
        };

        private static readonly HashSet<char> NumericChars = new HashSet<char>("0123456789.".ToCharArray());
        public Type ReturnType { get; }
        public string Key { get; }
        public string QuestionText { get; }
        private NumberInputType NumberInputType { get; }

        private string _defaultValue;
        private MethodInfo _parseMethod;

        internal NumberInput(string key, string questionText, NumberInputType numberType, object defaultValue)
        {
            NumberInputType = numberType;
            ReturnType = TypeMap[numberType];
            Key = key;
            QuestionText = questionText;

            _defaultValue = defaultValue.ToString();
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
            Console.Write(parseSuccess ? " ".Repeat(30) : $"{"»".Pastel(Constants.ERROR_TEXT)} Please enter a valid {Enum.GetName(typeof(NumberInputType), NumberInputType)}");
            --Console.CursorTop;

            return parsedValue;
        }
    }
}