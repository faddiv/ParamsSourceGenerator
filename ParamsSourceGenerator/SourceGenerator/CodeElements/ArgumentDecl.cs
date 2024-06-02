using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class ArgumentDecl
    {
        /// <summary>
        /// arg0, arg1, arg2
        /// </summary>
        public static ArgumentListSyntax List(params ArgumentSyntax[] args)
        {
            return ArgumentList(SeparatedList(args));
        }

        /// <summary>
        /// arg0, arg1, arg2
        /// </summary>
        public static ArgumentListSyntax List(IEnumerable<ArgumentSyntax> args)
        {
            return ArgumentList(SeparatedList(args));
        }


        /// <summary>
        /// In argument list: arg
        /// </summary>
        public static ArgumentListSyntax Arguments(params ExpressionSyntax[] arg)
        {
            return ArgumentList(SeparatedList(arg.Select(Argument)));
        }

        /// <summary>
        /// In argument list: argName
        /// </summary>
        internal static ArgumentSyntax Of(string argName)
        {
            return Argument(IdentifierName(argName));
        }

        /// <summary>
        /// <code>ref arg</code>
        /// </summary>
        public static ArgumentSyntax Ref(ExpressionSyntax arg)
        {
            return Argument(default, Token(SyntaxKind.RefKeyword), arg);
        }

        /// <summary>
        /// <code>in arg</code>
        /// </summary>
        public static ArgumentSyntax In(ExpressionSyntax arg)
        {
            return Argument(default, Token(SyntaxKind.InKeyword), arg);
        }

        /// <summary>
        /// <code>out arg</code>
        /// </summary>
        public static ArgumentSyntax Out(ExpressionSyntax arg)
        {
            return Argument(default, Token(SyntaxKind.OutKeyword), arg);
        }
    }
}
