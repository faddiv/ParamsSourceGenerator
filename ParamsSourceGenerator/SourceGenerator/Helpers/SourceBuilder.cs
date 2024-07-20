using System;
using System.Collections.Generic;
using System.Text;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.Rendering;

namespace Foxy.Params.SourceGenerator.Helpers;

internal class SourceBuilder : IRenderOutput
{
    private readonly StringBuilder _builder = new();
    public string Intend { get; set; } = "    ";
    private int _intendLevel = 0;
    public Stack<string> _scope = new Stack<string>();

    public override string ToString()
    {
        return _builder.ToString();
    }

    public void AddNamespaceHeader(string name)
    {
        AddLineInternal($"namespace {name}");
    }

    public void AddClassHeader(string name)
    {
        AddLineInternal($"partial class {name}");
    }

    public void AddBlock(Action<SourceBuilder> buidler)
    {
        OpenBlock();
        buidler(this);
        CloseBlock();
    }

    public void AddBlock<TArg1>(Action<SourceBuilder, TArg1> buidler, in TArg1 arg1)
    {
        OpenBlock();
        buidler(this, arg1);
        CloseBlock();
    }

    public void AddNamedBlock<TArg1>(
        string name,
        Action<SourceBuilder, TArg1> buidler,
        in TArg1 arg1)
    {
        PushScope(name);
        AddBlock(buidler, arg1);
        PopScope();
    }

    public void AddGenericStruct(string name, string genericParam1)
    {
        AddLineInternal($"file struct {name}<{genericParam1}>");
    }

    public void AddConstructorHeader(IEnumerable<string> args)
    {
        AddIntend();
        string className = _scope.Peek();
        _builder.Append($"public {className}(");
        CommaSeparatedItemList(args);
        _builder.AppendLine(")");
    }

    internal void Method(
        string name,
        IEnumerable<string> args,
        bool isStatic,
        string returnType,
        List<string> typeArguments,
        List<TypeConstrainInfo> typeConstraintsList)
    {
        AddIntend();
        _builder.Append("public");
        if (isStatic)
        {
            _builder.Append(" static");

        }
        _builder.Append(" ").Append(returnType);
        _builder.Append($" {name}");
        if (typeArguments.Count > 0)
        {
            _builder.Append($"<");
            CommaSeparatedItemList(typeArguments);
            _builder.Append($">");
        }
        _builder.Append($"(");
        CommaSeparatedItemList(args);
        _builder.AppendLine(")");
        AddTypeConstraints(typeConstraintsList);
    }

    public void Attribute(string name)
    {
        AppendLine($"[global::{name}]");
    }

    public void Field(string type, string name)
    {
        AppendLine($"public {type} {name};");
    }

    public void AutoGeneratedComment()
    {
        AddLineInternal("// <auto-generated />");
    }

    public void NullableEnable()
    {
        AddLineInternal("#nullable enable");
    }

    public void CloseBlock()
    {
        DecreaseIntend();
        AddLineInternal("}");
    }

    public void AppendLine()
    {
        _builder.AppendLine();
    }

    public void AppendLine(string text)
    {
        AddIntend();
        _builder.AppendLine(text);
    }

    public void AppendLine(char text)
    {
        AddIntend();
        _builder.Append(text);
        _builder.AppendLine();
    }

    public void OpenBlock()
    {
        AddLineInternal("{");
        IncreaseIntend();
    }

    public void IncreaseIntend()
    {
        _intendLevel++;
    }
    private void PushScope(string scope)
    {
        _scope.Push(scope);
    }

    private void PopScope()
    {
        _scope.Pop();
    }

    public void DecreaseIntend()
    {
        _intendLevel--;
    }

    public SourceLine StartLine()
    {
        return new SourceLine(this);
    }

    public void Clear()
    {
        _intendLevel = 0;
        _builder.Clear();
        _scope.Clear();
    }

    public class SourceLine
    {
        private readonly SourceBuilder _builder;

        public SourceLine(SourceBuilder builder)
        {
            _builder = builder;
            _builder.AddIntend();
        }

        public void Returns()
        {
            _builder._builder.Append("return ");
        }

        public void AddSegment(string segment)
        {
            _builder._builder.Append(segment);
        }

        public void AddCommaSeparatedList(IEnumerable<string> elements)
        {
            _builder._builder.Append(string.Join(", ", elements));
        }

        public void EndLine()
        {
            _builder._builder.AppendLine(";");
        }
    }

    private void AddTypeConstraints(List<TypeConstrainInfo> typeConstraintsList)
    {
        if (typeConstraintsList.Count <= 0)
            return;

        IncreaseIntend();
        foreach (var typeConstraints in typeConstraintsList)
        {
            AddIntend();
            _builder.Append($"where {typeConstraints.Type} : ");
            CommaSeparatedItemList(typeConstraints.Constraints);
            _builder.AppendLine();
        }
        DecreaseIntend();
    }

    private void AddLineInternal(string text)
    {
        AddIntend();
        _builder.AppendLine(text);
    }

    public void AddIntend()
    {
        for (int i = 0; i < _intendLevel; i++)
        {
            _builder.Append(Intend);
        }
    }

    private void CommaSeparatedItemList(IEnumerable<string> args)
    {
        ItemList(", ", args);
    }

    private void ItemList(string separator, IEnumerable<string> args)
    {
        var more = false;
        foreach (var item in args)
        {
            if (more)
            {
                _builder.Append(separator);
            }
            more = true;
            _builder.Append(item);
        }
    }

    internal void Append(char v)
    {
        _builder.Append(v);
    }

    internal void Append(string v)
    {
        _builder.Append(v);
    }

    internal void StartBlock()
    {
        AppendLine();
        Append('{');
        IncreaseIntend();
        AddIntend();
    }

    internal void EndBlock()
    {
        AppendLine();
        DecreaseIntend();
        AppendLine('}');
    }
}
