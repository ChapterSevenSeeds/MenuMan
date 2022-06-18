﻿using MenuMan;
using System;
using System.IO;
using System.Text;

namespace MenuManTester
{
    public class Program
    {
        static void Main()
        {
            //for (int i = 0; i < 0xffff; ++i)
            //{
            //    Console.Write(Convert.ToChar(i));
            //}
            //ListBox listBox = new ListBox("Hello?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello", "Pasta", "Gandhi", "Wired", "Fanta", "Soda", "Root beer", "Beer", "Ben", "Mike", "My mom", "Your dad", "Why is grass?" }, SelectionMode.None, 5);
            //var itemse = listBox.Show();
            //foreach (var s in itemse)
            //{
            //  Console.WriteLine(s);
            //}
            var menu = new Menu(
                Questions.NumberInput<int>("money", "How much money do you have?"),
                Questions.TextInput("lastName", "What is your last name?"),
                Questions.ListInput("favoriteFood", "What is your favorite food?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello", "Pasta", "Gandhi", "Wired", "Fanta", "Soda", "Root beer", "Beer", "Ben", "Mike", "My mom", "Your dad", "Why is grass?" }),
                Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", new string[] { "Pull me under", "Octavarium", "Vacant", "That one", "This one", "Beethoven", "Tchaikovsky", "The Dark Eternal Night", "Dream Theater", "Rush", "Se7en", "Why?", "Hello?" }),
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
