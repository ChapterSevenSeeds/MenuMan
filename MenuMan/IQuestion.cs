using System;
using System.Collections.Generic;

namespace MenuMan
{
    public interface IQuestion
    {
        Type ReturnType { get; }
        string Key { get; }
        string QuestionText { get; }
        object Ask();
        Func<Dictionary<string, object>, bool> Condition { get; }
    }
}
