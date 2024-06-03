using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class Line
    {
        /// <summary>
        /// left = right;
        /// </summary>
        public static ExpressionStatementSyntax Assign(ExpressionSyntax left, ExpressionSyntax right)
        {
            return ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, left, right));
        }

        /// <summary>
        /// var variableName = equalsTo;
        /// </summary>
        public static LocalDeclarationStatementSyntax Var(string variableName, ExpressionSyntax equalsTo)
        {
            return LocalDeclarationStatement(
                SimpleVarDeclaration(variableName, equalsTo));
        }

        /// <summary>
        /// var variableName = equalsTo
        /// </summary>
        private static VariableDeclarationSyntax SimpleVarDeclaration(string variableName, ExpressionSyntax equalsTo)
        {
            return VariableDeclaration(Keyword.Var(), 
                SingletonSeparatedList(
                    VariableDeclarator(
                        Identifier(variableName),
                        null,
                        EqualsValueClause(equalsTo))));
        }

    }
}
