using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using TestsGeneration.Data;

namespace TestsGeneration.Parsers
{
    public class Parser : IParser
    {
        public Parser()
        {

        }


        public IEnumerable<ParsedClass> Parse(string code) 
        {
            var root = CSharpSyntaxTree.ParseText(code).GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>(); 
            List<ParsedClass> result = new();

            foreach (var c in classes)
            { 
                var methods = c.DescendantNodes()
                               .OfType<MethodDeclarationSyntax>()
                               .Where(m => m.Modifiers.ToString().Contains("public"));
               
                ParsedClass parsedClass = new ParsedClass(
                    c.Identifier.Text,
                    GetNamespace(c),
                    methods.Select(m => m.Identifier.Text).ToList());

                result.Add(parsedClass);
            }

            return result;
        }

        private static string GetNamespace(ClassDeclarationSyntax classNode)
        {
            StringBuilder resBuilder = new(string.Empty);
            var parent = classNode.Parent;

            //пока есть родитель и он не корень
            while ((parent != null) && (parent is not CompilationUnitSyntax))
            {               
                var namespaceParent = parent as NamespaceDeclarationSyntax;
                if (namespaceParent != null)
                {
                    resBuilder.Append(namespaceParent.Name.ToString());
                }

                parent = parent.Parent;
            }

            return resBuilder.ToString();
        }
    }
}
