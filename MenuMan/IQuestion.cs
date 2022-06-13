using System;

namespace MenuMan
{
    public interface IQuestion
    {
        Type ReturnType { get; }
        string Key { get; }
        string QuestionText { get; }
        object Ask();
    }
}
