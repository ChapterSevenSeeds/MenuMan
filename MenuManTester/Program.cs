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
            //    Console.OutputEncoding = System.Text.Encoding.UTF8;
            //    Console.Write("\xfeff"); // bom = byte order mark
            //    Console.WriteLine("Sailboats: ⛵~\u26f5" +
            //"\n" +  // or \r
            //"\x043a\x043e\x0448\x043a\x0430 \x65e5\x672c\x56fd\U00002728⏰\u25a3\u06de\u02a5\u0414\u0416\u0489\u8966");
            var menu = new Menu<Answers>(
                Questions.TextInput(nameof(Answers.FirstName), "What is your first name?"),
                Questions.TextInput(nameof(Answers.LastName), "What is your last name?"),
                Questions.ListInput(nameof(Answers.FavoriteFood), "What is your favorite food?", new string[] { "Pizza", "Spaghetti", "Your mom", "Hello" }),
                Questions.RadioInput(nameof(Answers.FavoriteSong), "What is your favorite food?", new string[] { "Pull me under", "Octavarium", "Vacant" }));
            var answers = menu.Go();

            Console.ReadLine();
        }
    }

    public class Answers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FavoriteFood { get; set; }
        public string FavoriteSong { get; set; }
    }
}
