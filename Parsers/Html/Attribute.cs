namespace XperiCode.Parsers.Html
{
    public class Attribute
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public Attribute(string name)
        {
            this.Name = name;
        }

        public Attribute(string name, string value) : this(name)
        {
            this.Value = value;
        }
    }
}
