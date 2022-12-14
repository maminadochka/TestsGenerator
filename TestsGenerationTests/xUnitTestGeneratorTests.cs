using TestsGeneration.Data;
using TestsGeneration.Generators;

namespace TestsGenerationTests
{
    public class xUnitTestGeneratorTests
    {
        [Fact]
        public void Generate_Default()
        {
            //arrange
            xUnitTestGenerator generator = new xUnitTestGenerator();
            ParsedClass parsedClass = new ParsedClass()
            {
                Name = "ClassName",
                Namespace = "NameSpace",
                Methods = new List<string>() { "Method", "Method"}
            };

            var expected = @"using Xunit;
                               
                             namespace NameSpace.Tests
                             {
                                 class ClassNameTests
                                 {
                                     [Fact]
                                     public void MethodTest()
                                     {
                                         Assert.True(false, ""autogenerated"");
                                     }

                                     [Fact]
                                     public void Method1Test()
                                     {
                                         Assert.True(false, ""autogenerated"");
                                     }
                                 }
                             }".ExtractWords();

            //act
            var actual = generator.Generate(parsedClass).ExtractWords();

            //assert
            Assert.Equal(expected, actual);
        }
    }
}
