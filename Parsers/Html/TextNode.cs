namespace XperiCode.Parsers.Html
{
    public class TextNode : Node
    {
        public string Text { get; private set; }

        public TextNode(string text) : base(NodeType.Text)
        {
            this.Text = text;
            this.Children = new Node[] { };
        }
    }
}
