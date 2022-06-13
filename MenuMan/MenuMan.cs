﻿using Pastel;
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
                if (_resultPropertiesByKey[question.Key].PropertyType != question.ReturnType)
                {
                    throw new InvalidCastException($"Return type of question with key '{question.Key}' does not match the type of the corresponding response property.");
                }
            }

            _resultsObject = new T();
        }

        public T Go()
        {
            Console.CursorVisible = false;

            foreach (IQuestion question in Questions)
            {
                Console.Write("? ".Pastel(Constants.QUESTION_MARKER_COLOR));
                Console.Write(question.QuestionText.Pastel(Constants.REGULAR_TEXT_COLOR));
                Console.Write(" ");
                _resultPropertiesByKey[question.Key].SetValue(_resultsObject, question.Ask(), null);
            }

            Console.CursorVisible = true;

            return _resultsObject;

        }
    }
}
