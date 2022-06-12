using MenuMan.Inputs;

namespace MenuMan
{
    public static class Questions
    {
        public static IQuestion TextInput(string key, string questionText) => new TextInput(key, questionText);
        public static IQuestion ListInput(string key, string questionText, string[] options) => new ListInput(key, questionText, options);
        public static IQuestion CheckboxInput(string key, string questionText, string[] options) => new CheckboxInput(key, questionText, options);
        public static IQuestion ConfirmInput(string key, string questionText) => new ConfirmInput(key, questionText);
    }
}
