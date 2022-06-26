using System;
using System.Collections.Generic;

namespace MenuMan
{
    public interface IQuestion
    {
        /// <summary>
        /// The return type of the question (currently unused).
        /// </summary>
        Type ReturnType { get; }
        /// <summary>
        /// The dictionary key of the question.
        /// </summary>
        string Key { get; }
        /// <summary>
        /// The prompt text for the question.
        /// </summary>
        string QuestionText { get; }
        /// <summary>
        /// The thread blocking function to obtain an answer from the user for this question.
        /// </summary>
        /// <returns></returns>
        object Ask();
        /// <summary>
        /// A predicate that determines if this question should be asked to the user.
        /// </summary>
        Func<Dictionary<string, object>, bool> Condition { get; }
    }
}
