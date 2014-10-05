namespace XperiCode.Parsers.Html
{
    public class ElementNode : Node
    {
        public string TagName { get; private set; }
        public Attribute[] Attributes { get; private set; }

        public ElementNode(string tagName, Attribute[] attributes, Node[] children) : base(NodeType.Element)
        {
            this.TagName = tagName;
            this.Attributes = attributes;
            this.Children = children;
        }
    }
}
