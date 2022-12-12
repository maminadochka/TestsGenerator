using TestsGeneration.Data;
using TestsGeneration.Parsers;

namespace TestsGenerationTests
{
    public class ParserTests
    {
        [Fact]
        public void Parse_Test_Default()
        {
            //arrange
            Parser parser = new();
            string input = @"namespace SimpleNamespace
                             {
                                public class FirstClass
                                {
                                    public void FirstMethod()
                                    {

                                    }

                                    public void FirstMethod(int a)
                                    {

                                    }

                                    private void PrivateMethod()
                                    {
            
                                    }
                                }

                                public class SecondClass
                                {
                                    public void SecondMethod()
                                    {
                                        
                                    }
                                }
                            }";

            IEnumerable<ParsedClass> expected = new List<ParsedClass>()
            {
                new ParsedClass()
                {
                    Namespace = "SimpleNamespace",
                    Name = "FirstClass",
                    Methods = new List<string>() { "FirstMethod", "FirstMethod" }
                },
                new ParsedClass()
                {
                    Namespace = "SimpleNamespace",
                    Name = "SecondClass",
                    Methods = new List<string>() { "SecondMethod" }
                }
            }.AsEnumerable();

            //act
            var actual = parser.Parse(input);

            //assert
            Assert.Equal(expected, actual);
        }
    }
}