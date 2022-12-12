namespace TestsGeneration.Data
{
    public class ParsedClass
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<string> Methods { get; set; }


        public ParsedClass(string name, string @namespace, List<string> methods)
        {
            Name = name;
            Namespace = @namespace;
            Methods = methods;
        }

        public ParsedClass()
        {
            Name = string.Empty;
            Namespace = string.Empty;
            Methods = new List<string>();
        }


        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            ParsedClass other = obj as ParsedClass;

            if (other == null)
                return false;

            return Name.Equals(other.Name)
                && Namespace.Equals(other.Namespace)
                && Methods.SequenceEqual(other.Methods);
        }

        public override int GetHashCode() //если переопределение метода иквел, то этот тоже переопределяется
        {
            return HashCode.Combine(Name, Namespace, Methods);
        }
    }
}
