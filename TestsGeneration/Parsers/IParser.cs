using TestsGeneration.Data;

namespace TestsGeneration.Parsers
{
    public interface IParser
    {
        IEnumerable<ParsedClass> Parse(string code);
    }
}
