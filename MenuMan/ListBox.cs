using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MenuMan
{
    internal enum SearchMode
    {
        StartsWith,
        Contains,
        EndsWith,
        Regex
    }

    public enum SelectionMode
    {
        /// <summary>
        /// Forces the user to select just one item in the list. The user can select no items if empty input is allowed, searching is not disabled, and their search term has filtered out all of the available choices.
        /// </summary>
        Single,
        /// <summary>
        /// Allows the user to select more than one item. The user can choose no items if empty input is allowed.
        /// </summary>
        Many,
        /// <summary>
        /// The user cannot select any items. Upon exiting the listbox, all the filtered items are returned.
        /// </summary>
        None
    }

    public class ListBox
    {
        private static readonly string MANY_SELECT_HELPER_STRING = "Selection [Ctrl+]: [I] Invert [R] Remove Filtered [Q] Add Filtered [T] To Filtered".Pastel("#000000");
        private static readonly int MANY_SELECT_HELPER_STRING_LENGTH = MANY_SELECT_HELPER_STRING.RawStringLength();
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
        private bool DisableSearch;
        private SearchMode SearchMode = SearchMode.Contains;
        private string SearchError = "";

        /// <summary>
        /// Constructs a new listbox.
        /// </summary>
        /// <param name="prompt">The prompt text to show to the user before printing the list of choices.</param>
        /// <param name="choices">The options available to the user.</param>
        /// <param name="selectionMode">The selection mode.</param>
        /// <param name="allowEmptyInput">If true, the user can elect to not choose any items and return from the listbox. If false, the user cannot leave the listbox without first choosing at least one item except when the selection mode is set to None in which case this parameter is redundant.</param>
        /// <param name="pageSize">The page size for the list.</param>
        /// <param name="disableSearch">If true, filtering will be disabled.</param>
        /// <param name="defaultValue">The default selections. If the selection mode is set to Single, the default selection cannot have a length greater than 1. If the selection mode is set to None, the default selection cannot have a length greater than 0.</param>
        /// <param name="showSearchModeSelection">If false, the helper lines printed at the bottom of the terminal for filtering and selections will not be shown.</param>
        /// <exception cref="ArgumentException"></exception>
        public ListBox(string prompt, string[] choices, SelectionMode selectionMode, bool allowEmptyInput, int pageSize, bool disableSearch = false, string[] defaultValue = null, bool showSearchModeSelection = true)
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
            DisableSearch = disableSearch;
            ShowSearchModeSelection = showSearchModeSelection;

            if (SelectionMode == SelectionMode.Single && defaultValue?.Length > 1) throw new ArgumentException("SelectionMode.Single and a default selection of more than one item is not allowed.");
            if (SelectionMode == SelectionMode.None && defaultValue?.Length > 0) throw new ArgumentException("SelectionMode.None cannot have a default selection.");
            
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

            if (SelectionMode == SelectionMode.None) HighlightedIndex = -1;

            if (selectionMode == SelectionMode.Many && Console.WindowWidth < MANY_SELECT_HELPER_STRING_LENGTH && ShowSearchModeSelection) Console.SetWindowSize(MANY_SELECT_HELPER_STRING_LENGTH, Console.WindowHeight);
        }

        /// <summary>
        /// Shows the listbox at the current terminal position.
        /// </summary>
        /// <returns>A string array of the results. If the selection mode was set to Single, this array will either have 0 or 1 items in it.</returns>
        public string[] Show()
        {
            if (Prompt != "" && Prompt != null) Console.Write($"{Prompt} ");

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
                    case ConsoleKey.Tab:
                        if (SelectionMode == SelectionMode.Many)
                        {
                            if (SelectedItems.Contains(FilteredChoices[HighlightedIndex])) SelectedItems.Remove(FilteredChoices[HighlightedIndex]);
                            else SelectedItems.Add(FilteredChoices[HighlightedIndex]);
                        }

                        break;
                    case ConsoleKey.Enter:
                        Console.CursorLeft = 0;
                        Console.CursorTop = CursorTopForListStart;
                        for (int i = 0; i < PageSize + 2; ++i) ConsoleHelpers.WriteWholeLine();

                        Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
                        Console.CursorLeft = 0;
                        ConsoleHelpers.WriteWholeLine(withNewLine: false);
                        ++Console.CursorTop;
                        Console.CursorLeft = 0;
                        ConsoleHelpers.WriteWholeLine(withNewLine: false);

                        Console.CursorTop = CursorTopForListStart - 1;
                        Console.CursorLeft = CursorLeftForSearchText;

                        string[] returnValue;
                        if (SelectionMode == SelectionMode.Single) returnValue = HandleEnterPress(() => FilteredChoices[HighlightedIndex], () => new string[] { FilteredChoices[HighlightedIndex] }, FilteredChoices);
                        else if (SelectionMode == SelectionMode.Many) returnValue = HandleEnterPress(() => string.Join(",", SelectedItems), () => SelectedItems.ToArray(), SelectedItems);
                        else returnValue = HandleEnterPress(() => $"{FilteredChoices.Length} items selected", () => FilteredChoices, FilteredChoices);

                        if (returnValue == null) break;
                        return returnValue;
                    case ConsoleKey.Backspace:
                        if (SearchText.Length > 0) SearchText = SearchText.Substring(0, SearchText.Length - 1);
                        break;
                    case ConsoleKey.I: // Invert selection
                        if (lastPress.Modifiers == ConsoleModifiers.Control && SelectionMode == SelectionMode.Many)
                        {
                            var selectedClone = new HashSet<string>(SelectedItems);
                            SelectedItems.Clear();
                            foreach (var item in Choices)
                            {
                                if (!selectedClone.Contains(item)) SelectedItems.Add(item);
                            }
                        }
                        else SearchText += lastPress.KeyChar;
                        break;
                    case ConsoleKey.R: // Remove the filtered items from the selection.
                        if (lastPress.Modifiers == ConsoleModifiers.Control && SelectionMode == SelectionMode.Many)
                        {
                            foreach (var item in FilteredChoices) SelectedItems.Remove(item);
                        } 
                        else SearchText += lastPress.KeyChar;
                        break;
                    case ConsoleKey.Q: // Add the filtered items to the selection.
                        if (lastPress.Modifiers == ConsoleModifiers.Control && SelectionMode == SelectionMode.Many)
                        {
                            foreach (var item in FilteredChoices) SelectedItems.Add(item);
                        }
                        else SearchText += lastPress.KeyChar;
                        break;
                    case ConsoleKey.T: // Change the selected items to the filtered items.
                        if (lastPress.Modifiers == ConsoleModifiers.Control && SelectionMode == SelectionMode.Many)
                        {
                            SelectedItems.Clear();
                            foreach (var item in FilteredChoices) SelectedItems.Add(item);
                        }
                        else SearchText += lastPress.KeyChar;
                        break;
                    case ConsoleKey.S: // Change the search mode.
                        if (lastPress.Modifiers == ConsoleModifiers.Control)
                        {
                            SearchMode = (SearchMode)((int)(SearchMode + 1) % 4);
                        }
                        else SearchText += lastPress.KeyChar;
                        break;
                    default:
                        if (!DisableSearch && !char.IsControl(lastPress.KeyChar)) SearchText += lastPress.KeyChar;
                        break;
                }

                if (!DisableSearch)
                {
                    if (SearchText == "")
                    {
                        FilteredChoices = new string[Choices.Length];
                        Array.Copy(Choices, FilteredChoices, Choices.Length);
                    }
                    else
                    {
                        Regex searchRegex = new Regex(".");
                        switch (SearchMode)
                        {
                            case SearchMode.StartsWith:
                                searchRegex = new Regex($"^{Regex.Escape(SearchText)}", RegexOptions.IgnoreCase);
                                break;
                            case SearchMode.Contains:
                            default:
                                searchRegex = new Regex($"{Regex.Escape(SearchText)}", RegexOptions.IgnoreCase);
                                break;
                            case SearchMode.EndsWith:
                                searchRegex = new Regex($"{Regex.Escape(SearchText)}$", RegexOptions.IgnoreCase);
                                break;
                            case SearchMode.Regex:
                                try
                                {
                                    searchRegex = new Regex(SearchText, RegexOptions.IgnoreCase);
                                    SearchError = "";
                                }
                                catch (Exception)
                                {
                                    SearchError = " Invalid regular expression";
                                }
                                break;
                        }

                        var filtered = Choices.Where(x => searchRegex.IsMatch(x)).ToList();

                        if (!FilteredChoices.SequenceEqual(filtered))
                        {
                            if (SelectionMode != SelectionMode.None)
                            {
                                // If the list has a highlighted item, try to find it in the new filtered list. If we can't find it, just set it to the top of the list.
                                var newIndexOfHighlightedItem = filtered.IndexOf(Choices[HighlightedIndex]);
                                if (newIndexOfHighlightedItem > -1) HighlightedIndex = newIndexOfHighlightedItem;
                                else HighlightedIndex = 0;

                                // Set the scroll top to whatever the highlighted index is (so that it appears at the top).
                                // If there aren't enough items to do that, scroll to the top.
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
        }

        /// <summary>
        /// Handler for the enter keypress (implementation for each selection mode is different).
        /// </summary>
        /// <param name="outputTransformerOnOneOrMoreItems">Transforms the user selection into a string to be printed (to summarize their selection) upon exiting the listbox.</param>
        /// <param name="returnValueTransformer">Generates the proper return value to hand back upon exiting the listbox.</param>
        /// <param name="relevantItemContainer">The enumerable used to determine if the user's selection is empty.</param>
        /// <returns></returns>
        private string[] HandleEnterPress(Func<string> outputTransformerOnOneOrMoreItems, Func<string[]> returnValueTransformer, IEnumerable<string> relevantItemContainer)
        {
            if (relevantItemContainer.Count() == 0)
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

        /// <summary>
        /// Prints the list items.
        /// </summary>
        private void PrintList()
        {
            if (!DisableSearch)
            {
                // Update the search text
                Console.CursorLeft = CursorLeftForSearchText;
                Console.CursorTop = CursorTopForListStart - 1;

                if (SearchText == "") ConsoleHelpers.WriteWholeLine("type to search".Pastel(Constants.INFO_TEXT));
                else ConsoleHelpers.WriteWholeLine($"{SearchText.Pastel(Constants.SEARCH_TEXT)}{(SearchError != "" ? SearchError.Pastel(Constants.INFO_TEXT) : "")}{(SelectionMode == SelectionMode.Many && SelectedItems.Any(x => !FilteredChoices.Contains(x)) && SearchError == "" ? " (some selected items are being filtered)" : "").Pastel(Constants.INFO_TEXT)}");
            }

            Console.CursorTop = CursorTopForListStart;
            Console.CursorLeft = 0;

            // If we can scroll up, tell the user.
            if (ScrollIndexOffset > 0) ConsoleHelpers.WriteWholeLine($"  (more {HelperText})".Pastel(Constants.INFO_TEXT));

            // Print the list with the appropriate decorations (highlighted index, single selection, multi selection, etc) if there are items to print.
            if (FilteredChoices.Length > 0)
            {
                for (int i = ScrollIndexOffset; i < Math.Min(PageSize, FilteredChoices.Length) + ScrollIndexOffset; ++i)
                {
                    ConsoleHelpers.WriteWholeLine($"{$"{(i == HighlightedIndex && SelectionMode != SelectionMode.None ? ">" : " ")}{(SelectionMode == SelectionMode.Many && SelectedItems.Contains(FilteredChoices[i]) ? "→" : " ")}{FilteredChoices[i]}".Pastel(i == HighlightedIndex ? Constants.ACTIVE_TEXT_COLOR : Constants.REGULAR_TEXT_COLOR)}{(SelectionMode == SelectionMode.Many && i == HighlightedIndex ? " press TAB to select/deselect".Pastel(Constants.INFO_TEXT) : "")}");
                }
            }
            else ConsoleHelpers.WriteWholeLine("(no items)".Pastel(Constants.INFO_TEXT)); // If no items, tell the user.

            // If we can scroll down, tell the user.
            if (ScrollIndexOffset + PageSize < FilteredChoices.Length) ConsoleHelpers.WriteWholeLine($"  (more {HelperText})".Pastel(Constants.INFO_TEXT), false);

            // Clear the rest of the terminal lines from any pre-existing text that could have been there before.
            for (int i = Console.CursorTop - CursorTopForListStart; i <= PageSize + 2; ++i) ConsoleHelpers.WriteWholeLine();

            if (ShowSearchModeSelection) PrintSearchOptions();
        }

        /// <summary>
        /// Prints the search and selection helper text lines.
        /// </summary>
        private void PrintSearchOptions()
        {
            if (SelectionMode == SelectionMode.Many)
            {
                Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
                ConsoleHelpers.WriteWholeLine(MANY_SELECT_HELPER_STRING, false, backColor: "#fffffff");
            }
            
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Console.CursorLeft = 0;

            ConsoleHelpers.WriteWholeLine($"Search Mode [Ctrl+S]: [{(SearchMode == SearchMode.StartsWith ? "X" : "")}] Starts with [{(SearchMode == SearchMode.Contains ? "X" : "")}] Contains [{(SearchMode == SearchMode.EndsWith ? "X" : "")}] Ends With [{(SearchMode == SearchMode.Regex ? "X" : "")}] Regex".Pastel("#000000"), false, backColor: "#ffffff");
        }
    }
}