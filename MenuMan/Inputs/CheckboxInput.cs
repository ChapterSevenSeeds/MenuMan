using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan.Inputs
{
    internal class CheckboxInput : IQuestion
    {
        public Type ReturnType => typeof(string[]);
        public string Key { get; }
        public string QuestionText { get; }
        public bool Condition(Dictionary<string, object> answers) => condition(answers);

        private Func<Dictionary<string, object>, bool> condition;
        private string[] Choices;
        private string[] DefaultSelections;
        private int PageSize;
        private bool AllowEmptyInput;

        internal CheckboxInput(string key, string questionText, string[] choices, bool allowEmptyInput, string[] defaultValue, int pageSize)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
            DefaultSelections = defaultValue;
            PageSize = pageSize;
            AllowEmptyInput = allowEmptyInput;
        }
        public object Ask()
        {
            ListBox listBox = new ListBox(null, Choices, SelectionMode.Many, AllowEmptyInput, PageSize, DefaultSelections);
            return listBox.Show();
        }
    }
}
