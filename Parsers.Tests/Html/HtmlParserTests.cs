using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XperiCode.Parsers.Html;

namespace XperiCode.Parsers.Tests.Html
{
    [TestClass]
    public class HtmlParserTests
    {
        [TestMethod]
        public void ParseTagName_Should_Stop_At_NonNumericAndNonAlphaNumeric_Character()
        {
            string input = "h1>";
            var parser = new HtmlParser(input);

            string result = parser.ParseTagName();

            Assert.AreEqual("h1", result);
        }

        [TestMethod]
        public void ParseAttributeName_Should_Stop_At_NonNumericAndNonAlphaNumericAndNonDash_Character()
        {
            string input = "data-test='value'";
            var parser = new HtmlParser(input);

            string result = parser.ParseAttributeName();

            Assert.AreEqual("data-test", result);
        }

        [TestMethod]
        public void ParseNode_Should_Return_One_ElementNode()
        {
            string input = "<html>    </html>";
            var parser = new HtmlParser(input);

            Node result = parser.ParseNode();

            Assert.IsNotNull(result);
            Assert.AreEqual(NodeType.Element, result.Type);
            Assert.IsInstanceOfType(result, typeof(ElementNode));
            Assert.AreEqual("html", ((ElementNode)result).TagName);
        }

        [TestMethod]
        public void ParseNode_Should_Return_Two_ElementNodes()
        {
            string input = "<div>  </div>  <a></a>";
            var parser = new HtmlParser(input);

            Node[] result = parser.ParseNodes();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ElementNode));
            Assert.IsInstanceOfType(result[1], typeof(ElementNode));
            Assert.AreEqual("div", ((ElementNode)result[0]).TagName);
            Assert.AreEqual("a", ((ElementNode)result[1]).TagName);
        }
    }
}
