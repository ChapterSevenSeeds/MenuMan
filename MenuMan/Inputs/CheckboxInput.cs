using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan.Inputs
{
    using CustomMessageValidationFunc = Func<string[], Dictionary<string, object>, string>;
    using ValidationPredicate = Func<string[], Dictionary<string, object>, bool>;
    internal class CheckboxInput : IQuestion
    {
        public Type ReturnType => typeof(string[]);
        public string Key { get; }
        public string QuestionText { get; }
        public string[] Choices;
        public CustomMessageValidationFunc ValidationWithCustomMessage { get; }
        public ValidationPredicate ValidationPredicate { get; }
        public string ValidationMessage { get; }
        public bool ValidateOnLoad { get; }
        internal CheckboxInput(string key, string questionText, string[] choices)
        {
            Key = key;
            QuestionText = questionText;
            Choices = choices;
        }

        internal CheckboxInput(string key, string questionText, string[] choices, CustomMessageValidationFunc customMessageValidationFunction, bool validateOnLoad) : this(key, questionText, choices)
        {
            ValidationWithCustomMessage = customMessageValidationFunction;
            ValidateOnLoad = validateOnLoad;
        }

        internal CheckboxInput(string key, string questionText, string[] choices, ValidationPredicate validationPredicate, string validationMessage, bool validateOnLoad) : this(key, questionText, choices)
        {
            ValidationPredicate = validationPredicate;
            ValidationMessage = validationMessage;
            ValidateOnLoad = validateOnLoad;
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

                if (lastPress == ConsoleKey.UpArrow && _highlightedIndex > 0) --_highlightedIndex;
                else if (lastPress == ConsoleKey.DownArrow && _highlightedIndex < Choices.Length - 1) ++_highlightedIndex;
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

            for (int i = 0; i < Choices.Length; ++i)
            {
                Console.WriteLine($"{(i == _highlightedIndex ? ">" : " ")}{(_selectedIndexes.Contains(i) ? "→" : " ")}{Choices[i]}".Pastel(i == _highlightedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR));
            }
        }
    }
}
