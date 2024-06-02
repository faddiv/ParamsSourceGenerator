using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class Literals
    {
        /// <summary>
        /// {intValue}
        /// </summary>
        public static LiteralExpressionSyntax Value(int value)
        {
            return LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));
        }

    }
}
