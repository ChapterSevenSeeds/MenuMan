using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MenuMan
{
    public class Menu<T> where T : new()
    {
        public IQuestion[] Questions { get; }
        private readonly Dictionary<string, PropertyInfo> _resultPropertiesByKey;
        private readonly T _resultsObject;
        public Menu(params IQuestion[] questions)
        {
            Questions = questions;

            _resultPropertiesByKey = new Dictionary<string, PropertyInfo>();
            foreach (IQuestion question in Questions)
            {
                _resultPropertiesByKey.Add(question.Key, typeof(T).GetProperty(question.Key));
            }

            _resultsObject = new T();
        }

        public T Go()
        {
            foreach (IQuestion question in Questions)
            {
                Console.Write("? ".Pastel("#98C37A"));
                Console.Write(question.QuestionText.Pastel("#ABB2BF"));
                Console.Write(" ");
                _resultPropertiesByKey[question.Key].SetValue(_resultsObject, question.Ask(), null);
            }

            return _resultsObject;
        }
    }
}
