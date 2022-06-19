using MenuMan.Inputs;
using System;
using System.Collections.Generic;

namespace MenuMan
{
    using Condition = Func<Dictionary<string, object>, bool>;
    public static class Questions
    {
        public static IQuestion TextInput(string key, string questionText, Condition condition = null, bool allowEmptyInput = false, string defaultValue = "") => new TextInput(key, questionText, allowEmptyInput, defaultValue, condition);
        public static IQuestion ListInput(string key, string questionText, string[] options, Condition condition = null, bool allowEmptyInput = false, string defaultValue = "", int pageSize = 10) => new ListInput(key, questionText, options, allowEmptyInput, defaultValue, pageSize, condition);
        public static IQuestion CheckboxInput(string key, string questionText, string[] options, Condition condition = null, bool allowEmptyInput = false, string[] defaultValue = null, int pageSize = 10) => new CheckboxInput(key, questionText, options, allowEmptyInput, defaultValue, pageSize, condition);
        public static IQuestion ConfirmInput(string key, string questionText, Condition condition = null, YesNo defaultValue = YesNo.Yes) => new ConfirmInput(key, questionText, defaultValue, condition);
        public static IQuestion NumberInput<T>(string key, string questionText, Condition condition = null, object defaultValue = null) where T : struct => new NumberInput<T>(key, questionText, defaultValue, condition);
    }
}
