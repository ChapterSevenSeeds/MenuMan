using System;

namespace MenuMan.Inputs
{
    internal class ListInput : IQuestion
    {
        public Type ReturnType => typeof(string);
        public string Key { get; }
        public string QuestionText { get; }

        public string[] Choices;

        private int PageSize;
        private string[] Defaults;

        internal ListInput(string key, string questionText, string[] choices, string defaultValue, int pageSize)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
            PageSize = pageSize;
            Defaults = new string[] { defaultValue };
        }

        public object Ask()
        {
            ListBox listBox = new ListBox(null, Choices, SelectionMode.Single, PageSize, Defaults);
            return listBox.Show();
        }
    }
}
