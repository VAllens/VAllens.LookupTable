using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VAllens.LookupTable
{
    /// <summary>
    /// C#文件语法分析字符串提取访问者
    /// </summary>
    public class ExtractStringLiteralVisitor : SyntaxWalker
    {
        private readonly List<string> _literals = new List<string>();

        public IEnumerable<string> Literals => _literals;

        public override void Visit(SyntaxNode node)
        {
            if (node.Kind() == SyntaxKind.StringLiteralExpression)
                _literals.Add(node.ToString());
            base.Visit(node);
        }
    }
}