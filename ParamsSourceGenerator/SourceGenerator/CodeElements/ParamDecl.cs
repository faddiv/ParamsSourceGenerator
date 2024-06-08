using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class ParamDecl
    {
        public static ParameterSyntax Of(TypeSyntax type, string identifier)
        {
            return Parameter(
                attributeLists: default,
                modifiers: default,
                type,
                Identifier(identifier),
                @default: default);
        }

        public static ParameterSyntax Of(SyntaxTokenList modifiers, TypeSyntax type, string identifier)
        {
            return Parameter(
                attributeLists: default,
                modifiers: modifiers,
                type,
                Identifier(identifier),
                @default: default);
        }
    }
}
