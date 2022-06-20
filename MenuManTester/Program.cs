using MenuMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MenuManTester
{
    public class Program
    {
        static void Main()
        {
            // RegexCompilationInfo expr;
            // List<RegexCompilationInfo> compilationList = new List<RegexCompilationInfo>();

            // Define regular expression to validate format of email address
            //expr = new RegexCompilationInfo(@"[\u001B\u009B][\[\]()#;?]*((([a-zA-Z\d]*(;[-a-zA-Z\d\/#&.:=?%@~_]*)*)?\u0007)|((\d{1,4}(?:;\d{0,4})*)?[\dA-PR-TZcf-ntqry=><~]))",
            //           RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
            //           "ANSIRegex",
            //           "CompiledRegexes",
            //           true);
            // Add info object to list of objects
            // compilationList.Add(expr);

            // Generate assembly with compiled regular expressions
            // RegexCompilationInfo[] compilationArray = new RegexCompilationInfo[compilationList.Count];
            // AssemblyName assemName = new AssemblyName("RegexLib, Version=1.0.0.1001, Culture=neutral, PublicKeyToken=null");
            // compilationList.CopyTo(compilationArray);
            // Regex.CompileToAssembly(compilationArray, assemName);

            //for (int i = 0; i < 0xffff; ++i)
            //{
            //    Console.Write(Convert.ToChar(i));
            //}

            //ListBox listBox = new ListBox("Hello?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello", "Pasta", "Gandhi", "Wired", "Fanta", "Soda", "Root beer", "Beer", "Ben", "Mike", "My mom", "Your dad", "Why is grass?" }, SelectionMode.None, true, 5);
            //var itemse = listBox.Show();
            //foreach (var s in itemse)
            //{
            //    Console.WriteLine(s);
            //}

            var menu = new Menu(
                Questions.NumberInput<int>("money", "How much money do you have?"),
                Questions.TextInput("lastName", "What is your last name?"),
                Questions.ListInput("favoriteFood", "What is your favorite food?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello", "Pasta", "Gandhi", "Wired", "Fanta", "Soda", "Root beer", "Beer", "Ben", "Mike", "My mom", "Your dad", "Why is grass?" }, disableSearch: true),
                Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", new string[] { "Pull me under", "Octavarium", "Vacant", "That one", "This one", "Beethoven", "Tchaikovsky", "The Dark Eternal Night", "Dream Theater", "Rush", "Se7en", "Why?", "Hello?" }, disableSearch: true),
                Questions.ConfirmInput("isWeird", "Are you weird?"));
            var answers = menu.Go();

            foreach (var answer in answers)
            {
                Console.WriteLine($"{answer.Key}: {answer.Value}");
            }

            Console.ReadLine();
        }
    }
}
