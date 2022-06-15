using MenuMan;
using System;

namespace MenuManTester
{
    public class Program
    {
        static void Main()
        {
            var menu = new Menu(
                Questions.NumberInput("money", "How much money do you have?", NumberInputType.UInt8, 35),
                Questions.TextInput("firstName", "What is your first name?", "Tyson"),
                Questions.TextInput("lastName", "What is your last name?", "Jones"),
                Questions.ListInput("favoriteFood", "What is your favorite food?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello" }, "Hello"),
                Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", new string[] { "Pull me under", "Octavarium", "Vacant" }, new string[] { "Vacant", "Octavarium" }),
                Questions.ConfirmInput("isWeird", "Are you weird?", YesNo.Yes));
            var answers = menu.Go();

            foreach (var answer in answers) 
            {
                Console.WriteLine($"{answer.Key}: {answer.Value}");
            }

            Console.ReadLine();
        }
    }
}
