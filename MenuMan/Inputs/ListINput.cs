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

        private int _selectedIndex = 0;
        private int _consolePositionForListStart;

        public object Ask()
        {
            _consolePositionForListStart = ++Console.CursorTop;

            while (true)
            {
                PrintList();
                ConsoleKey lastPress = ConsoleHelpers.ReadCertainKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter).Key;

                if (lastPress == ConsoleKey.UpArrow && _selectedIndex > 0) --_selectedIndex;
                else if (lastPress == ConsoleKey.DownArrow && _selectedIndex < Choices.Length - 1) ++_selectedIndex;
                else if (lastPress == ConsoleKey.Enter)
                {
                    Console.CursorTop = _consolePositionForListStart + Choices.Length;
                    return Choices[_selectedIndex];
                }
                else continue;
            }

        }

        private void PrintList()
        {
            Console.CursorTop = _consolePositionForListStart;
            Console.CursorLeft = 0;

            for (int i = 0; i < Choices.Length; ++i)
            {
                Console.WriteLine($"{(i == _selectedIndex ? ">" : " ")} {Choices[i]}".Pastel(i == _selectedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR));
            }
        }
    }
}
