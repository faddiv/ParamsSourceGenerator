/*
using FluentAssertions;
using Xunit;

namespace Foxy.Params.Tests
{
    public class StackArray32Tests
    {
        [Fact]
        public void Length_ReturnsTheCorrectLength()
        {
            var stack = new StackArray32<int>();

            stack.Length.Should().Be(32);
        }

        [Fact]
        public void Ctor_SetsObjectUp()
        {
            var stack = new StackArray32<int>(1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32);

            for (int i = 0; i < 32; i++)
            {
                stack[i].Should().Be(i + 1);
            }
        }

        [Fact]
        public void AsSpan_CreatesSpanReference()
        {
            var stack = new StackArray32<int>();
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
            var stack = new StackArray32<int>();
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