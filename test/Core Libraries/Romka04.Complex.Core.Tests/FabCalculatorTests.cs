using FluentAssertions;
using NUnit.Framework;

namespace Romka04.Complex.Core;

[TestFixture]
public class FabCalculatorTests
{
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(1, ExpectedResult = 1)]
    [TestCase(2, ExpectedResult = 1)]
    [TestCase(3, ExpectedResult = 2)]
    [TestCase(4, ExpectedResult = 3)]
    [TestCase(5, ExpectedResult = 5)]
    [TestCase(6, ExpectedResult = 8)]
    [TestCase(19, ExpectedResult = 4181)]
    public int Fab_ShouldReturnProperResults(int index)
    {
        // act
        var actual = FabCalculator.Fab(index);
        // assert
        return actual;
    }

    [TestCase(-1)]
    [TestCase(-100)]
    public void Fab_IndexLessThanZero_ShouldThrowException(int index)
    {
        // act
        Action act = () => FabCalculator.Fab(index);
        // assert
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    #region Test Helpers

    private FabCalculatorTestsFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new FabCalculatorTestsFixture();
    }

    [TearDown]
    public void TearDown()
    {
    }

    #endregion
}

public class FabCalculatorTestsFixture
{
}