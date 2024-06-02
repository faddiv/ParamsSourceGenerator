using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public class Field
    {
        public static FieldDeclarationSyntax Public(TypeSyntax type, string identifier)
        {
            return FieldDeclaration(
                attributeLists: default,
                modifiers: Modifier.Public(),
                VariableDeclaration(
                    type,
                    SingletonSeparatedList(VariableDeclarator(Identifier(identifier)))));
        }
    }
}
