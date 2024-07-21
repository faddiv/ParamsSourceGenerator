using Microsoft.CodeAnalysis;
using System.Text;

[Generator]
public partial class StackArrayIncrementalGenerator : IIncrementalGenerator
{
    private const string _attributeName = "Foxy.Params.ParamsAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(AddParamsAttribute);
    }

    private void AddParamsAttribute(IncrementalGeneratorPostInitializationContext context)
    {
        for (int i = 2; i <= 32; i++)
        {
            var builder = new StringBuilder();
            builder.Append(
                $$"""
                using System.Runtime.CompilerServices;
                using System.Runtime.InteropServices;

                namespace Foxy.Params;
                                
                /// <summary>
                /// Represents a stack-allocated array with a fixed size of {{i}} elements.
                /// </summary>
                /// <typeparam name="T">The type of elements in the array.</typeparam>
                [InlineArray({{i}})]
                public struct StackArray{{i}}<T>
                {
                    private T _element0;
                                
                    /// <summary>
                    /// Initializes a new instance of the <see cref="StackArray{{i}}{T}"/> struct with the specified elements.
                    /// </summary>
                    public StackArray{{i}}(T element0
                """);
            for (int j = 1; j < i; j++)
            {
                builder.Append($", T element{j}");
            }

            builder.AppendLine(")");
            builder.AppendLine("    {");
            builder.AppendLine("        _element0 = element0;");
            for (int j = 1; j < i; j++)
            {
                builder.AppendLine($"        this[{j}] = element{j};");
            }

            builder.Append(
                $$"""
                    }
                                
                    /// <summary>
                    /// Gets the length of the array, which is always {{i}}.
                    /// </summary>
                    public readonly int Length => {{i}};
                                
                    /// <summary>
                    /// Creates a new <see cref="Span{T}"/> over the elements of the array.
                    /// </summary>
                    /// <returns>A <see cref="Span{T}"/> that represents the array.</returns>
                    public Span<T> AsSpan()
                    {
                        return MemoryMarshal.CreateSpan(ref _element0, Length);
                    }
                                
                    /// <summary>
                    /// Creates a new <see cref="ReadOnlySpan{T}"/> over the elements of the array.
                    /// </summary>
                    /// <returns>A <see cref="ReadOnlySpan{T}"/> that represents the array.</returns>
                    public ReadOnlySpan<T> AsReadOnlySpan()
                    {
                        return MemoryMarshal.CreateReadOnlySpan(ref _element0, Length);
                    }
                }
                
                """);
            context.AddSource($"StackArray{i}.g.cs", 
                builder.ToString());
        }
    }
}

