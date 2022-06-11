using MenuMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManTester
{
    public class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu<Answers>(Questions.TextInput(nameof(Answers.Name), "What is your name?"));
            var answers = menu.Go();

            Console.ReadLine();
        }
    }

    public class Answers
    {
        public string Name { get; set; }
    }
}
