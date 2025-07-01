using System;
using System.Linq;
using Foxy.Params.SourceGenerator.Helpers;
using Xunit;
using FluentAssertions;

namespace SourceGeneratorTests
{
    public class CollectionComparerTests
    {
        [Fact]
        public void GetHashCode_NullList_ShouldReturnDefaultHashCode()
        {
            // Act
            var result = CollectionComparer.GetHashCode<object>(null);

            // Assert
            result.Should().Be(2011230944);
        }

        [Fact]
        public void GetHashCode_NonEmptyList_ShouldReturnCorrectHashCode()
        {
            // Arrange
            var list = CreateList();

            // Act
            var result = CollectionComparer.GetHashCode(list);

            // Assert
            var expectedHashCode = 1884520134;
            result.Should().Be(expectedHashCode);
        }

        [Fact]
        public void GetHashCode_FromDifferentInstances_ShouldReturnTheSameHashCode()
        {
            // Arrange
            var list1 = CreateList();
            var list2 = CreateList();

            // Act
            var result1 = CollectionComparer.GetHashCode(list1);
            var result2 = CollectionComparer.GetHashCode(list2);

            // Assert
            result1.Should().Be(result2);
        }

        private static int[] CreateList()
        {
            return [1, 2, 3];
        }

    }
}
