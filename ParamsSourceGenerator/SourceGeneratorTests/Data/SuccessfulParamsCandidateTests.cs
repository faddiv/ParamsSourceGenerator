using Xunit;
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
        Assert.True(candidate.Equals(candidate));
    }

    [Fact]
    public void Equals_WithEqualObject_ShouldReturnTrue()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate();

        // Act & Assert
        Assert.True(candidate1.Equals(candidate2));
    }

    [Fact]
    public void Equals_WithNullObject_ShouldReturnFalse()
    {
        // Arrange
        var candidate = TestData.CreateSuccessfulParamsCandidate();

        // Act & Assert
        Assert.False(candidate.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var candidate = TestData.CreateSuccessfulParamsCandidate();

        var differentTypeObject = new { };

        // Act & Assert
        Assert.False(candidate.Equals(differentTypeObject));
    }

    [Fact]
    public void Equals_WithTypeInfoDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            typeInfo: TestData.CreateCandidateTypeInfo(typeName: "DifferentTypeName"));

        // Act & Assert
        Assert.False(candidate1.Equals(candidate2));
    }

    [Fact]
    public void Equals_WithDerivedDataDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            derivedData: TestData.CreateDerivedData(methodName: "DifferentMethod"));

        // Act & Assert
        Assert.False(candidate1.Equals(candidate2));
    }

    [Fact]
    public void Equals_WithMaxOverridesDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            maxOverrides: 10);

        // Act & Assert
        Assert.False(candidate1.Equals(candidate2));
    }

    [Fact]
    public void Equals_WithHasParamDifferent_ShouldReturnFalse()
    {
        // Arrange
        var candidate1 = TestData.CreateSuccessfulParamsCandidate();

        var candidate2 = TestData.CreateSuccessfulParamsCandidate(
            hasParams: false);

        // Act & Assert
        Assert.False(candidate1.Equals(candidate2));
    }
}
