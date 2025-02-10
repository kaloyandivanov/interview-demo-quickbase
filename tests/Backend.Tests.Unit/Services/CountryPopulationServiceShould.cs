using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Moq;
using System.Data.Common;

namespace Backend.Tests.Unit.Services;

public class CountryPopulationServiceShould
{
    private readonly Mock<IDbManager> _mockDbManager = new();
    private readonly Mock<IStatService> _mockStatService = new();
    private readonly Mock<ICountryStandardizationService> _mockCountryStandardizationService = new();

    private readonly ICountryPopulationService Sut;

    public CountryPopulationServiceShould()
    {
        Sut = new CountryPopulationService(
            _mockStatService.Object,
            _mockCountryStandardizationService.Object,
            _mockDbManager.Object
            );
    }


    [Fact]
    public async Task GetCountryPopulation_Should_Filter_Standardize_Combine_Db_Stats_Sources()
    {
        // Arrange 
        var dbCountries = new List<Country>() {
            new Country("U.S.A.", 10),
            new Country("Invalid", -1)
        };

        var dbCountriesFiltered = new List<Country>() {
            new Country("U.S.A.", 10)
        };

        var dbCountriesStandardized = new List<Country>() {
            new Country("United States of America", 10),
        };

        var statCountries = new List<Country>() {
            new Country("U.K.", 5),
            new Country("", 1)
        };

        var statCountriesFiltered = new List<Country>() {
            new Country("U.K.", 5)
        };

        var statCountriesStandardized = new List<Country>() {
            new Country("United Kingdom", 5)
        };

        var combinedCountries = new List<Country>()
        {
            new Country("United States of America", 10),
            new Country("United Kingdom", 5)
        };

        _mockDbManager.Setup(_ => _.GetCountries(It.IsAny<DbConnection>())).ReturnsAsync(dbCountries);
        _mockStatService.Setup(_ => _.GetCountryPopulationsAsync()).ReturnsAsync(statCountries);

        _mockCountryStandardizationService
            .Setup(_ => _.FilterInvalidRecords(It.Is<IEnumerable<Country>>(countries =>
                AreListsEqual(countries, dbCountries)
            ))).Returns(dbCountriesFiltered);
        _mockCountryStandardizationService
            .Setup(_ => _.FilterInvalidRecords(It.Is<IEnumerable<Country>>(countries =>
                AreListsEqual(countries, statCountries)
            ))).Returns(statCountriesFiltered);

        _mockCountryStandardizationService
            .Setup(_ => _.GetStandardCountryNames(It.Is<IEnumerable<Country>>(countries =>
                AreListsEqual(countries, dbCountriesFiltered)
            ))).Returns(dbCountriesStandardized);

        _mockCountryStandardizationService
            .Setup(_ => _.GetStandardCountryNames(It.Is<IEnumerable<Country>>(countries =>
                AreListsEqual(countries, statCountriesFiltered)
            ))).Returns(statCountriesStandardized);

        _mockCountryStandardizationService.Setup(_ => _.CombineSources(It.IsAny<IEnumerable<Country>>(), It.IsAny<IEnumerable<Country>>())).Returns(combinedCountries);

        // Act
        var actualResult = await Sut.GetCountryPopulation();

        // Assert
        actualResult.Should().BeEquivalentTo(combinedCountries);
        // Data is retrieved
        _mockDbManager.Verify(_ => _.GetCountries(It.IsAny<DbConnection>()), Times.Once());
        _mockStatService.Verify(_ => _.GetCountryPopulationsAsync(), Times.Once());
        // Results are filtered and standardized
        _mockCountryStandardizationService.Verify(_ => _.FilterInvalidRecords(It.IsAny<IEnumerable<Country>>()), Times.Exactly(2));
        _mockCountryStandardizationService.Verify(_ => _.GetStandardCountryNames(It.IsAny<IEnumerable<Country>>()), Times.Exactly(2));

        // Verify combined is called in proper order of data sources
        _mockCountryStandardizationService.Verify(_ => _.CombineSources(
            It.Is<IEnumerable<Country>>(
                countries => VerifyListsAreEqual(countries, dbCountriesStandardized)
            ),
            It.Is<IEnumerable<Country>>(
                countries => VerifyListsAreEqual(countries, statCountriesStandardized))
            ), Times.Exactly(1));
    }

    private bool VerifyListsAreEqual(IEnumerable<Country> actual, IEnumerable<Country> expected)
    {
        actual.Should().BeEquivalentTo(expected);
        return true;
    }

    private bool AreListsEqual(IEnumerable<Country> firstSet, IEnumerable<Country> secondSet)
        => Enumerable.SequenceEqual(firstSet, secondSet);
}
