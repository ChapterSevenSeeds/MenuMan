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
            ListBox listBox = new ListBox("Hello?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello", "Pasta", "Gandhi", "Wired", "Fanta", "Soda", "Root beer", "Beer", "Ben", "Mike", "My mom", "Your dad", "Why is grass?" }, SelectionMode.None, true, 5);
            var itemse = listBox.Show();
            foreach (var s in itemse)
            {
                Console.WriteLine(s);
            }

            var menu = new Menu(
                Questions.NumberInput<int>("money", "How much money do you have?", defaultValue: 33),
                Questions.TextInput("lastName", "What is your last name?", defaultValue: "Tyson"),
                Questions.ListInput("favoriteFood", "What is your favorite foodasdfasdfasdfasdfasdfasdf?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello", "Pasta", "Gandhi", "Wired", "Fanta", "Soda", "Root beer", "Beer", "Ben", "Mike", "My mom", "Your dad", "Why is grass?" }),
                Questions.ConfirmInput("isWeird", "Are you weird?"),
                Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", new string[] { "Pull me under", "Octavarium", "Vacant", "That one", "This one", "Beethoven", "Tchaikovsky", "The Dark Eternal Night", "Dream Theater", "Rush", "Se7en", "Why?", "Hello?" }));
            var answers = menu.Go();

            foreach (var answer in answers)
            {
                Console.WriteLine($"{answer.Key}: {answer.Value}");
            }

            Console.ReadLine();
        }
    }
}
