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
            return ConsoleHelpers.ReadStringWithColor("#4AB6C2");
        }
    }
}
