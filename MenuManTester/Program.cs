﻿using MenuMan;
using System;

namespace MenuManTester
{
    public class Program
    {
        static void Main()
        {
            var menu = new Menu(
                Questions.NumberInput("money", "How much money do you have?", NumberInputType.Decimal),
                Questions.TextInput("firstName", "What is your first name?"),
                Questions.TextInput("lastName", "What is your last name?"),
                Questions.ListInput("favoriteFood", "What is your favorite food?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello" }),
                Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", new string[] { "Pull me under", "Octavarium", "Vacant" }),
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
