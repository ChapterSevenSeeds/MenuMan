using Pastel;
using System;
using System.Collections.Generic;

namespace MenuMan
{
    public class Menu
    {
        /// <summary>
        /// The questions associated with this menu.
        /// </summary>
        public IQuestion[] Questions { get; }

        private readonly Dictionary<string, object> answers;

        /// <summary>
        /// Constructs a new menu.
        /// </summary>
        /// <param name="questions">The questions to put in the menu.</param>
        public Menu(params IQuestion[] questions)
        {
            Questions = questions;

            answers = new Dictionary<string, object>();
        }

        /// <summary>
        /// Launches the menu.
        /// </summary>
        /// <returns>The answers to the questions.</returns>
        public Dictionary<string, object> Go()
        {
            Console.CursorVisible = false;

            foreach (IQuestion question in Questions)
            {
                if (!question.Condition(answers)) continue;

                Console.Write("? ".Pastel(Constants.QUESTION_MARKER_COLOR));
                Console.Write(question.QuestionText.Pastel(Constants.REGULAR_TEXT_COLOR));
                Console.Write(" ");
                answers.Add(question.Key, question.Ask());
            }

            Console.CursorVisible = true;

            return answers;
        }
    }
}
