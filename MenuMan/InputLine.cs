using Pastel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MenuMan
{
    internal class InputLine
    {
        public string Text 
        { 
            get => Builder.ToString(); 
            set
            {
                Builder = new StringBuilder(value);
                NegativeCursorOffset = 0;
            }
        }

        private StringBuilder Builder;
        private int StringLeft;
        private int StringTop;
        private string Color;
        private HashSet<char> AllowedStringChars;
        private int NegativeCursorOffset = 0;

        public InputLine(string color, int stringLeft, int stringTop, HashSet<char> allowedStringChars = null, string initialValue = "")
        {
            Color = color;
            Text = initialValue;
            StringLeft = stringLeft;
            StringTop = stringTop;
            AllowedStringChars = allowedStringChars;
        }

        //public void SimulateFocus()
        //{
        //    Console.CursorVisible = true;
        //    Console.CursorTop = StringTop;
        //    Console.CursorLeft = StringLeft + Text.Length;

        //    ConsoleKeyInfo keyPress = Console.ReadKey();

        //    bool needsOnTextChangedInvocation = false;
        //    if (!args.Handled)
        //    {
        //        switch (keyPress.Key)
        //        {
        //            case ConsoleKey.Backspace:
        //                Builder.Remove(Builder.Length - NegativeCursorOffset, 1);
        //                needsOnTextChangedInvocation = true;
        //                break;
        //            case ConsoleKey.LeftArrow:
        //                ++NegativeCursorOffset;
        //                break;
        //            case ConsoleKey.RightArrow:
        //                --NegativeCursorOffset;
        //                break;
        //            default:
        //                if (AllowedStringChars.Contains(keyPress.KeyChar))
        //                {
        //                    Builder.Insert(Builder.Length - NegativeCursorOffset, keyPress.KeyChar);
        //                    needsOnTextChangedInvocation = true;
        //                }
        //                break;

        //        }

        //        if (needsOnTextChangedInvocation)
        //        {
        //            ConsoleHelpers.WriteWholeLine(Text.Pastel(Color));
        //            OnTextChanged?.Invoke(this, new OnTextChangedEventArgs { Text = Text });
        //        }
        //    }
        //}

        public void AppendHelperText(string text)
        {
            ConsoleHelpers.WriteWholeLine($"{Text} {text}");
        }
    }

    public class OnKeyPressEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public ConsoleKeyInfo Key { get; set; }
    }

    public class OnTextChangedEventArgs : EventArgs
    {
        public string Text { get; set; }
    }
}
