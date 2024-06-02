using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class Indexers
    {
        /// <summary>
        /// ?[index]
        /// </summary>
        public static BracketedArgumentListSyntax Indexer(int index)
        {
            return Indexer(Literals.Value(index));
        }
        /// <summary>
        /// ?[expressionSyntax]
        /// </summary>
        public static BracketedArgumentListSyntax Indexer(ExpressionSyntax expressionSyntax)
        {
            return BracketedArgumentList(
                SingletonSeparatedList(
                    Argument(expressionSyntax)));
        }

    }
}
