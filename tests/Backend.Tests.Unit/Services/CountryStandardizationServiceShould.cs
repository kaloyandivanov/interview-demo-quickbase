using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Moq;

namespace Backend.Tests.Unit.Services;

public class CountryStandardizationServiceShould
{
    private readonly Mock<ICountryNameService> _mockCountryNameService = new();
    private readonly IEnumerable<Country> _expectedResult;

    private readonly ICountryStandardizationService Sut;

    public CountryStandardizationServiceShould()
    {
        _expectedResult = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9),
            new Country("Tuvalu", 8)
        };

        Sut = new CountryStandardizationService(_mockCountryNameService.Object);
    }

    #region GetStandardCountryNames
    [Fact]
    public void GetStandardCountryNames_Should_Replace_Country_Name()
    {
        // Arrange
        _mockCountryNameService.Setup(_ => _.GetStandardCountryName("United States of America")).Returns("U.S.A.");
        _mockCountryNameService.Setup(_ => _.GetStandardCountryName("Republic of Bulgaria")).Returns("Bulgaria");
        _mockCountryNameService.Setup(_ => _.GetStandardCountryName("Tuvalu ")).Returns("Tuvalu");

        var countries = new List<Country>()
        {
            new Country("United States of America", 10),
            new Country("Republic of Bulgaria", 9),
            new Country("Tuvalu ", 8)
        };

        // Act
        var actualResult = Sut.GetStandardCountryNames(countries);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }

    [Fact]
    public void GetStandardCountryNames_When_EmptyList_Should_Return_EmptyList()
    {
        // Act
        var actualResult = Sut.GetStandardCountryNames(new List<Country>());

        // Arrange
        actualResult.Should().BeEmpty();
    }
    #endregion

    #region FilterInvalidRecords
    [Fact]
    public void FilterInvalidRecords_When_ValidList_Should_Return_SameList()
    {
        // Arrange
        var countries = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9),
            new Country("Tuvalu", 8)
        };

        // Act
        var actualResult = Sut.FilterInvalidRecords(countries);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }

    [Theory]
    [InlineData(" ", 1, 0)] // Start of list
    [InlineData(" ", 1, 1)] // Middle of list
    [InlineData(" ", 1, 3)] // End of list
    [InlineData((string)null, 1, 1)] // Null
    [InlineData("", 1, 1)] // Empty string
    [InlineData("   ", 1, 1)] // Tab
    [InlineData("  ", 1, 1)] // Spaces
    [InlineData("Name", 0, 1)] // No population
    [InlineData("Name", -1, 1)] // Negative population
    public void FilterInvalidRecords_When_InvalidRecord_Should_Return_FilteredList(string countryName, int population, int index)
    {
        // Arrange
        var countries = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9),
            new Country("Tuvalu", 8)
        };

        countries.Insert(index, new Country(countryName, population));

        // Act
        var actualResult = Sut.FilterInvalidRecords(countries);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }
    #endregion

    #region FilterInvalidRecords
    [Fact]
    public void CombineSources_Returns_CombinedList()
    {
        // Arrange
        var primarySource = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9)
        };

        var secondarySource = new List<Country>()
        {
            new Country("Tuvalu", 8)
        };

        // Act
        var actualResult = Sut.CombineSources(primarySource, secondarySource);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }

    [Fact]
    public void CombineSources_WhenDuplicates_Returns_UniqueList_With_PrimarySource_Data()
    {
        // Arrange
        var primarySource = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9)
        };

        var secondarySource = new List<Country>()
        {
            new Country("U.S.A.", 11),
            new Country("Bulgaria", 6),
            new Country("Tuvalu", 8)
        };

        // Act
        var actualResult = Sut.CombineSources(primarySource, secondarySource);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }

    [Fact]
    public void CombineSources_When_Empty_PrimarySource_Return_SecondarySource()
    {
        // Arrange
        var primarySource = new List<Country>()
        {
        };

        var secondarySource = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9),
            new Country("Tuvalu", 8)
        };

        // Act
        var actualResult = Sut.CombineSources(primarySource, secondarySource);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }

    [Fact]
    public void CombineSources_When_Empty_SecondarySource_Return_PrimarySource()
    {
        // Arrange
        var primarySource = new List<Country>()
        {
            new Country("U.S.A.", 10),
            new Country("Bulgaria", 9),
            new Country("Tuvalu", 8)
        };

        var secondarySource = new List<Country>()
        {
        };

        // Act
        var actualResult = Sut.CombineSources(primarySource, secondarySource);

        // Arrange
        actualResult.Should().BeEquivalentTo(_expectedResult);
    }
    #endregion
}
