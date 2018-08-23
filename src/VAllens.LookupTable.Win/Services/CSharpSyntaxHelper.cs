using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VAllens.LookupTable
{
    public class CSharpSyntaxHelper
    {
        public static IEnumerable<string> ExtractStringLiterals(string filePath)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

            var root = syntaxTree.GetRoot();
            var visitor = new ExtractStringLiteralVisitor();
            visitor.Visit(root);

            return visitor.Literals;
        }
    }
}