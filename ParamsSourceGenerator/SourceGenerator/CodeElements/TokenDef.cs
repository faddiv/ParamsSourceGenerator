using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class TokenDef
    {
        public static SyntaxTokenList Of(RefKind refKind)
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

        public static SyntaxTokenList Params()
        {
            return Tokens(SyntaxKind.ParamsKeyword);
        }

        public static SyntaxTokenList Partial()
        {
            return Tokens(SyntaxKind.PartialKeyword);
        }

        private static SyntaxTokenList Tokens(SyntaxKind syntaxKind)
        {
            return TokenList(Token(syntaxKind));
        }
    }
}
