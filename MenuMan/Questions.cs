using MenuMan.Inputs;

namespace MenuMan
{
    public static class Questions
    {
        public static IQuestion TextInput(string key, string questionText, bool allowEmptyInput = false, string defaultValue = "") => new TextInput(key, questionText, allowEmptyInput, defaultValue);
        public static IQuestion ListInput(string key, string questionText, string[] options, bool allowEmptyInput = false, string defaultValue = "", int pageSize = 10) => new ListInput(key, questionText, options, allowEmptyInput, defaultValue, pageSize);
        public static IQuestion CheckboxInput(string key, string questionText, string[] options, bool allowEmptyInput = false, string[] defaultValue = null, int pageSize = 10) => new CheckboxInput(key, questionText, options, allowEmptyInput, defaultValue, pageSize);
        public static IQuestion ConfirmInput(string key, string questionText, YesNo defaultValue = YesNo.Yes) => new ConfirmInput(key, questionText, defaultValue);
        public static IQuestion NumberInput<T>(string key, string questionText, object defaultValue = null) where T : struct => new NumberInput<T>(key, questionText, defaultValue);
    }
}
