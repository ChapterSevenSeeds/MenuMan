using MenuMan.Inputs;
using System;
using System.Collections.Generic;

namespace MenuMan
{
    using Condition = Func<Dictionary<string, object>, bool>;

    /// <summary>
    /// A static class for shorthand question creation (with some default parameters).
    /// </summary>
    public static class Questions
    {
        /// <summary>
        /// Represents a simple single-line text input.
        /// 
        /// The answers object type is string.
        /// </summary>
        /// <param name="key">The question key.</param>
        /// <param name="questionText">The question message to display to the user.</param>
        /// <param name="condition">A predicate to determine if the question should be asked. The current answers are passed as the only parameter.</param>
        /// <param name="allowEmptyInput">If true, the empty string is accepted as valid input.</param>
        /// <param name="defaultValue">The default string to prompt to the user.</param>
        /// <returns>A new TextInput object.</returns>
        public static IQuestion TextInput(string key, string questionText, Condition condition = null, bool allowEmptyInput = false, string defaultValue = "") => new TextInput(key, questionText, allowEmptyInput, defaultValue, condition);

        /// <summary>
        /// Represents a listbox for selecting a single item.
        /// 
        /// The answers object type is string.
        /// </summary>
        /// <param name="key">The question key.</param>
        /// <param name="questionText">The question message to display to the user.</param>
        /// <param name="options">The items to display in the listbox.</param>
        /// <param name="condition">A predicate to determine if the question should be asked. The current answers are passed as the only parameter.</param>
        /// <param name="allowEmptyInput">If true, null can be returned if the user's search term has filtered out all available items.</param>
        /// <param name="defaultValue">The default selection to highlight when the listbox is shown. If this item isn't present in the options array, the first item in the options array will be highlighted.</param>
        /// <param name="pageSize">An options array larger than this number will force the user to scroll to see all available choices.</param>
        /// <param name="disableSearch">If true, the user will be unable to filter the choices.</param>
        /// <param name="showSearchOptions">If false, the helper line shown at the bottom of the terminal showing your current search mode will not be shown.</param>
        /// <returns>A new ListInput object.</returns>
        public static IQuestion ListInput(string key, string questionText, string[] options, Condition condition = null, bool allowEmptyInput = false, string defaultValue = "", int pageSize = 10, bool disableSearch = false, bool showSearchOptions = true) => new ListInput(key, questionText, options, allowEmptyInput, defaultValue, pageSize, condition, disableSearch, showSearchOptions);

        /// <summary>
        /// Represents a listbox for selecting multiple items.
        /// 
        /// The answers object type is string[].
        /// </summary>
        /// <param name="key">The question key.</param>
        /// <param name="questionText">The question message to display to the user.</param>
        /// <param name="options">The items to display in the listbox.</param>
        /// <param name="condition">A predicate to determine if the question should be asked. The current answers are passed as the only parameter.</param>
        /// <param name="allowEmptyInput">If true, an empty string array can be returned if the user selected no items.</param>
        /// <param name="defaultValue">The items that will start out selected when the listbox is shown.</param>
        /// <param name="pageSize">An options array larger than this number will force the user to scroll to see all available choices.</param>
        /// <param name="disableSearch">If true, the user will be unable to filter the choices.</param>
        /// <param name="showSearchOptions">If false, the helper lines shown at the bottom of the terminal showing your current search mode and some selection options will not be shown.</param>
        /// <returns>A new CheckboxInput object.</returns>
        public static IQuestion CheckboxInput(string key, string questionText, string[] options, Condition condition = null, bool allowEmptyInput = false, string[] defaultValue = null, int pageSize = 10, bool disableSearch = false, bool showSearchOptions = true) => new CheckboxInput(key, questionText, options, allowEmptyInput, defaultValue, pageSize, condition, disableSearch, showSearchOptions);

        /// <summary>
        /// Represents a boolean input in the form of Yes/No.
        /// 
        /// The answers object type is YesNo.
        /// </summary>
        /// <param name="key">The question key.</param>
        /// <param name="questionText">The question message to display to the user.</param>
        /// <param name="condition">A predicate to determine if the question should be asked. The current answers are passed as the only parameter.</param>
        /// <param name="defaultValue">This value will be selected if the user presses enter immediately after prompted for input.</param>
        /// <returns>A new ConfirmInput object.</returns>
        public static IQuestion ConfirmInput(string key, string questionText, Condition condition = null, YesNo defaultValue = YesNo.Yes) => new ConfirmInput(key, questionText, defaultValue, condition);

        /// <summary>
        /// Represents a simple single-line text input that filters out all non-numeric keys and only accepts valid input for the given numeric type.
        /// 
        /// The answers object type is T.
        /// </summary>
        /// <typeparam name="T">The numeric type desired to be used for parsing and validation.</typeparam>
        /// <param name="key">The question key.</param>
        /// <param name="questionText">The question message to display to the user.</param>
        /// <param name="condition">A predicate to determine if the question should be asked. The current answers are passed as the only parameter.</param>
        /// <param name="defaultValue">The default value of T to prompt to the user.</param>
        /// <returns>A new NumberInput object.</returns>
        /// <exception cref="ArgumentException">Thrown if the type T is not a numeric type.</exception>
        public static IQuestion NumberInput<T>(string key, string questionText, Condition condition = null, T? defaultValue = null) where T : struct => new NumberInput<T>(key, questionText, defaultValue, condition);
    }
}
