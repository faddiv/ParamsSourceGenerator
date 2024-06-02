﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class TypeDef
    {
        /// <summary>
        /// parts0.parts1.parts2
        /// </summary>
        public static ExpressionSyntax Of(params string[] parts)
        {
            ExpressionSyntax current = IdentifierName(parts[0]);
            for (int i = 1; i < parts.Length; i++)
            {
                current = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    current,
                    IdentifierName(parts[i]));
            }
            return current;
        }

        /// <summary>
        /// global::qualifier
        /// </summary>
        public static AliasQualifiedNameSyntax Global(string qualifier)
        {
            return AliasQualifiedName(
                IdentifierName(Token(SyntaxKind.GlobalKeyword)),
                IdentifierName(qualifier));
        }

        /// <summary>
        /// global::qualifier1.qualifier2.qualifier3
        /// </summary>
        public static NameSyntax Global(params string[] qualifiers)
        {
            NameSyntax result = Global(qualifiers[0]);
            for (int i = 1; i < qualifiers.Length; i++)
            {
                result = QualifiedName(result, IdentifierName(qualifiers[i]));
            }
            return result;
        }

        /// <summary>
        /// global::qualifier1.qualifier2.qualifier3&lt;typeArgument1, typeArgumen2&gt;
        /// </summary>
        public static NameSyntax Global(string[] qualifiers, params TypeSyntax[] typeArgument)
        {

            NameSyntax result = Global(qualifiers[0]);
            for (int i = 1; i < qualifiers.Length; i++)
            {
                SimpleNameSyntax identifier = i == qualifiers.Length - 1
                    ? GenericName(Identifier(qualifiers[i]), TypeArguments(typeArgument))
                    : IdentifierName(qualifiers[i]);
                result = QualifiedName(result, identifier);
            }
            return result;
        }

        /// <summary>
        /// {argumentTypeName}
        /// </summary>
        private static TypeArgumentListSyntax TypeArguments(TypeSyntax[] typeArgument)
        {
            return TypeArgumentList(SeparatedList(typeArgument));
        }

    }
}
