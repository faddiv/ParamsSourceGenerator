using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foxy.Params.SourceGenerator.Helpers
{
    internal static class TypeDeclarationSyntaxExtensions
    {
        public static TNode AddEmptyLineAfterMember<TNode>(this TNode node, MemberDeclarationSyntax member) where TNode : TypeDeclarationSyntax {
            var trailingTrivia = member.GetTrailingTrivia();
            return node.ReplaceNode(
                member,
                member.WithTrailingTrivia(trailingTrivia.Add(SyntaxFactory.CarriageReturnLineFeed))
                );
        }
    }
}
