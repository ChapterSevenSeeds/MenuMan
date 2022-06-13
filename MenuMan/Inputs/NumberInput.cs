using Pastel;
using System;
using System.Collections.Generic;

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

        private static readonly HashSet<ConsoleKey> NumericKeys = new HashSet<ConsoleKey> { ConsoleKey.D0, ConsoleKey.NumPad0, ConsoleKey.D1, ConsoleKey.NumPad1, ConsoleKey.D2, ConsoleKey.NumPad2, ConsoleKey.D3, ConsoleKey.NumPad3, ConsoleKey.D4, ConsoleKey.NumPad4, ConsoleKey.D5, ConsoleKey.NumPad5, ConsoleKey.D6, ConsoleKey.NumPad6, ConsoleKey.D7, ConsoleKey.NumPad7, ConsoleKey.D8, ConsoleKey.NumPad8, ConsoleKey.D9, ConsoleKey.NumPad9, ConsoleKey.OemPeriod, ConsoleKey.Decimal, ConsoleKey.Backspace, ConsoleKey.Enter };
        public Type ReturnType { get; }
        public string Key { get; }
        public string QuestionText { get; }
        private NumberInputType NumberInputType { get; }

        internal NumberInput(string key, string questionText, NumberInputType numberType)
        {
            NumberInputType = numberType;
            ReturnType = TypeMap[numberType];
            Key = key;
            QuestionText = questionText;
        }

        public object Ask()
        {
            int stringStart = Console.CursorLeft;
            string runningString = "";
            var parseMethod = ReturnType.GetMethod("TryParse", new Type[] { typeof(string), ReturnType.MakeByRefType() });
            object parsedValue = null;

            while (true)
            {
                Console.CursorLeft = stringStart;
                Console.Write(runningString.Pastel(Constants.ACTIVE_TEXT_COLOR));
                Console.Write(" ");

                ConsoleKeyInfo keyInfo = ConsoleHelpers.ReadAnyKey();

                if (!NumericKeys.Contains(keyInfo.Key)) continue;

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

                object[] parameters = new object[] { runningString, null };
                if ((bool)parseMethod.Invoke(null, parameters))
                {
                    parsedValue = parameters[1];
                    Console.CursorLeft = 0;
                    ++Console.CursorTop;
                    Console.Write(" ".Repeat(30));
                    --Console.CursorTop;
                }
                else
                {
                    parsedValue = null;
                    Console.CursorLeft = 0;
                    ++Console.CursorTop;
                    Console.Write($"{"»".Pastel(Constants.ERROR_TEXT)} Please enter a valid {Enum.GetName(typeof(NumberInputType), NumberInputType)}");
                    --Console.CursorTop;
                }
            }
        }
    }
}
