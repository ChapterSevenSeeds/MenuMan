using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan
{
    enum SearchMode
    {
        StartsWith,
        EndsWith,
        Contains,
        Regex
    }

    public enum SelectionMode
    {
        Single,
        Many,
        None
    }

    public class ListBox
    {
        private string Prompt { get;}
        private string[] Choices { get; }
        private SelectionMode SelectionMode { get; }
        private int PageSize { get; }

        private int ScrollIndexOffset;
        private int CursorTopForListStart;
        private int CursorLeftForSearchText;
        private int HighlightedIndex;
        private HashSet<int> SelectedIndexes = new HashSet<int>();
        private string HelperText;

        public ListBox(string prompt, string[] choices, SelectionMode selectionMode, int pageSize, string[] defaultValue = null)
        {
            Prompt = prompt;
            Choices = choices;
            SelectionMode = selectionMode;
            PageSize = pageSize;
            Prompt = prompt;
            HelperText = SelectionMode != SelectionMode.None ? "choices" : "items";

            if (SelectionMode == SelectionMode.Single && defaultValue?.Length > 1) throw new ArgumentException("SelectionMode.Single and a default selection of more than one item is not allowed.");
            if (SelectionMode == SelectionMode.None && defaultValue?.Length > 0) throw new ArgumentException("SelectionMode.None cannot have a default selection.");

            if (prompt != "" && prompt != null) Console.Write($"{prompt} ");
            CursorTopForListStart = Console.CursorTop + 1;
            CursorLeftForSearchText = Console.CursorLeft;

            if (defaultValue != null)
            {
                var listifiedChoices = Choices.ToList();

                if (SelectionMode == SelectionMode.Single)
                {
                    var index = listifiedChoices.IndexOf(defaultValue[0]);
                    if (index > -1) HighlightedIndex = index;
                    else HighlightedIndex = 0;
                } 
                else
                {
                    foreach (var item in defaultValue)
                    {
                        var index = listifiedChoices.IndexOf(item);
                        if (index > -1) SelectedIndexes.Add(index);
                    }
                }
            }
        }

        public string[] Show()
        {
            while (true)
            {
                PrintList();
                ConsoleKey lastPress = ConsoleHelpers.ReadCertainKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Spacebar).Key;

                if (lastPress == ConsoleKey.UpArrow)
                {
                    if ((SelectionMode == SelectionMode.Many || SelectionMode == SelectionMode.Single) && HighlightedIndex > 0)
                    {
                        --HighlightedIndex;
                        if (HighlightedIndex < ScrollIndexOffset) --ScrollIndexOffset;
                    }
                    else if (SelectionMode == SelectionMode.None && ScrollIndexOffset > 0) --ScrollIndexOffset;
                }
                else if (lastPress == ConsoleKey.DownArrow)
                {
                    if ((SelectionMode == SelectionMode.Many || SelectionMode == SelectionMode.Single) && HighlightedIndex < Choices.Length - 1)
                    {
                        ++HighlightedIndex;
                        if (HighlightedIndex - ScrollIndexOffset >= PageSize) ++ScrollIndexOffset;
                    }
                    else if (SelectionMode == SelectionMode.None && ScrollIndexOffset + PageSize < Choices.Length) ++ScrollIndexOffset;

                }
                else if (lastPress == ConsoleKey.Spacebar && SelectionMode == SelectionMode.Many)
                {
                    if (SelectedIndexes.Contains(HighlightedIndex)) SelectedIndexes.Remove(HighlightedIndex);
                    else SelectedIndexes.Add(HighlightedIndex);
                }
                else if (lastPress == ConsoleKey.Enter)
                {
                    Console.CursorTop = CursorTopForListStart;
                    for (int i = 0; i < PageSize + 2; ++i) ConsoleHelpers.WriteWholeLine();
                    Console.CursorTop = CursorTopForListStart - 1;
                    Console.CursorLeft = CursorLeftForSearchText;

                    if (SelectionMode == SelectionMode.Single)
                    {
                        Console.WriteLine(Choices[HighlightedIndex].Pastel(Constants.ACTIVE_TEXT_COLOR));
                        return new string[] { Choices[HighlightedIndex] };
                    }
                    else if (SelectionMode == SelectionMode.Many)
                    {
                        var items = SelectedIndexes.Select(i => Choices[i]).ToArray();
                        Console.WriteLine(string.Join(",", items).Pastel(Constants.ACTIVE_TEXT_COLOR));
                        return items;
                    }
                    else 
                    {
                        Console.Write(Environment.NewLine);
                        return Choices;
                    }
                }
                else continue;
            }
        }

        private void PrintList()
        {
            Console.CursorTop = CursorTopForListStart;
            Console.CursorLeft = 0;

            if (Choices.Length >= PageSize)
            {
                if (ScrollIndexOffset > 0) ConsoleHelpers.WriteWholeLine($"  (more {HelperText})".Pastel(Constants.INFO_TEXT));
                else
                {
                    Console.CursorTop += PageSize + 1;
                    ConsoleHelpers.WriteWholeLine(false);
                    Console.CursorLeft = 0;
                    Console.CursorTop -= PageSize + 1;
                }
                Console.CursorTop += PageSize;
                if (ScrollIndexOffset + PageSize < Choices.Length)
                {
                    ConsoleHelpers.WriteWholeLine($"  (more {HelperText})".Pastel(Constants.INFO_TEXT), false);
                    Console.CursorLeft = 0;
                }
                else
                {
                    ConsoleHelpers.WriteWholeLine(false);
                    Console.CursorLeft = 0;
                }

                Console.CursorTop -= PageSize;
            }

            for (int i = ScrollIndexOffset; i < Math.Min(PageSize, Choices.Length) + ScrollIndexOffset; ++i)
            {
                string toDisplay = $"{(i == HighlightedIndex && SelectionMode != SelectionMode.None ? ">" : " ")}{(SelectionMode == SelectionMode.Many && SelectedIndexes.Contains(i) ? "→" : " ")}{Choices[i]}";
                if (SelectionMode != SelectionMode.None) toDisplay = toDisplay.Pastel(i == HighlightedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR);
                ConsoleHelpers.WriteWholeLine(toDisplay);
            }
        }
    }
}