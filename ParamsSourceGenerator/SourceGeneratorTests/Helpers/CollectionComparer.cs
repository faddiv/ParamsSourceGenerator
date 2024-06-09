using System.Collections.Generic;
using Foxy.Params.SourceGenerator.Helpers;
using Xunit;
using FluentAssertions;

namespace Foxy.Params.SourceGenerator.Tests.Helpers
{
    public class CollectionComparerTests
    {
        [Fact]
        public void Equals_BothListsNull_ShouldReturnTrue()
        {
            // Arrange
            var comparer = GetComparer();

            // Act
            var result = comparer.Equals(null, null);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_FirstListNull_ShouldReturnFalse()
        {
            // Arrange
            var comparer = GetComparer();
            List<int> list = CreateList();

            // Act
            var result = comparer.Equals(null, list);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_SecondListNull_ShouldReturnFalse()
        {
            // Arrange
            var comparer = GetComparer();
            var list = CreateList();

            // Act
            var result = comparer.Equals(list, null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_SameInstance_ShouldReturnTrue()
        {
            // Arrange
            var comparer = GetComparer();
            var list = CreateList();

            // Act
            var result = comparer.Equals(list, list);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_DifferentLengthLists_ShouldReturnFalse()
        {
            // Arrange
            var comparer = GetComparer();
            var list1 = new List<int> { 1, 2, 3 };
            var list2 = new List<int> { 1, 2 };

            // Act
            var result = comparer.Equals(list1, list2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_SameElements_ShouldReturnTrue()
        {
            // Arrange
            var comparer = GetComparer();
            var list1 = CreateList();
            var list2 = new List<int> { 1, 2, 3 };

            // Act
            var result = comparer.Equals(list1, list2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_DifferentElements_ShouldReturnFalse()
        {
            // Arrange
            var comparer = GetComparer();
            var list1 = CreateList();
            var list2 = new List<int> { 1, 2, 4 };

            // Act
            var result = comparer.Equals(list1, list2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_NullList_ShouldReturnDefaultHashCode()
        {
            // Arrange
            var comparer = GetComparer();

            // Act
            var result = comparer.GetHashCode(null);

            // Assert
            result.Should().Be(2011230944);
        }

        [Fact]
        public void GetHashCode_NonEmptyList_ShouldReturnCorrectHashCode()
        {
            // Arrange
            var comparer = GetComparer();
            var list = CreateList();

            // Act
            var result = comparer.GetHashCode(list);

            // Assert
            var expectedHashCode = 1884520134;
            result.Should().Be(expectedHashCode);
        }

        private static CollectionComparer<List<int>, int> GetComparer()
        {
            return CollectionComparer<List<int>, int>.Default;
        }

        private static List<int> CreateList()
        {
            return new List<int> { 1, 2, 3 };
        }

    }
}