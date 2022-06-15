using System;

namespace MenuMan
{
    internal class TextInput : IQuestion
    {
        public Type ReturnType => typeof(string);
        public string Key { get; }
        public string QuestionText { get; }

        private string _defaultValue;

        internal TextInput(string key, string questionText, string defaultValue = "")
        {
            Key = key;
            QuestionText = questionText;

            _defaultValue = defaultValue;
        }

        public object Ask()
        {
            return ConsoleHelpers.ReadStringWithColor(Constants.ACTIVE_TEXT_COLOR, _defaultValue);
        }
    }
}
