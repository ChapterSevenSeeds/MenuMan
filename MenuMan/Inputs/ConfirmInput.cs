using Pastel;
using System;

namespace MenuMan.Inputs
{
    internal class ConfirmInput : IQuestion
    {
        public Type ReturnType => typeof(YesNo);
        public string Key { get; }
        public string QuestionText { get; }

        internal ConfirmInput(string key, string questionText)
        {
            Key = key;
            QuestionText = questionText;
        }

        public object Ask()
        {
            YesNo? answer = null;
            int lineStart = Console.CursorLeft;
            Console.Write($"(y/n)");

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
                else if (key == ConsoleKey.Enter && answer.HasValue) return answer.Value; 
            }
        }
    }
}
