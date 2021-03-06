using System;
using System.Collections.Generic;

namespace MenuMan.Inputs
{
    internal class CheckboxInput : IQuestion
    {
        public Type ReturnType => typeof(string[]);
        public string Key { get; }
        public string QuestionText { get; }
        public Func<Dictionary<string, object>, bool> Condition { get; }

        private string[] Choices;
        private string[] DefaultSelections;
        private int PageSize;
        private bool AllowEmptyInput;
        private bool DisableSearch;
        private bool ShowSearchOptions;
        internal CheckboxInput(string key, string questionText, string[] choices, bool allowEmptyInput, string[] defaultValue, int pageSize, Func<Dictionary<string, object>, bool> condition, bool disableSearch, bool showSearchOptions)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
            DefaultSelections = defaultValue;
            PageSize = pageSize;
            AllowEmptyInput = allowEmptyInput;
            Condition = condition ?? MiscTools.DefaultCondition;
            DisableSearch = disableSearch;
            ShowSearchOptions = showSearchOptions;
        }
        public object Ask()
        {
            ListBox listBox = new ListBox(null, Choices, SelectionMode.Many, AllowEmptyInput, PageSize, DisableSearch, DefaultSelections, ShowSearchOptions);
            return listBox.Show();
        }
    }
}
