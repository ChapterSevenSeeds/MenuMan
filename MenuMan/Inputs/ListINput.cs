using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMan.Inputs
{
    internal class ListInput : IQuestion
    {
        public string Key { get ; set; }
        public string QuestionText { get; set; }

        public string[] Choices;

        public object Ask()
        {
            ++Console.CursorTop;

            foreach (var item in Choices)
            {
                Console.CursorLeft = 1;
                Console.WriteLine(item.Pastel("#ABB2BF"));
            }
        }
    }
}
