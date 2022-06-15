using Pastel;
using System;

namespace MenuMan.Inputs
{
    internal class ConfirmInput : IQuestion
    {
        public Type ReturnType => typeof(YesNo);
        public string Key { get; }
        public string QuestionText { get; }

        private YesNo _defaultValue;

        internal ConfirmInput(string key, string questionText, YesNo defaultValue = YesNo.Yes)
        {
            Key = key;
            QuestionText = questionText;

            _defaultValue = defaultValue;
        }

        public object Ask()
        {
            YesNo answer = _defaultValue;
            int lineStart = Console.CursorLeft;
            Console.Write(answer == YesNo.Yes ? "(Y/n)" : "(y/N)");

            while (true)
            {
                Console.CursorLeft = lineStart;
                ConsoleKey key = ConsoleHelpers.ReadCertainKeys(ConsoleKey.Y, ConsoleKey.N, ConsoleKey.Enter).Key;

                if (key == ConsoleKey.Y)
                {
                    answer = YesNo.Yes;
                    Console.Write("Y    ".Pastel(Constants.ACTIVE_TEXT_COLOR));
                }
                else if (key == ConsoleKey.N)
                {
                    answer = YesNo.No;
                    Console.Write("N    ".Pastel(Constants.ACTIVE_TEXT_COLOR));
                }
                else if (key == ConsoleKey.Enter) return answer; 
            }
        }
    }
}
