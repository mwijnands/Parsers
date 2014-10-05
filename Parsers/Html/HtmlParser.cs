using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XperiCode.Parsers.Html
{
    public class HtmlParser : Parser
    {
        public HtmlParser(string input) : base(input)
        {
        }

        public string ParseTagName()
        {
            return this.ConsumeWhile(c =>
                {
                    return (c >= 'a' && c <= 'z')
                        || (c >= 'A' && c <= 'Z')
                        || (c >= '0' && c <= '9');
                });
        }

        public string ParseAttributeName()
        {
            return this.ConsumeWhile(c =>
                {
                    return (c >= 'a' && c <= 'z')
                        || (c >= 'A' && c <= 'Z')
                        || (c >= '0' && c <= '9')
                        || (c == '-');
                });
        }

        public Node ParseNode()
        {
            return this.NextCharacter() == '<'
                ? (Node)this.ParseElement()
                : (Node)this.ParseText();
        }

        public ElementNode ParseElement()
        {
            Debug.Assert(this.ConsumeCharacter() == '<');
            string tagName = this.ParseTagName();
            //var attributes = this.ParseAttributes();
            //Debug.Assert(this.ConsumeCharacter() == '>');

            // TODO: Children.

            // TODO: Closing tag.

            return new ElementNode(tagName);
        }

        public TextNode ParseText()
        {
            return new TextNode(this.ConsumeWhile(c => c != '<'));
        }
    }
}
