using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XperiCode.Parsers
{
    public class Parser
    {
        public int Position { get; protected set; }
        public string Input { get; private set; }

        public Parser(string input)
        {
            this.Position = 0;
            this.Input = input;
        }

        public bool EndOfFile()
        {
            return (this.Position >= this.Input.Length);
        }

        public char NextCharacter()
        {
            return this.Input[this.Position];
        }

        public bool StartsWith(char character)
        {
            return (this.NextCharacter() == character);
        }

        public bool StartsWith(string text)
        {
            return (this.Input.Substring(this.Position).StartsWith(text));
        }

        public char ConsumeCharacter()
        {
            return this.Input[this.Position++];
        }

        public string ConsumeWhile(Func<char, bool> test)
        {
            var result = new List<char>();
            while (!this.EndOfFile() && test(this.NextCharacter()))
            {
                result.Add(this.ConsumeCharacter());
            }
            return new String(result.ToArray());
        }

        public void ConsumeWhiteSpace()
        {
            ConsumeWhile(c => Char.IsWhiteSpace(c));
        }
    }
}
