using MenuMan.Inputs;

namespace MenuMan
{
    public static class Questions
    {
        public static IQuestion TextInput(string key, string questionText) => new TextInput { Key = key, QuestionText = questionText };
        public static IQuestion ListInput(string key, string questionText, string[] options) => new ListInput { Key = key, QuestionText = questionText, Choices = options };
        public static IQuestion RadioInput(string key, string questionText, string[] options) => new RadioInput { Key = key, QuestionText = questionText, Choices = options };
    }
}
