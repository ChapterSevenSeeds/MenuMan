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
        private int lineStart;

        internal ConfirmInput(string key, string questionText, YesNo defaultValue = YesNo.Yes)
        {
            Key = key;
            QuestionText = questionText;

            _defaultValue = defaultValue;
        }

        public object Ask()
        {
            YesNo answer = _defaultValue;
            lineStart = Console.CursorLeft;
            Console.Write((answer == YesNo.Yes ? "(Y/n)" : "(y/N)").Pastel(Constants.INFO_TEXT));

            while (true)
            {
                Console.CursorLeft = lineStart;
                ConsoleKey key = ConsoleHelpers.ReadCertainKeys(ConsoleKey.Y, ConsoleKey.N, ConsoleKey.Enter).Key;

                if (key == ConsoleKey.Y)
                {
                    answer = YesNo.Yes;
                    WriteAnswer(answer);
                }
                else if (key == ConsoleKey.N)
                {
                    answer = YesNo.No;
                    WriteAnswer(answer);
                }
                else if (key == ConsoleKey.Enter)
                {
                    WriteAnswer(answer);
                    Console.Write(Environment.NewLine);
                    return answer;
                }
            }
        }

        private void WriteAnswer(YesNo answer)
        {
            Console.CursorLeft = lineStart;
            Console.Write($"{(answer == YesNo.Yes ? "Y" : "N")}    ".Pastel(Constants.ACTIVE_TEXT_COLOR));
        }
    }
}
