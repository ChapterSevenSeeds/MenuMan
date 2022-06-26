# MenuMan

### By Tyson Jones

#### A simple terminal-style menu for obtaining user input. Inspried heavily by [inquirer](https://www.npmjs.com/package/inquirer).

## Usage
```cs
var menu = new Menu(
    Questions.NumberInput<int>("money", "How much money do you have?", defaultValue: 33),
    Questions.TextInput("lastName", "What is your last name?", defaultValue: "Tyson"),
    Questions.ListInput("favoriteFood", "What is your favorite food?", availableFoods),
    Questions.CheckboxInput("favoriteSongs", "What are your favorite songs?", availableSongs),
    Questions.ConfirmInput("isWeird", "Are you weird?"));
    
var answers = menu.Go();
```
