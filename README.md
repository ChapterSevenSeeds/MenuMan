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
```

# API
- ## `Menu` class
  Represents a menu object with a fixed number of questions. Constructed by the only available constructor.
  ## Parameters
  - Array of `IQuestion`, available by the `Questions` static class.
  ## Methods
  - `Go`
    - Shows the menu. Each question handed to the constructor is asked in the order they were handed to the constructor.
    - Returns a `Dictionary<string, object>` where the keys represents the question keys and the values represent the user's responses.
- ## `NumberInput<T>` class
  Represents a simple single-line text input that filters out all non-numeric keys and only accepts valid input for the given numeric type. Constructed by the `Questions.NumericInput<T>` method.

  The answers object type is T.
  ## Parameters
  - `T` - The numeric type desired to be used for parsing and validation.
  - `string key`{:.cs} - The question key.
  - string `questionText` - The question message to display to the user.
  - `condition` - *Optional*. A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
  - `defaultValue` - *Optional*. The default value of `T` to prompt to the user.
- ## `ConfirmInput` class
  Represents a boolean input in the form of Yes/No. Constructed by the `Questions.ConfirmInput` method.

  The answers object type is `YesNo`.
  ## Parameters
  - `key` - The question key.
  - `questionText` - The question message to display to the user.
  - `condition` - *Optional*. A predicate to determine if the question should be asked. The current answers are passed as the only parameter.
  - `defaultValue` - *Optional*. This value will be selected if the user presses enter immediately after prompted for input. If left unspecified, `YesNo.Yes` is used.
- ## `CheckboxInput` class
  Represents a listbox for selecting multiple items. Constructed by the `Questions.NumericInput<T>` method.

  The answers object type is string[].
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