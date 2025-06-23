using Microsoft.CodeAnalysis;
using Xunit;
using FluentAssertions;
using Foxy.Params.SourceGenerator.Data;

namespace SourceGeneratorTests.Data
{
    public class DiagnosticInfoTests
    {
        private static DiagnosticDescriptor GetTestDescriptor() =>
            new("id", "title", "messageFormat {0}", "category", DiagnosticSeverity.Error, true);

        private static DiagnosticDescriptor GetDifferentDescriptor() =>
            new("id2", "title2", "messageFormat2 {0}", "category2", DiagnosticSeverity.Warning, true);

        private static Location GetTestLocation() => Location.None;

        private static object[] GetTestArgs() => ["arg1", "arg2"];

        private static object[] GetDifferentArgs() => ["arg3", "arg4"];

        [Fact]
        public void Create_ShouldReturnDiagnosticInfoInstance()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            // Act
            var diagnosticInfo = DiagnosticInfo.Create(descriptor, location, args);

            // Assert
            diagnosticInfo.Should().NotBeNull();
            diagnosticInfo.Descriptor.Should().Be(descriptor);
            diagnosticInfo.Location.Should().Be(location);
            diagnosticInfo.Args.Should().BeEquivalentTo(args);
        }

        [Fact]
        public void Equals_SameInstance_ShouldReturnTrue()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            var diagnosticInfo = new DiagnosticInfo(descriptor, location, args);

            // Act
            var result = diagnosticInfo.Equals(diagnosticInfo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_Null_ShouldReturnFalse()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            var diagnosticInfo = new DiagnosticInfo(descriptor, location, args);

            // Act
            var result = diagnosticInfo.Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_DifferentType_ShouldReturnFalse()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            var diagnosticInfo = new DiagnosticInfo(descriptor, location, args);

            // Act
            // ReSharper disable once SuspiciousTypeConversion.Global
            var result = diagnosticInfo.Equals("string");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_DifferentValues_ShouldReturnFalse()
        {
            // Arrange
            var descriptor1 = GetTestDescriptor();
            var descriptor2 = GetDifferentDescriptor();
            var location = GetTestLocation();
            var args1 = GetTestArgs();
            var args2 = GetDifferentArgs();

            var diagnosticInfo1 = new DiagnosticInfo(descriptor1, location, args1);
            var diagnosticInfo2 = new DiagnosticInfo(descriptor2, location, args2);

            // Act
            var result = diagnosticInfo1.Equals(diagnosticInfo2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            var diagnosticInfo1 = new DiagnosticInfo(descriptor, location, args);
            var diagnosticInfo2 = new DiagnosticInfo(descriptor, location, args);

            // Act
            var result = diagnosticInfo1.Equals(diagnosticInfo2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_SameValues_ShouldReturnSameHashCode()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            var diagnosticInfo1 = new DiagnosticInfo(descriptor, location, args);
            var diagnosticInfo2 = new DiagnosticInfo(descriptor, location, args);

            // Act
            var hashCode1 = diagnosticInfo1.GetHashCode();
            var hashCode2 = diagnosticInfo2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void ToDiagnostics_ShouldReturnDiagnostic()
        {
            // Arrange
            var descriptor = GetTestDescriptor();
            var location = GetTestLocation();
            var args = GetTestArgs();

            var diagnosticInfo = new DiagnosticInfo(descriptor, location, args);

            // Act
            var diagnostic = diagnosticInfo.ToDiagnostics();

            // Assert
            diagnostic.Should().NotBeNull();
            diagnostic.Descriptor.Should().Be(descriptor);
            diagnostic.Location.Should().Be(location);
            diagnostic.GetMessage().Should().Be(string.Format(descriptor.MessageFormat.ToString(), args));
        }
    }
}
