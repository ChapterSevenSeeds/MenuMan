using System;

namespace MenuMan
{
    internal class TextInput : IQuestion
    {
        public Type ReturnType => typeof(string);
        public string Key { get; }
        public string QuestionText { get; }

        private readonly string defaultValue;
        private readonly bool allowEmptyInput;

        internal TextInput(string key, string questionText, bool allowEmptyInput, string defaultValue = "")
        {
            Key = key;
            QuestionText = questionText;

            this.defaultValue = defaultValue;
            this.allowEmptyInput = allowEmptyInput;
        }

        public object Ask()
        {
            return ConsoleHelpers.ReadStringWithColor(Constants.ACTIVE_TEXT_COLOR, allowEmptyInput, defaultValue);
        }
    }
}
