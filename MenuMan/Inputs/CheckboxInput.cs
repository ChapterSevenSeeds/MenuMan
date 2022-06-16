using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan.Inputs
{
    internal class CheckboxInput : IQuestion
    {
        public Type ReturnType => typeof(string[]);
        public string Key { get; }
        public string QuestionText { get; }
        public string[] Choices;
        private int _pageSize;
        private int _pagingIndexOffset = 0;
        internal CheckboxInput(string key, string questionText, string[] choices, string[] defaultValue, int pageSize)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
            
            if (defaultValue != null)
            {
                var choicesList = Choices.ToList();
                foreach (string choice in defaultValue)
                {
                    _selectedIndexes.Add(choicesList.IndexOf(choice));
                }
            }

            _pageSize = pageSize;
        }

        private int _consolePositionForListStart;
        private int _highlightedIndex = 0;
        private HashSet<int> _selectedIndexes = new HashSet<int>();
        public object Ask()
        {
            _consolePositionForListStart = ++Console.CursorTop;

            while (true)
            {
                PrintList();
               
                ConsoleKey lastPress = ConsoleHelpers.ReadCertainKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Spacebar).Key;

                if (lastPress == ConsoleKey.UpArrow && _highlightedIndex > 0)
                {
                    if (--_highlightedIndex < _pagingIndexOffset) --_pagingIndexOffset;
                }
                else if (lastPress == ConsoleKey.DownArrow && _highlightedIndex < Choices.Length - 1)
                {
                    if (++_highlightedIndex - _pagingIndexOffset >= _pageSize) ++_pagingIndexOffset;
                }
                else if (lastPress == ConsoleKey.Enter && _selectedIndexes.Count > 0) return _selectedIndexes.Select(x => Choices[x]).ToArray();
                else if (lastPress == ConsoleKey.Spacebar)
                {
                    if (_selectedIndexes.Contains(_highlightedIndex)) _selectedIndexes.Remove(_highlightedIndex);
                    else _selectedIndexes.Add(_highlightedIndex);

                }
            }
        }

        private void PrintList()
        {
            Console.CursorTop = _consolePositionForListStart;
            Console.CursorLeft = 0;

            if (Choices.Length >= _pageSize)
            {
                if (_pagingIndexOffset > 0)
                {
                    Console.WriteLine("▲▲▲▲▲ More Options ▲▲▲▲▲");
                    Console.CursorLeft = 0;
                }
                else
                {
                    Console.CursorTop += _pageSize + 1;
                    Console.Write(" ".Repeat(Console.WindowWidth));
                    Console.CursorLeft = 0;
                    Console.CursorTop -= _pageSize + 1;
                }
                Console.CursorTop += _pageSize;
                if (_pagingIndexOffset + _pageSize < Choices.Length)
                {
                    Console.Write("▼▼▼▼▼ More Options ▼▼▼▼▼");
                    Console.CursorLeft = 0;
                }
                else
                {
                    Console.Write(" ".Repeat(Console.WindowWidth));
                    Console.CursorLeft = 0;
                }
                Console.CursorTop -= _pageSize;
            }

            for (int i = _pagingIndexOffset; i < Math.Min(_pageSize, Choices.Length) + _pagingIndexOffset; ++i)
            {
                Console.WriteLine($"{(i == _highlightedIndex ? ">" : " ")}{(_selectedIndexes.Contains(i) ? "→" : " ")}{Choices[i]}{" ".Repeat(Console.WindowWidth - Choices[i].Length - 2)}".Pastel(i == _highlightedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR));
            }
        }
    }
}
