using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XperiCode.Parsers.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void EndOfFile_Should_Return_True_When_Last_Position_Of_Input_Reached()
        {
            string input = string.Empty;
            var parser = new Parser(input);

            bool eof = parser.EndOfFile();

            Assert.IsTrue(eof);
        }

        [TestMethod]
        public void ConsumeCharacter_Should_Return_Current_Character()
        {
            string input = "a";
            var parser = new Parser(input);

            char currentCharacter = parser.ConsumeCharacter();

            Assert.AreEqual('a', currentCharacter);
        }

        [TestMethod]
        public void ConsumeCharacter_Should_Increase_Parser_Position()
        {
            string input = "a";
            var parser = new Parser(input);

            parser.ConsumeCharacter();

            Assert.AreEqual(1, parser.Position);
        }
    }
}
