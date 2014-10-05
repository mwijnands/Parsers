namespace XperiCode.Parsers.Html
{
    public class ElementNode : Node
    {
        public string TagName { get; private set; }
        public Attribute[] Attributes { get; set; }

        public ElementNode(string tagName) : base(NodeType.Element)
        {
            this.TagName = tagName;
        }
    }
}
