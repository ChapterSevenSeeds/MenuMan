using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MenuMan
{
    public class Menu
    {
        public IQuestion[] Questions { get; }
        private readonly Dictionary<string, object> _answers;
        public Menu(params IQuestion[] questions)
        {
            Questions = questions;

            _answers = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Go()
        {
            Console.CursorVisible = false;

            foreach (IQuestion question in Questions)
            {
                Console.Write("? ".Pastel(Constants.QUESTION_MARKER_COLOR));
                Console.Write(question.QuestionText.Pastel(Constants.REGULAR_TEXT_COLOR));
                Console.Write(" ");
                _answers.Add(question.QuestionText, question.Ask());
            }

            Console.CursorVisible = true;

            return _answers;

        }
    }
}
