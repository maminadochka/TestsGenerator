using TestsGeneration.Generators;
using TestsGeneration.Parsers;
using Example.FileToFileGeneration;

namespace Example
{
    internal class Program
    {
        static async Task Main(string[] args) 
        {
            string resultDirPath = @"K:\files\results";
            List<string> files = new List<string>()
            {
                @"K:\files\Input.cs", 
                @"K:\files\DoubleInput.cs"
            };

            FileToFileGenerator generator = new( 
                new Parser(),
                new xUnitTestGenerator(),
                FileToFileGeneratorOptions.Default);

            await generator.GenerateTestsAsync(files, resultDirPath); 

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}