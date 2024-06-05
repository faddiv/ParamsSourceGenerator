using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class ParamDecl
    {
        public static ParameterSyntax Of(TypeSyntax type, string identifier, RefKind refKind)
        {
            return Parameter(
                attributeLists: default,
                modifiers: Modifiers(refKind),
                type,
                Identifier(identifier),
                @default: default);
        }

        private static SyntaxTokenList Modifiers(RefKind refKind)
        {
            switch (refKind)
            {
                case RefKind.Ref:
                    return Tokens(SyntaxKind.RefKeyword);
                case RefKind.In:
                    return Tokens(SyntaxKind.InKeyword);
                case RefKind.Out:
                    return Tokens(SyntaxKind.OutKeyword);
                case RefKind.RefReadOnlyParameter:
                    return TokenList(Token(SyntaxKind.RefKeyword), Token(SyntaxKind.ReadOnlyKeyword));
                default:
                    return default;
            }
        }

        private static SyntaxTokenList Tokens(SyntaxKind syntaxKind)
        {
            return TokenList(Token(syntaxKind));
        }
    }
}
