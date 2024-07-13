using Xunit;
using FluentAssertions;
using SourceGeneratorTests.TestInfrastructure;

namespace SourceGeneratorTests.Data;

public class SuccessfulParamsCandidateTests
{
    [Fact]
    public void Equals_WithSameObject_ShouldReturnTrue()
    {
        // Arrange
        var candidate = TestData.CreateSuccessfulParamsCandidate();

        // Act & Assert
        candidate.Equals(candidate).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithEqualObject_ShouldReturnTrue()
    {
        // Arrange
        var typeInfo = TestData.CreateCandidateTypeInfo();
        var derivedData = TestData.CreateDerivedData();

        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate();

        // Act & Assert
        candidate1.Equals(candidate2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithNullObject_ShouldReturnFalse()
    {
        // Arrange
        var candidate = TestData.CreateSuccessfulParamsCandidate();

        // Act & Assert
        candidate.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var candidate = TestData.CreateSuccessfulParamsCandidate();

        var differentTypeObject = new { };

        // Act & Assert
        candidate.Equals(differentTypeObject).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithTypeInfoDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            typeInfo: TestData.CreateCandidateTypeInfo(typeName: "DifferentTypeName"));

        // Act & Assert
        candidate1.Equals(candidate2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDerivedDataDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            derivedData: TestData.CreateDerivedData(argName: "DifferentArgName"));

        // Act & Assert
        candidate1.Equals(candidate2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithMaxOverridesDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            maxOverrides: 10);

        // Act & Assert
        candidate1.Equals(candidate2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithHasParamDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            hasParams: false);

        // Act & Assert
        candidate1.Equals(candidate2).Should().BeFalse();
    }
}
