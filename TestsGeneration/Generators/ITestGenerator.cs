using TestsGeneration.Data;

namespace TestsGeneration.Generators
{
    public interface ITestGenerator
    {
        string Generate(ParsedClass parsedClass);

        IEnumerable<string> Generate(IEnumerable<ParsedClass> classes);
    }
}
