using System;
using System.Collections.Generic;

namespace MenuMan
{
    internal class TextInput : IQuestion
    {
        public Type ReturnType => typeof(string);
        public string Key { get; }
        public string QuestionText { get; }

        public Func<Dictionary<string, object>, bool> Condition { get; }
        private readonly string defaultValue;
        private readonly bool allowEmptyInput;

        internal TextInput(string key, string questionText, bool allowEmptyInput, string defaultValue, Func<Dictionary<string, object>, bool> condition)
        {
            Key = key;
            QuestionText = questionText;

            this.defaultValue = defaultValue;
            this.allowEmptyInput = allowEmptyInput;

            Condition = condition ?? MiscTools.DefaultCondition;
        }

        public object Ask()
        {
            return ConsoleHelpers.ReadStringWithColor(Constants.ACTIVE_TEXT_COLOR, allowEmptyInput, defaultValue);
        }
    }
}
