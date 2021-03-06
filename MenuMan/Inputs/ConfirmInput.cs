using Pastel;
using System;
using System.Collections.Generic;

namespace MenuMan.Inputs
{
    internal class ConfirmInput : IQuestion
    {
        public Type ReturnType => typeof(YesNo);
        public string Key { get; }
        public string QuestionText { get; }
        public Func<Dictionary<string, object>, bool> Condition { get; }

        private YesNo defaultValue;
        private int lineStart;

        internal ConfirmInput(string key, string questionText, YesNo defaultValue, Func<Dictionary<string, object>, bool> condition)
        {
            Key = key;
            QuestionText = questionText;
            Condition = condition ?? MiscTools.DefaultCondition;
            this.defaultValue = defaultValue;
        }

        public object Ask()
        {
            YesNo answer = defaultValue;
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
                    return answer;
                }
            }
        }

        /// <summary>
        /// Writes the selected answer to the terminal.
        /// </summary>
        /// <param name="answer"></param>
        private void WriteAnswer(YesNo answer)
        {
            Console.CursorLeft = lineStart;
            ConsoleHelpers.WriteWholeLine($"{(answer == YesNo.Yes ? "Y" : "N")}".Pastel(Constants.ACTIVE_TEXT_COLOR));
        }
    }
}
