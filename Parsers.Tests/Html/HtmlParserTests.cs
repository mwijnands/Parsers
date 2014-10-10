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
            string input = "<h1>";
            var parser = new HtmlParser(input)
                {
                    Position = 1
                };

            string result = parser.ParseTagName();

            Assert.AreEqual("h1", result);
        }

        [TestMethod]
        public void ParseAttributeName_Should_Stop_At_NonNumericAndNonAlphaNumericAndNonDash_Character()
        {
            string input = "<div data-test='value'>";
            var parser = new HtmlParser(input)
                {
                    Position = 5
                };

            string result = parser.ParseAttributeName();

            Assert.AreEqual("data-test", result);
        }

        [TestMethod]
        public void ParseAttribute_Should_Return_One_Attribute_With_Single_Quotes()
        {
            string input = "<div class='myclass'>";
            var parser = new HtmlParser(input)
                {
                    Position = 5
                };

            Attribute result = parser.ParseAttribute();

            Assert.IsNotNull(result);
            Assert.AreEqual("class", result.Name);
            Assert.AreEqual("myclass", result.Value);
        }

        [TestMethod]
        public void ParseAttribute_Should_Return_One_Attribute_With_Double_Quotes()
        {
            string input = "<div class=\"myclass\">";
            var parser = new HtmlParser(input)
                {
                    Position = 5
                };

            Attribute result = parser.ParseAttribute();

            Assert.IsNotNull(result);
            Assert.AreEqual("class", result.Name);
            Assert.AreEqual("myclass", result.Value);
        }

        [TestMethod]
        public void ParseAttributes_Should_Return_Two_Attributes()
        {
            string input = "<a href='#' target=\"_blank\">";
            var parser = new HtmlParser(input)
                {
                    Position = 3
                };

            Attribute[] attributes = parser.ParseAttributes();

            Assert.IsNotNull(attributes);
            Assert.AreEqual(2, attributes.Length);
            Assert.AreEqual("href", attributes[0].Name);
            Assert.AreEqual("#", attributes[0].Value);
            Assert.AreEqual("target", attributes[1].Name);
            Assert.AreEqual("_blank", attributes[1].Value);
        }

        [TestMethod]
        public void ParseNode_Should_Return_One_ElementNode()
        {
            string input = "<html></html>";
            var parser = new HtmlParser(input);

            Node result = parser.ParseNode();

            Assert.IsNotNull(result);
            Assert.AreEqual(NodeType.Element, result.Type);
            Assert.IsInstanceOfType(result, typeof(ElementNode));
            Assert.AreEqual("html", ((ElementNode)result).TagName);
        }

        [TestMethod]
        public void ParseNode_Should_Return_Self_Closing_ElementNode()
        {
            string input = "<br />";
            var parser = new HtmlParser(input);

            Node result = parser.ParseNode();

            Assert.IsNotNull(result);
            Assert.AreEqual(NodeType.Element, result.Type);
            Assert.IsInstanceOfType(result, typeof(ElementNode));
            Assert.AreEqual("br", ((ElementNode)result).TagName);
        }

        [TestMethod]
        public void ParseNodes_Should_Return_Two_ElementNodes()
        {
            string input = "<div></div><a></a>";
            var parser = new HtmlParser(input);

            Node[] result = parser.ParseNodes();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ElementNode));
            Assert.AreEqual(NodeType.Element, result[0].Type);
            Assert.IsInstanceOfType(result[1], typeof(ElementNode));
            Assert.AreEqual(NodeType.Element, result[1].Type);
            Assert.AreEqual("div", ((ElementNode)result[0]).TagName);
            Assert.AreEqual("a", ((ElementNode)result[1]).TagName);
        }

        [TestMethod]
        public void Parse_Should_Return_Html_Root_Node_When_Input_Doesnt_Have_Any()
        {
            string input = "<div></div><a></a>";
            var parser = new HtmlParser(input);

            Node result = parser.Parse();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ElementNode));
            Assert.AreEqual("html", ((ElementNode)result).TagName);
            Assert.AreEqual(2, result.Children.Length);
        }

        [TestMethod]
        public void Parse_Should_Return_Full_Html_Node()
        {
            string input = @"
                            <html>
                            <head>
                                <title>My Title</title>
                            </head>
                            <body>
                                <div class='myclass'>
                                    My Text <a href=""#"" target=""_blank"">My Link</a>
                                    <br />
                                </div>
                            </body>
                            </html>
                            ";
            var parser = new HtmlParser(input);

            Node result = parser.Parse();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ElementNode));

            var html = (ElementNode)result;

            Assert.AreEqual("html", html.TagName);
            Assert.AreEqual(2, html.Children.Length);
            Assert.IsInstanceOfType(html.Children[0], typeof(ElementNode));

            var head = (ElementNode)html.Children[0];

            Assert.AreEqual("head", head.TagName);
            Assert.AreEqual(1, head.Children.Length);
            Assert.IsInstanceOfType(head.Children[0], typeof(ElementNode));

            var title = (ElementNode)head.Children[0];

            Assert.AreEqual("title", title.TagName);
            Assert.AreEqual(1, title.Children.Length);
            Assert.IsInstanceOfType(title.Children[0], typeof(TextNode));

            var titleText = (TextNode)title.Children[0];

            Assert.AreEqual("My Title", titleText.Text);

            Assert.IsInstanceOfType(html.Children[1], typeof(ElementNode));

            var body = (ElementNode)html.Children[1];

            Assert.AreEqual("body", body.TagName);
            Assert.AreEqual(1, body.Children.Length);
            Assert.IsInstanceOfType(body.Children[0], typeof(ElementNode));

            var div = (ElementNode)body.Children[0];

            Assert.AreEqual("div", div.TagName);
            Assert.AreEqual(3, div.Children.Length);
            Assert.AreEqual(1, div.Attributes.Length);
            Assert.AreEqual("class", div.Attributes[0].Name);
            Assert.AreEqual("myclass", div.Attributes[0].Value);
            Assert.IsInstanceOfType(div.Children[0], typeof(TextNode));

            var divText = (TextNode)div.Children[0];

            Assert.AreEqual("My Text ", divText.Text);
            Assert.IsInstanceOfType(div.Children[1], typeof(ElementNode));

            var anchor = (ElementNode)div.Children[1];

            Assert.AreEqual("a", anchor.TagName);
            Assert.AreEqual(1, anchor.Children.Length);
            Assert.AreEqual(2, anchor.Attributes.Length);
            Assert.AreEqual("href", anchor.Attributes[0].Name);
            Assert.AreEqual("#", anchor.Attributes[0].Value);
            Assert.AreEqual("target", anchor.Attributes[1].Name);
            Assert.AreEqual("_blank", anchor.Attributes[1].Value);
            Assert.IsInstanceOfType(anchor.Children[0], typeof(TextNode));

            var anchorText = (TextNode)anchor.Children[0];

            Assert.AreEqual("My Link", anchorText.Text);
            Assert.IsInstanceOfType(div.Children[2], typeof(ElementNode));

            var br = (ElementNode)div.Children[2];

            Assert.AreEqual("br", br.TagName);
            Assert.AreEqual(0, br.Children.Length);
            Assert.AreEqual(0, br.Attributes.Length);
        }
    }
}
