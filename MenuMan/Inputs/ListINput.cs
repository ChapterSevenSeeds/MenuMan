using Pastel;
using System;
using System.Linq;

namespace MenuMan.Inputs
{
    internal class ListInput : IQuestion
    {
        public Type ReturnType => typeof(string);
        public string Key { get; }
        public string QuestionText { get; }

        public string[] Choices;

        internal ListInput(string key, string questionText, string[] choices, string defaultValue, int pageSize)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;

            _selectedIndex = Choices.ToList().IndexOf(defaultValue);
            if (_selectedIndex == -1) _selectedIndex = 0;

            _pageSize = pageSize;
        }

        private int _selectedIndex = 0;
        private int _consolePositionForListStart;
        private int _pageSize;
        private int _pagingIndexOffset = 0;

        public object Ask()
        {
            _consolePositionForListStart = ++Console.CursorTop;

            while (true)
            {
                PrintList();
                ConsoleKey lastPress = ConsoleHelpers.ReadCertainKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter).Key;

                if (lastPress == ConsoleKey.UpArrow && _selectedIndex > 0)
                {
                    if (--_selectedIndex < _pagingIndexOffset) --_pagingIndexOffset;
                }
                else if (lastPress == ConsoleKey.DownArrow && _selectedIndex < Choices.Length - 1)
                {
                    if (++_selectedIndex - _pagingIndexOffset >= _pageSize) ++_pagingIndexOffset;
                }
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
                Console.WriteLine($"{(i == _selectedIndex ? ">" : " ")} {Choices[i]}{" ".Repeat(Console.WindowWidth - Choices[i].Length - 2)}".Pastel(i == _selectedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR));
            }
        }
    }
}
