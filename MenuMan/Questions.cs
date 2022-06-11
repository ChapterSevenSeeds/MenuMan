using MenuMan.Inputs;

namespace MenuMan
{
    public static class Questions
    {
        public static IQuestion TextInput(string key, string questionText) => new TextInput { Key = key, QuestionText = questionText };
    }
}
