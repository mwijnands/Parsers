using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace XperiCode.Parsers.Html
{
    public class HtmlParser : Parser
    {
        public HtmlParser(string input)
            : base(input)
        {
        }

        public Node Parse()
        {
            Node[] nodes = this.ParseNodes();

            if (nodes.Length == 1)
            {
                return nodes[0];
            }

            return new ElementNode("html", nodes);
        }

        public Node[] ParseNodes()
        {
            var result = new List<Node>();

            do
            {
                this.ConsumeWhiteSpace();

                if(this.EndOfFile() || this.StartsWith("</"))
                {
                    break;
                }

                result.Add(this.ParseNode());

            } while (true);

            return result.ToArray();
        }

        public Node ParseNode()
        {
            return this.NextCharacter() == '<'
                ? (Node)this.ParseElement()
                : (Node)this.ParseText();
        }

        public TextNode ParseText()
        {
            return new TextNode(this.ConsumeWhile(c => c != '<'));
        }

        public ElementNode ParseElement()
        {
            char theChar;
            string theString;

            theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == '<');

            string tagName = this.ParseTagName();

            var attributes = this.ParseAttributes();

            theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == '>' || theChar == '/');

            if (theChar == '/')
            {
                theChar = this.ConsumeCharacter();
                Debug.Assert(theChar == '>');

                return new ElementNode(tagName, attributes);
            }

            Node[] children = this.ParseNodes();

            theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == '<');

            theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == '/');

            theString = this.ParseTagName();
            Debug.Assert(theString == tagName);

            this.ConsumeWhiteSpace();

            theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == '>');

            return new ElementNode(tagName, attributes, children);
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

        public Attribute[] ParseAttributes()
        {
            var result = new Dictionary<string, Attribute>(StringComparer.OrdinalIgnoreCase);
            
            do
            {
                this.ConsumeWhiteSpace();
                if (this.NextCharacter() == '>' || this.StartsWith("/>"))
                {
                    break;
                }

                Attribute attribute = this.ParseAttribute();

                if (!result.ContainsKey(attribute.Name))
                {
                    result.Add(attribute.Name, attribute);
                }
            } while (true);

            return result.Values.ToArray();
        }

        public Attribute ParseAttribute()
        {
            string name = this.ParseAttributeName();

            this.ConsumeWhiteSpace();

            char theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == '=');

            this.ConsumeWhiteSpace();

            string value = this.ParseAttributeValue();

            return new Attribute(name, value);
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

        public string ParseAttributeValue()
        {
            char openQuote = this.ConsumeCharacter();
            Debug.Assert(openQuote == '\'' || openQuote == '"');

            string value = this.ConsumeWhile(c => c != openQuote);

            char theChar = this.ConsumeCharacter();
            Debug.Assert(theChar == openQuote);

            return value;
        }
    }
}
