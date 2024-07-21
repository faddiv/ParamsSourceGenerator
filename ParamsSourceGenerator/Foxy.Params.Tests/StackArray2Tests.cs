/*
using FluentAssertions;
using Xunit;

namespace Foxy.Params.Tests
{
    public class StackArray2Tests
    {
        [Fact]
        public void Length_ReturnsTheCorrectLength()
        {
            var stack = new StackArray2<int>();

            stack.Length.Should().Be(2);
        }

        [Fact]
        public void Ctor_SetsObjectUp()
        {
            var stack = new StackArray2<int>(1, 2);

            for (int i = 0; i < 2; i++)
            {
                stack[i].Should().Be(i + 1);
            }
        }

        [Fact]
        public void AsSpan_CreatesSpanReference()
        {
            var stack = new StackArray2<int>(0, 0);
            var span = stack.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = i + 1;
            }
            for (int i = 0; i < stack.Length; i++)
            {
                stack[i].Should().Be(i + 1);
            }
        }

        [Fact]
        public void AsReadOnlySpan_CreatesReadOnlySpanReference()
        {
            var stack = new StackArray2<int>(0, 0);
            var span = stack.AsReadOnlySpan();
            for (int i = 0; i < stack.Length; i++)
            {
                stack[i] = i + 1;
            }
            for (int i = 0; i < span.Length; i++)
            {
                span[i].Should().Be(i + 1);
            }
        }
    }
}
*/