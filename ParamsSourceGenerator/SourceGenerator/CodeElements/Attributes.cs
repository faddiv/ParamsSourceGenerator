using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.CodeElements
{
    public static class Attributes
    {

        /// <summary>
        /// [attribute]<br/>
        /// ?
        /// </summary>
        public static SyntaxList<AttributeListSyntax> List(AttributeSyntax attribute)
        {
            return SingletonList(AttributeList(SingletonSeparatedList(attribute)));
        }

    }
}
