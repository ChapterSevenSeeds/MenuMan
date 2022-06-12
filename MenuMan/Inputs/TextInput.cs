using Pastel;
using System;

namespace MenuMan.Inputs
{
    internal class TextInput : IQuestion
    {
        public string Key { get; }
        public string QuestionText { get; }

        internal TextInput(string key, string questionText)
        {
            Key = key;
            QuestionText = questionText;
        }

        public object Ask()
        {
            return ConsoleHelpers.ReadStringWithColor(Constants.ACTIVE_TEXT_COLOR);
        }
    }
}
