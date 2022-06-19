using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan.Inputs
{
    internal class ListInput : IQuestion
    {
        public Type ReturnType => typeof(string);
        public string Key { get; }
        public string QuestionText { get; }
        public Func<Dictionary<string, object>, bool> Condition { get; }

        public string[] Choices;
        private int PageSize;
        private string[] Defaults;
        private bool AllowEmptyInput;

        internal ListInput(string key, string questionText, string[] choices, bool allowEmptyInput, string defaultValue, int pageSize, Func<Dictionary<string, object>, bool> condition)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
            PageSize = pageSize;
            AllowEmptyInput = allowEmptyInput;
            Condition = condition ?? MiscTools.DefaultCondition;

            if (defaultValue != null && defaultValue != "") Defaults = new string[] { defaultValue };
        }

        public object Ask()
        {
            ListBox listBox = new ListBox(null, Choices, SelectionMode.Single, AllowEmptyInput, PageSize, Defaults);
            return listBox.Show().FirstOrDefault();
        }
    }
}
