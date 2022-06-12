using Pastel;
using System;

namespace MenuMan.Inputs
{
    internal class TextInput : IQuestion
    {
        public string Key { get; set; }
        public string QuestionText { get; set; }
        public object Ask()
        {
            return ConsoleHelpers.ReadStringWithColor(Constants.ACTIVE_TEXT_COLOR);
        }
    }
}
