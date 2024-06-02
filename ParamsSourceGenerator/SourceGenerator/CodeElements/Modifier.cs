using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class Modifier
    {
        public static SyntaxTokenList Public()
        {
            return Modifiers(SyntaxKind.PublicKeyword);
        }

        internal static SyntaxTokenList File()
        {
            return Modifiers(SyntaxKind.FileKeyword);
        }

        /// <summary>
        /// modifier ?
        /// </summary>
        private static SyntaxTokenList Modifiers(SyntaxKind modifier)
        {
            return TokenList(Token(modifier));
        }

    }
}
