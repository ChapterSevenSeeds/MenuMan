using System;

namespace MenuMan
{
    internal class TextInput : IQuestion
    {
        public Type ReturnType => typeof(string);
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
