using MenuMan;
using System;

namespace MenuManTester
{
    public class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu<Answers>(
                Questions.NumberInput(nameof(Answers.Money), "How much money do you have?", NumberInputType.Decimal),
                Questions.TextInput(nameof(Answers.FirstName), "What is your first name?"),
                Questions.TextInput(nameof(Answers.LastName), "What is your last name?"),
                Questions.ListInput(nameof(Answers.FavoriteFood), "What is your favorite food?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello" }),
                Questions.CheckboxInput(nameof(Answers.FavoriteSongs), "What are your favorite songs?", new string[] { "Pull me under", "Octavarium", "Vacant" }),
                Questions.ConfirmInput(nameof(Answers.IsGay), "Are you gay?"));
            var answers = menu.Go();

            Console.ReadLine();
        }
    }

    public class Answers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FavoriteFood { get; set; }
        public string[] FavoriteSongs { get; set; }
        public YesNo IsGay { get; set; }
        public decimal Money { get; set; }
    }
}
