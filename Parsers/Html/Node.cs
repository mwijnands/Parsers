namespace XperiCode.Parsers.Html
{
    public class Node
    {
        public NodeType Type { get; private set; }
        public Node[] Children { get; set; }

        public Node(NodeType type)
        {
            this.Type = type;
        }
    }
}
