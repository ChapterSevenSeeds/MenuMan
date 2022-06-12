using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMan.Inputs
{
    internal class RadioInput : IQuestion
    {
        public string Key { get; set; }
        public string QuestionText { get; set; }
        public string[] Choices;

        private int _consolePositionForListStart;
        private int _highlightedIndex = 0;
        private int _selectedIndex = -1;
        public object Ask()
        {
            _consolePositionForListStart = ++Console.CursorTop;


            while (true)
            {
                PrintList();
               
                ConsoleKey lastPress = ConsoleHelpers.ReadCertainKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Spacebar).Key;

                if (lastPress == ConsoleKey.UpArrow && _highlightedIndex > 0) --_highlightedIndex;
                else if (lastPress == ConsoleKey.DownArrow && _highlightedIndex < Choices.Length - 1) ++_highlightedIndex;
                else if (lastPress == ConsoleKey.Enter && _selectedIndex != -1) return Choices[_selectedIndex];
                else if (lastPress == ConsoleKey.Spacebar) _selectedIndex = _highlightedIndex;
            }
        }

        private void PrintList()
        {
            Console.CursorTop = _consolePositionForListStart;
            Console.CursorLeft = 0;

            for (int i = 0; i < Choices.Length; ++i)
            {
                Console.WriteLine($"{(i == _highlightedIndex ? ">" : " ")}{(i == _selectedIndex ? "\u25CF" : "\u25CB")}{Choices[i]}".Pastel(i == _highlightedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR));
            }
        }
    }
}
