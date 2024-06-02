using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public class Constructor
    {
        public static ObjectCreationExpressionSyntax New(
            TypeSyntax typeName, params ArgumentSyntax[] arguments)
        {
            return ObjectCreationExpression(typeName, ArgumentDecl.List(arguments), initializer: default);
        }

        public static ObjectCreationExpressionSyntax New(
            TypeSyntax typeName, IEnumerable<ArgumentSyntax> arguments)
        {
            return ObjectCreationExpression(typeName, ArgumentDecl.List(arguments), initializer: default);
        }
    }
}
