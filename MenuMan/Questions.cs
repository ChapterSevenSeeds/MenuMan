using MenuMan.Inputs;

namespace MenuMan
{
    public static class Questions
    {
        public static IQuestion TextInput(string key, string questionText, string defaultValue) => new TextInput(key, questionText, defaultValue);
        public static IQuestion ListInput(string key, string questionText, string[] options, string defaultValue, int pageSize = 10) => new ListInput(key, questionText, options, defaultValue, pageSize);
        public static IQuestion CheckboxInput(string key, string questionText, string[] options, string[] defaultValue, int pageSize = 10) => new CheckboxInput(key, questionText, options, defaultValue, pageSize);
        public static IQuestion ConfirmInput(string key, string questionText, YesNo defaultValue) => new ConfirmInput(key, questionText, defaultValue);
        public static IQuestion NumberInput(string key, string questionText, NumberInputType numberType, object defaultValue) => new NumberInput(key, questionText, numberType, defaultValue);
    }
}
