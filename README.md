# MenuMan

### By Tyson Jones

#### A simple terminal-style menu for obtaining user input. Inspried heavily by [inquirer](https://www.npmjs.com/package/inquirer).

# Usage
```cs
var menu = new Menu(
    Questions.NumberInput<int>("money", "How much money do you have?", defaultValue: 33),
    Questions.TextInput("lastName", "What is your last name?", defaultValue: "Tyson"),
    Questions.ListInput("favoriteFood", "What is your favorite food?", availableFoods),
    Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", availableSongs),
    Questions.ConfirmInput("isWeird", "Are you weird?"));
    
var answers = menu.Go();
Console.WriteLine(answers.Get<int>("money")); // Prints 33 or whatever I typed in.
// Or you can cast the result manually.
Console.WriteLine((int)answers["money"]);
```

# API
- ## `Menu` class
  Represents a menu object with a fixed number of questions. Constructed by the only available constructor.
  ## Constructor Signature
  ```cs
  public Menu(params IQuestion[] questions)
  ```
  ## Methods
  - `Go`
    - Shows the menu. Each question handed to the constructor is asked in the order they were handed to the constructor.
    - Returns a `Dictionary<string, object>` where the keys represents the question keys and the values represent the user's responses.
- ## `NumberInput<T>` class
  Represents a simple single-line text input that filters out all non-numeric keys and only accepts valid input for the given numeric type. Constructed by the `Questions.NumericInput<T>` method.

  The answers object type is `T`.
  ## Signature
  ```cs
  public static IQuestion NumberInput<T>(string key, string questionText, Func<Dictionary<string, object>, bool> condition = null, T? defaultValue = null) where T : struct;
  ```
  ## Parameters
  - `T` - The numeric type desired to be used for parsing and validation.
  - `key` - The question key.
  - `questionText` - The question message to display to the user.
  - `condition` - *Optional*. A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
  - `defaultValue` - *Optional*. The default value of `T` to prompt to the user.
- ## `ConfirmInput` class
  Represents a boolean input in the form of Yes/No. Constructed by the `Questions.ConfirmInput` method.

  The answers object type is `YesNo`.
  ## Signature
  ```cs
  public static IQuestion ConfirmInput(string key, string questionText, Func<Dictionary<string, object>, bool> condition = null, YesNo defaultValue = YesNo.Yes);
  ```
  ## Parameters
  - `key` - The question key.
  - `questionText` - The question message to display to the user.
  - `condition` - *Optional*. A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
  - `defaultValue` - *Optional*. This value will be selected if the user presses enter immediately after prompted for input.
- ## `CheckboxInput` class
  Represents a listbox for selecting multiple items. Constructed by the `Questions.NumericInput` method.

  The answers object type is `string[]`.
  ## Signature
  ```cs
  public static IQuestion CheckboxInput(string key, string questionText, string[] options, Func<Dictionary<string, object>, bool> condition = null, bool allowEmptyInput = false, string[] defaultValue = null, int pageSize = 10, bool disableSearch = false, bool showSearchOptions = true);
  ```
  ## Parameters
    - `key` - The question key.
    - `questionText` - The question message to display to the user.
    - `options` - The items to display in the listbox.
    - `condition` - A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
    - `allowEmptyInput` - If true, an empty string array can be returned if the user selected no items.
    - `defaultValue` - The items that will start out selected when the listbox is shown.
    - `pageSize` - An options array larger than this number will force the user to scroll to see all available choices.
    - `disableSearch` - If true, the user will be unable to filter the choices.
    - `showSearchOptions` - If false, the helper lines shown at the bottom of the terminal showing your current search mode and some selection options will not be shown.
- ## `ListInput` class
  Represents a listbox for selecting a single item. Constructed by the `Questions.ListInput` method.
  
  The answers object type is `string`.
  ## Signature
  ```cs
  public static IQuestion ListInput(string key, string questionText, string[] options, Func<Dictionary<string, object>, bool> condition = null, bool allowEmptyInput = false, string defaultValue = "", int pageSize = 10, bool disableSearch = false, bool showSearchOptions = true);
  ```
  ## Parameters
  - `key` - The question key.
  - `questionText` - The question message to display to the user.
  - `options` - The items to display in the listbox.
  - `condition` - *Optional*. A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
  - `allowEmptyInput` - *Optional*. If true, null can be returned if the user's search term has filtered out all available items.
  - `defaultValue` - *Optional*. The default selection to highlight when the listbox is shown. If this item isn't present in the options array, the first item in the options array will be highlighted.
  - `pageSize` - *Optional*. An options array larger than this number will force the user to scroll to see all available choices.
  - `disableSearch` - *Optional*. If true, the user will be unable to filter the choices.
  - `showSearchOptions` - *Optional*. If false, the helper line shown at the bottom of the terminal showing your current search mode will not be shown.
- ## `TextInput` class
  Represents a simple single-line text input. Constructed by the `Questions.TextInput` method.

  The answers object type is `string`.
  ## Signature
  ```cs
  public static IQuestion TextInput(string key, string questionText, Func<Dictionary<string, object>, bool> condition = null, bool allowEmptyInput = false, string defaultValue = "");
  ```
  ## Parameters
  - `key` - The question key.
  - `questionText` - The question message to display to the user.
  - `condition` - *Optional*. A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
  - `allowEmptyInput` - *Optional*. If true, the empty string is accepted as valid input.
  - `defaultValue` - *Optional*. The default string to prompt to the user.

- ## `Get` extension method
  Attempts to retrieve the value at the specified key in the answers dictionary, casting the result to T if successful and returning the default value for T if unsuccessful. If the cast is unsuccessful, an exception will be thrown.
  ## Signature
  ```cs
  public static T Get<T>(this IDictionary<string, object> dictionary, string key)
  ```
  ## Parameters
  - `T` - The type to which to cast the result.
  - `dictionary` - The input dictionary.
  - `key` - The key.