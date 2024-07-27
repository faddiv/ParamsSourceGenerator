using System;
using System.Collections.Generic;
using System.Text;
using Foxy.Params.SourceGenerator.Data;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder(string intend = "    ")
{
    private int _intendLevel = 0;

    private readonly StringBuilder _builder = new();

    public string Intend { get; } = intend;

    public override string ToString()
    {
        return _builder.ToString();
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
            AddCommaSeparatedList(typeArguments);
            _builder.Append($">");
        }
        _builder.Append($"(");
        AddCommaSeparatedList(args);
        _builder.AppendLine(")");
        AddTypeConstraints(typeConstraintsList);
    }

    public void Field(string type, string name)
    {
        this.AppendLine($"public {type} {name};");
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

    public void AppendTextLine(string text)
    {
        AddIntend();
        AppendLineInternal(text);
    }

    public SourceLine StartLine()
    {
        return new SourceLine(this);
    }

    public void Clear()
    {
        _intendLevel = 0;
        _builder.Clear();
    }

    private void OpenBlock()
    {
        AddLineInternal("{");
        IncreaseIntend();
    }

    private void IncreaseIntend()
    {
        _intendLevel++;
    }
 
    private void DecreaseIntend()
    {
        _intendLevel--;
    }

    private void AppendLineInternal(string text)
    {
        _builder.AppendLine(text);
    }

    private void AppendInternal(string text)
    {
        _builder.Append(text);
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
            AddCommaSeparatedList(typeConstraints.Constraints);
            _builder.AppendLine();
        }
        DecreaseIntend();
    }

    private void AddLineInternal(string text)
    {
        AddIntend();
        _builder.AppendLine(text);
    }

    private void AddIntend()
    {
        for (int i = 0; i < _intendLevel; i++)
        {
            _builder.Append(Intend);
        }
    }

    private void AddCommaSeparatedList(IEnumerable<string> args)
    {
        _builder.AppendJoin(", ", args);
    }

    private void AppendFormatted<T>(T? t)
    {
        if (t is not null)
        {
            _builder.Append(t.ToString());
        }
    }

    private void Append(string arg)
    {
        _builder.Append(arg);
    }

    private void Append(int arg)
    {
        _builder.Append(arg);
    }

    private void EnsureCapacity(int capacity)
    {
        _builder.EnsureCapacity(capacity);
    }
}
