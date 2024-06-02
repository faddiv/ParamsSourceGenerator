using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class Keyword
    {
        /// <summary>
        /// var
        /// </summary>
        public static IdentifierNameSyntax Var()
        {
            return IdentifierName(Identifier("var"));
        }

    }
}
