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
        private string Prompt { get; }
        private string[] Choices { get; }
        private SelectionMode SelectionMode { get; }
        private int PageSize { get; }

        private int ScrollIndexOffset;
        private int CursorTopForListStart;
        private int CursorLeftForSearchText;
        private int HighlightedIndex;
        private HashSet<string> SelectedItems = new HashSet<string>();
        private string HelperText;
        private string SearchText = "";
        private bool ShowSearchModeSelection;
        private string[] FilteredChoices;
        private bool AllowEmptyInput;

        public ListBox(string prompt, string[] choices, SelectionMode selectionMode, bool allowEmptyInput, int pageSize, string[] defaultValue = null, bool showSearchModeSelection = true)
        {
            Prompt = prompt;
            Choices = choices;

            FilteredChoices = new string[Choices.Length];
            Array.Copy(Choices, FilteredChoices, Choices.Length);

            SelectionMode = selectionMode;
            PageSize = pageSize;
            Prompt = prompt;
            HelperText = SelectionMode != SelectionMode.None ? "choices" : "items";
            AllowEmptyInput = allowEmptyInput;

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
                        if (index > -1) SelectedItems.Add(FilteredChoices[index]);
                    }
                }
            }
        }

        public string[] Show()
        {
            while (true)
            {
                PrintList();
                ConsoleKeyInfo lastPress = ConsoleHelpers.ReadAnyKey();

                switch (lastPress.Key)
                {
                    case ConsoleKey.UpArrow:
                        if ((SelectionMode == SelectionMode.Many || SelectionMode == SelectionMode.Single) && HighlightedIndex > 0)
                        {
                            --HighlightedIndex;
                            if (HighlightedIndex < ScrollIndexOffset) --ScrollIndexOffset;
                        }
                        else if (SelectionMode == SelectionMode.None && ScrollIndexOffset > 0) --ScrollIndexOffset;

                        break;
                    case ConsoleKey.DownArrow:
                        if ((SelectionMode == SelectionMode.Many || SelectionMode == SelectionMode.Single) && HighlightedIndex < FilteredChoices.Length - 1)
                        {
                            ++HighlightedIndex;
                            if (HighlightedIndex - ScrollIndexOffset >= PageSize) ++ScrollIndexOffset;
                        }
                        else if (SelectionMode == SelectionMode.None && ScrollIndexOffset + PageSize < FilteredChoices.Length) ++ScrollIndexOffset;

                        break;
                    case ConsoleKey.Spacebar:
                        if (SelectionMode == SelectionMode.Many)
                        {
                            if (SelectedItems.Contains(FilteredChoices[HighlightedIndex])) SelectedItems.Remove(FilteredChoices[HighlightedIndex]);
                            else SelectedItems.Add(FilteredChoices[HighlightedIndex]);
                        }

                        break;
                    case ConsoleKey.Enter:
                        Console.CursorTop = CursorTopForListStart;
                        for (int i = 0; i < PageSize + 2; ++i) ConsoleHelpers.WriteWholeLine();
                        Console.CursorTop = CursorTopForListStart - 1;
                        Console.CursorLeft = CursorLeftForSearchText;

                        string[] returnValue;
                        if (SelectionMode == SelectionMode.Single) returnValue = HandleEnterPress(() => FilteredChoices[HighlightedIndex], () => new string[] { FilteredChoices[HighlightedIndex] });
                        else if (SelectionMode == SelectionMode.Many) returnValue = HandleEnterPress(() => string.Join(",", SelectedItems), () => SelectedItems.ToArray());
                        else returnValue = HandleEnterPress(() => $"{FilteredChoices.Length} items selected", () => FilteredChoices);

                        if (returnValue == null) break;
                        return returnValue;
                    case ConsoleKey.Backspace:
                        if (SearchText.Length > 0) SearchText = SearchText.Substring(0, SearchText.Length - 1);
                        break;
                    default:
                        SearchText += lastPress.KeyChar;
                        break;
                }

                if (SearchText == "")
                {
                    FilteredChoices = new string[Choices.Length];
                    Array.Copy(Choices, FilteredChoices, Choices.Length);
                }
                else
                {
                    var filtered = new List<string>();
                    foreach (var choice in Choices)
                    {
                        if (choice.Contains(SearchText)) filtered.Add(choice);
                    }

                    if (FilteredChoices.Length != filtered.Count)
                    {
                        if (SelectionMode != SelectionMode.None)
                        {
                            var newIndexOfHighlightedItem = filtered.IndexOf(Choices[HighlightedIndex]);
                            if (newIndexOfHighlightedItem > -1) HighlightedIndex = newIndexOfHighlightedItem;
                            else HighlightedIndex = 0;

                            if (HighlightedIndex + PageSize > filtered.Count)
                                ScrollIndexOffset = Math.Max(0, filtered.Count - PageSize);
                            else ScrollIndexOffset = HighlightedIndex;
                        }
                        else ScrollIndexOffset = 0;
                    }

                    FilteredChoices = filtered.ToArray();
                }
            }
        }

        private string[] HandleEnterPress(Func<string> outputTransformerOnOneOrMoreItems, Func<string[]> returnValueTransformer)
        {
            if (FilteredChoices.Length == 0)
            {
                if (AllowEmptyInput)
                {
                    ConsoleHelpers.WriteWholeLine("nothing selected".Pastel(Constants.INFO_TEXT));
                    return new string[0];
                }
                else return null;
            }

            ConsoleHelpers.WriteWholeLine(outputTransformerOnOneOrMoreItems().Pastel(Constants.ACTIVE_TEXT_COLOR));
            return returnValueTransformer();
        }

        private void PrintList()
        {
            Console.CursorTop = CursorTopForListStart;
            Console.CursorLeft = 0;
            for (int i = 0; i < PageSize + 2; ++i) ConsoleHelpers.WriteWholeLine();

            Console.CursorLeft = CursorLeftForSearchText;
            Console.CursorTop = CursorTopForListStart - 1;
            if (SearchText == "")
            {
                ConsoleHelpers.WriteWholeLine("type to search".Pastel(Constants.INFO_TEXT));
            }
            else
            {
                ConsoleHelpers.WriteWholeLine($"{SearchText.Pastel(Constants.SEARCH_TEXT)}{(SelectionMode == SelectionMode.Many && SelectedItems.Any(x => !FilteredChoices.Contains(x)) ? " (some selected items are being filtered)" : "").Pastel(Constants.INFO_TEXT)}");
            }
            Console.CursorTop = CursorTopForListStart;
            Console.CursorLeft = 0;

            if (FilteredChoices.Length >= PageSize)
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
                if (ScrollIndexOffset + PageSize < FilteredChoices.Length)
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

            if (FilteredChoices.Length > 0)
            {
                for (int i = ScrollIndexOffset; i < Math.Min(PageSize, FilteredChoices.Length) + ScrollIndexOffset; ++i)
                {
                    string toDisplay = $"{(i == HighlightedIndex && SelectionMode != SelectionMode.None ? ">" : " ")}{(SelectionMode == SelectionMode.Many && SelectedItems.Contains(FilteredChoices[i]) ? "→" : " ")}{FilteredChoices[i]}";
                    if (SelectionMode != SelectionMode.None) toDisplay = toDisplay.Pastel(i == HighlightedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR);
                    ConsoleHelpers.WriteWholeLine(toDisplay);
                }
            }
            else
            {
                ConsoleHelpers.WriteWholeLine("(no items)".Pastel(Constants.INFO_TEXT));
            }
        }
    }
}