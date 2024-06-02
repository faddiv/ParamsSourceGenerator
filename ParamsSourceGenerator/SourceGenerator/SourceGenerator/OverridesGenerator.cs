﻿using Foxy.Params.SourceGenerator.CodeElements;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Foxy.Params.SourceGenerator.SourceGenerator
{
    internal class OverridesGenerator
    {
        private readonly SourceBuilder _sourceBuilder;
        private readonly INamedTypeSymbol _typeInfo;
        private readonly IEnumerable<SuccessfulParamsCandidate> _paramsCandidates;
        private readonly int _maxOverridesMax;

        public OverridesGenerator(INamedTypeSymbol typeInfo, IEnumerable<SuccessfulParamsCandidate> paramsCandidates)
        {
            _sourceBuilder = new SourceBuilder();
            _typeInfo = typeInfo ?? throw new ArgumentNullException(nameof(typeInfo));
            _paramsCandidates = paramsCandidates ?? throw new ArgumentNullException(nameof(paramsCandidates));
            _maxOverridesMax = paramsCandidates.Max(e => e.MaxOverrides);
        }

        public SourceText Execute()
        {
            _sourceBuilder.Clear();
            _sourceBuilder.AppendTrivias(
                AutoGeneratedComment(),
                CarriageReturnLineFeed,
                NullableEnable(),
                CarriageReturnLineFeed);
            GenerateNamespace();

            return SourceText.From(_sourceBuilder.ToString(), Encoding.UTF8);
        }

        private static SyntaxTrivia AutoGeneratedComment()
        {
            return Comment("// <auto-generated />");
        }

        private static SyntaxTrivia NullableEnable()
        {
            return Trivia(
                NullableDirectiveTrivia(
                    Token(TriviaList(Space), SyntaxKind.EnableKeyword, TriviaList()),
                    true));
        }

        private void GenerateNamespace()
        {
            AddNamespace(_typeInfo);

            GeneratePartialClass();
            GenerateArgumentsClasses();

            AddNamespaceCloseBlock(_typeInfo);
        }

        private void GeneratePartialClass()
        {
            var nestLevel = CreateClasses(_typeInfo);

            foreach (var paramsCandidate in _paramsCandidates)
            {
                var data = new DerivedData(paramsCandidate);

                for (int n = 1; n <= paramsCandidate.MaxOverrides; n++)
                {
                    if (n > 1)
                    {
                        _sourceBuilder.AppendLine();
                    }

                    GenerateMethodOverrideWithNArgs(data, n);
                }

                GenerateOverrideWithParamsParameter(paramsCandidate, data);
            }

            CloseTimes(nestLevel);
        }

        private int CreateClasses(INamedTypeSymbol? typeInfo)
        {
            var items = SemanticHelpers.GetTypeHierarchy(typeInfo);
            foreach (var item in items)
            {
                _sourceBuilder.Class(item.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
            }

            return items.Count;
        }

        private void CloseTimes(int nestLevel)
        {
            for (int i = 0; i < nestLevel; i++)
            {
                _sourceBuilder.CloseBlock();
            }
        }

        private void GenerateMethodOverrideWithNArgs(DerivedData data, int argsCount)
        {
            var variableArguments = data.FixArguments.Concat(
                Enumerable.Range(0, argsCount).Select(j => $"{data.SpanArgumentType} {data.ArgName}{j}"));
            GenerateMethodHeaderWithArguments(data, variableArguments);
            GenerateBodyForOverrideWithNArgs(data, argsCount);
            _sourceBuilder.CloseBlock();
        }

        private void GenerateMethodHeaderWithArguments(DerivedData data, IEnumerable<string> arguments)
        {
            _sourceBuilder.Method(
                data.MethodName,
                arguments,
                data.IsStatic,
                data.ReturnType,
                data.TypeArguments,
                data.TypeConstraints);
        }

        private void GenerateBodyForOverrideWithNArgs(DerivedData data, int argsCount)
        {
            _sourceBuilder.AppendBlockLine(GenerateArgumentsVariable(data, argsCount));
            _sourceBuilder.AppendBlockLine(GenerateSpanVariableForInlineArray(data, argsCount));
            _sourceBuilder.AppendBlockLine(GenerateCallOriginalMethod(data));
        }


        /// <summary>
        /// var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref arg.arg0, argsCount);
        /// </summary>
        private VariableDeclarationSyntax GenerateSpanVariableForInlineArray(DerivedData data, int argsCount)
        {
            return Line.Var(data.ArgNameSpan, CallCreateReadOnlySpan(data.ArgName, argsCount));
        }

        /// <summary>
        /// global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 1)
        /// </summary>
        private ExpressionSyntax CallCreateReadOnlySpan(string argName, int argsCount)
        {
            return InvocationExpression(
                TypeDef.Global("System", "Runtime", "InteropServices", "MemoryMarshal", "CreateReadOnlySpan"),
                ArgumentDecl.List(
                    ArgumentDecl.Ref(TypeDef.Of(argName, "arg0")),
                    Argument(Literals.Value(argsCount)))
                );
        }

        /// <summary>
        /// var {argName} = new Arguments{argsCount}&lt;{SpanArgumentType}&gt;(argName1, argName2, argName3);
        /// </summary>
        private VariableDeclarationSyntax GenerateArgumentsVariable(DerivedData data, int argsCount)
        {
            return Line.Var(data.ArgName,
                Constructor.New(
                    TypeDef.Of([$"Arguments{argsCount}"], ParseTypeName(data.SpanArgumentType)),
                    Arguments(argsCount, data.ArgName)
                    ));

            static IEnumerable<ArgumentSyntax> Arguments(int argsCount, string argNamePrefix)
            {
                for (var i = 0; i < argsCount; i++)
                {
                    yield return ArgumentDecl.Of($"{argNamePrefix}{i}");
                }
            }
        }

        private void GenerateOverrideWithParamsParameter(SuccessfulParamsCandidate paramsCandidate, DerivedData data)
        {
            if (!paramsCandidate.HasParams)
            {
                return;
            }
            _sourceBuilder.AppendLine();
            GenerateMethodHeaderWithArguments(data, data.FixArguments.Append($"params {data.SpanArgumentType}[] {data.ArgName}"));
            GenerateBodyWithParamsParameter(data);
            _sourceBuilder.CloseBlock();
        }

        private void GenerateBodyWithParamsParameter(DerivedData data)
        {
            _sourceBuilder.AppendBlockLine(GenerateSpanVariableForParamsArgument(data));
            _sourceBuilder.AppendBlockLine(GenerateCallOriginalMethod(data));
        }

        /// <summary>
        /// var argsSpan = new global::System.ReadOnlySpan<T>(args);
        /// </summary>
        private VariableDeclarationSyntax GenerateSpanVariableForParamsArgument(DerivedData data)
        {
            return Line.Var(
                data.ArgNameSpan,
                NewReadOnlySpanOfTWithArgs(data.SpanArgumentType, data.ArgName));            
        }

        /// <summary>
        /// new global::System.ReadOnlySpan&lt;T&gt;(argName)
        /// </summary>
        private static ObjectCreationExpressionSyntax NewReadOnlySpanOfTWithArgs(string spanArgumentType, string argName)
        {
            return Constructor.New(
                TypeDef.Global(["System", "ReadOnlySpan"], ParseTypeName(spanArgumentType)),
                ArgumentDecl.Of(argName));
        }

        /// <summary>
        /// {typeParameterName}
        /// </summary>
        private static TypeParameterListSyntax TypeParameters(string typeParameterName)
        {
            return TypeParameterList(SingletonSeparatedList(TypeParameter(typeParameterName)));
        }

        private void GenerateArgumentsClasses()
        {
            for (int i = 1; i <= _maxOverridesMax; i++)
            {
                _sourceBuilder.AppendLine();
                CreateArgumentsStruct(i);
            }
        }

        /// <summary>
        /// return Format<{T}>({format}, {argsSpan})
        /// </summary>
        private SyntaxNode GenerateCallOriginalMethod(DerivedData data)
        {
            SimpleNameSyntax methodName = data.TypeArguments.Count > 0
                ? GenericName(Identifier(data.MethodName), TypeArgumentList(SeparatedList(GenerateInvocationTypeArguments(data))))
                : IdentifierName(data.MethodName);
            SyntaxNode invocationExpression = InvocationExpression(
                methodName,
                ArgumentList(SeparatedList(GenerateInvocationArguments(data))));
            return AddReturnStatement(data.ReturnsKind, invocationExpression);
        }

        /// <summary>
        /// <code>Format()</code> 
        /// or
        /// <code>return Format()</code>
        /// or
        /// <code>return ref Format()</code>
        /// </summary>
        private static SyntaxNode AddReturnStatement(ReturnKind returnKind, SyntaxNode invocationExpression)
        {
            if (returnKind == ReturnKind.ReturnsRef)
            {
                invocationExpression = RefExpression((ExpressionSyntax)invocationExpression);
            }
            if (returnKind != ReturnKind.ReturnsVoid)
            {
                invocationExpression = ReturnStatement((ExpressionSyntax)invocationExpression);
            }

            return invocationExpression;
        }

        private IEnumerable<TypeSyntax> GenerateInvocationTypeArguments(DerivedData data)
        {
            foreach (var item in data.TypeArguments)
            {
                yield return IdentifierName(item);
            }
        }

        private IEnumerable<ArgumentSyntax> GenerateInvocationArguments(DerivedData data)
        {
            foreach (var item in data.ParameterInfos)
            {
                yield return item.GetPassParameterModifier() switch
                {
                    RefKind.Ref => ArgumentDecl.Ref(IdentifierName(item.Name)),
                    RefKind.Out => ArgumentDecl.Out(IdentifierName(item.Name)),
                    RefKind.In => ArgumentDecl.In(IdentifierName(item.Name)),
                    _ => ArgumentDecl.Of(item.Name)
                };
            }
            yield return ArgumentDecl.Of(data.ArgNameSpanInput);
        }

        private void CreateArgumentsStruct(int length)
        {
            var typeName = Identifier($"Arguments{length}");
            var argumentsStruct = StructDeclaration(
                attributeLists: Attributes.List(InlineArrayAttribute(length)),
                modifiers: Modifier.File(),
                identifier: typeName,
                typeParameterList: TypeParameters("T"),
                baseList: default,
                constraintClauses: default,
                members: ArgumentsStructMembers(length, typeName));
            argumentsStruct = argumentsStruct.NormalizeWhitespace();
            argumentsStruct = argumentsStruct.AddEmptyLineAfterMember(argumentsStruct.Members[0]);
            _sourceBuilder.AppendBlockLine(argumentsStruct, false);
        }

        private static SyntaxList<MemberDeclarationSyntax> ArgumentsStructMembers(int length, SyntaxToken typeName)
        {
            return List(ArgumentsStructMembers(typeName, length));

            static IEnumerable<MemberDeclarationSyntax> ArgumentsStructMembers(SyntaxToken typeName, int argumentsCount)
            {
                yield return Field.Public(IdentifierName("T"), "arg0");
                yield return CtorArguments(typeName, argumentsCount);
            }

        }

        private static MemberDeclarationSyntax CtorArguments(SyntaxToken typeName, int argumentsCount)
        {
            return ConstructorDeclaration(
                attributeLists: default,
                modifiers: Modifier.Public(),
                identifier: typeName,
                parameterList: ArgumentsTypeCtorParameters(argumentsCount),
                initializer: default,
                body: ArgumentsTypeCtorBody(argumentsCount));
        }

        private static ParameterListSyntax ArgumentsTypeCtorParameters(int argumentsCount)
        {
            return ParameterList(SeparatedList(ArgumentsTypeCtorEnumerator(argumentsCount)));

            static IEnumerable<ParameterSyntax>? ArgumentsTypeCtorEnumerator(int argumentsCount)
            {
                for (int i = 0; i < argumentsCount; i++)
                {
                    yield return Parameter(Identifier($"value{i}"))
                        .WithType(IdentifierName("T"));
                }
            }
        }

        private static BlockSyntax ArgumentsTypeCtorBody(int argumentsCount)
        {
            return Block(ArgumentsTypeCtorBodyEnumerator(argumentsCount));

            static IEnumerable<StatementSyntax> ArgumentsTypeCtorBodyEnumerator(int argumentsCount)
            {
                yield return Line.Assign(IdentifierName("arg0"), IdentifierName("value0"));
                for (int i = 1; i < argumentsCount; i++)
                {
                    yield return Line.Assign(
                        ElementAccessExpression(ThisExpression(), Indexers.Indexer(i)),
                        IdentifierName($"value{i}"));
                }
            }
        }


        /// <summary>
        /// [global::System.Runtime.CompilerServices.InlineArray(length)]<br/>
        /// ?
        /// </summary>
        private static AttributeSyntax InlineArrayAttribute(int length)
        {
            return Attribute(
                TypeDef.Global("System", "Runtime", "CompilerServices", "InlineArray"),
                AttributeArgumentList(SingletonSeparatedList(AttributeArgument(Literals.Value(length))
                )));
        }

        private void AddNamespace(INamedTypeSymbol typeInfo)
        {
            if (typeInfo.ContainingNamespace.IsGlobalNamespace)
                return;

            /*var name = ParseName(SemanticHelpers.GetNameSpaceNoGlobal(typeInfo));
            var @namespace = NamespaceDeclaration(name);
            sb.AppendTrivia(@namespace);*/

            _sourceBuilder.Namespace(SemanticHelpers.GetNameSpaceNoGlobal(typeInfo));
        }

        private void AddNamespaceCloseBlock(INamedTypeSymbol typeInfo)
        {
            if (typeInfo.ContainingNamespace.IsGlobalNamespace)
                return;

            _sourceBuilder.CloseBlock();
        }
    }
}
