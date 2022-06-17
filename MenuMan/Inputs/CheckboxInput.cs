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
        public string[] Choices;
        public string[] DefaultSelections;
        public int PageSize;
        internal CheckboxInput(string key, string questionText, string[] choices, string[] defaultValue, int pageSize)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
            DefaultSelections = defaultValue;
            PageSize = pageSize;
        }
        public object Ask()
        {
            ListBox listBox = new ListBox(null, Choices, SelectionMode.Many, PageSize, DefaultSelections);
            return listBox.Show();
        }
    }
}
