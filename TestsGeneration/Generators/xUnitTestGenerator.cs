﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;
using TestsGeneration.Data;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestsGeneration.Generators
{
    public class xUnitTestGenerator : ITestGenerator
    {
        public xUnitTestGenerator()
        {

        }


        public IEnumerable<string> Generate(IEnumerable<ParsedClass> classes)
        {
            return classes.Select(c => GenerateTestClass(c)); 
        }

        public string Generate(ParsedClass parsedClass)
        {
            return GenerateTestClass(parsedClass); 
        }

        private static string GenerateTestClass(ParsedClass c)
        {
            return CompilationUnit()
                    .WithUsings(
                        SingletonList<UsingDirectiveSyntax>(
                            UsingDirective(
                                IdentifierName("Xunit"))))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            NamespaceDeclaration(
                                IdentifierName(GenerateProperNamespace(c.Namespace)))
                            .WithMembers(
                                SingletonList<MemberDeclarationSyntax>(
                                    ClassDeclaration(c.Name + "Tests")
                                        .WithMembers(
                                            List<MemberDeclarationSyntax>(
                                                GenerateTestMethods(c)))))))
                    .NormalizeWhitespace().ToFullString();
        }

        private static string GenerateProperNamespace(string defaultNamespace) 
        {
            if (string.IsNullOrEmpty(defaultNamespace))
                return "Tests";

            return defaultNamespace + ".Tests";
        }

        private static MemberDeclarationSyntax[] GenerateTestMethods(ParsedClass c) 
        {
            List<MemberDeclarationSyntax> res = new();
            HashSet<string> seen = new HashSet<string>();

            foreach (var method in c.Methods)
            {
                var syntax = MethodDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.VoidKeyword)),
                                Identifier(ResolvePossibleDuplicate(method, seen) + "Test"))
                            .WithAttributeLists(
                                SingletonList<AttributeListSyntax>(
                                    AttributeList(
                                        SingletonSeparatedList<AttributeSyntax>(
                                            Attribute(
                                                IdentifierName("Fact"))))))
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword)))
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ExpressionStatement(
                                            InvocationExpression(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("Assert"),
                                                    IdentifierName("True")))
                                            .WithArgumentList(
                                                ArgumentList(
                                                    SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]{
                                                            Argument(
                                                                LiteralExpression(
                                                                    SyntaxKind.FalseLiteralExpression)),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(
                                                                LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    Literal("autogenerated")))})))))));
                res.Add(syntax);
            }

            return res.ToArray();
        }

        private static string ResolvePossibleDuplicate(string str, HashSet<string> set) //перегрузка методов
        {
            int i = 0;
            string currStr = str;

            while (!set.Add(currStr))
            {
                ++i;
                currStr += i;
            }

            return currStr;
        }
    }
}
