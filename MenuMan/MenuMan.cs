using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuMan
{
    internal static class ConsoleHelpers
    {
        private static HashSet<ConsoleKey> AlphaKeys = new HashSet<ConsoleKey> { ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.I, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.M, ConsoleKey.N, ConsoleKey.O, ConsoleKey.P, ConsoleKey.Q, ConsoleKey.R, ConsoleKey.S, ConsoleKey.T, ConsoleKey.U, ConsoleKey.V, ConsoleKey.W, ConsoleKey.X, ConsoleKey.Y, ConsoleKey.Z };
        private static HashSet<ConsoleKey> NumericKeys = new HashSet<ConsoleKey> { ConsoleKey.D0, ConsoleKey.NumPad0, ConsoleKey.D1, ConsoleKey.NumPad1, ConsoleKey.D2, ConsoleKey.NumPad2, ConsoleKey.D3, ConsoleKey.NumPad3, ConsoleKey.D4, ConsoleKey.NumPad4, ConsoleKey.D5, ConsoleKey.NumPad5, ConsoleKey.D6, ConsoleKey.NumPad6, ConsoleKey.D7, ConsoleKey.NumPad7, ConsoleKey.D8, ConsoleKey.NumPad8, ConsoleKey.D9, ConsoleKey.NumPad9 };
        internal static ConsoleKeyInfo ReadKeyWithSuppression(params ConsoleKey[] keysToSuppress)
        {
            ConsoleKeyInfo key;
            do
            {
                key = ReadAnyKey();
            } while (keysToSuppress.Contains(key.Key));

            return key;
        }

        internal static ConsoleKeyInfo ReadKeyWithSuppressionInverted(params ConsoleKey[] keysToAllow)
        {
            ConsoleKeyInfo key;
            do
            {
                key = ReadAnyKey();
            } while (!keysToAllow.Contains(key.Key));

            return key;
        }

        internal static ConsoleKeyInfo ReadAnyKey()
        {
            return Console.ReadKey(true);
        }

        internal static string ReadString()
        {
            var asdf = "|".Pastel("#98C37A");
            var escaptes = asdf.Split('|');
            Console.Write(escaptes[0]);
            return Console.ReadLine();
        }
    }
    public class Menu
    {
        public Menu()
        {
            IQuestion input = new TextInput();
            input.Print();
            while (true)
            {
                input.HandleInput(ConsoleHelpers.ReadAnyKey());
            }
        }
    }

    internal class TextInput : IQuestion
    {
        private string _value = "";
        void IQuestion.Print()
        {
            Console.Write("? ".Pastel("#98C37A"));
            Console.Write("What is your name? ".Pastel("#ABB2BF"));
        }

        void IQuestion.HandleInput(ConsoleKeyInfo key)
        {
            Console.Write(key.KeyChar.ToString().Pastel("#4AB6C2"));
        }
    }

    internal interface IQuestion
    {
        void Print();
        void HandleInput(ConsoleKeyInfo key);
    }
}
