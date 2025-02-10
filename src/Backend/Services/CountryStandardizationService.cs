using Backend.Interfaces;
using Backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services;

public class CountryStandardizationService : ICountryStandardizationService
{
    ICountryNameService _countryNameService;

    public CountryStandardizationService(ICountryNameService countryNameService)
    {
        _countryNameService = countryNameService;
    }

    /// <summary>
    /// Replaces the country name with a standardized one
    /// </summary>
    /// <param name="countries"></param>
    /// <returns></returns>
    public IEnumerable<Country> GetStandardCountryNames(IEnumerable<Country> countries) =>
        countries.Select(c => new Country(_countryNameService.GetStandardCountryName(c.Name), c.Population));

    /// <summary>
    /// Filters invalid countries from the list
    /// Invalid countries are those without a name and/or positive population
    /// </summary>
    /// <param name="countries"></param>
    /// <returns></returns>
    public IEnumerable<Country> FilterInvalidRecords(IEnumerable<Country> countries)
        => countries.Where(c => c.Population > 0 && !string.IsNullOrWhiteSpace(c.Name));

    /// <summary>
    /// Combine two sources of countries
    /// In case of duplication, prefer the value from the primary source
    /// </summary>
    /// <param name="primarySource"></param>
    /// <param name="additionalSources"></param>
    /// <returns></returns>
    public IEnumerable<Country> CombineSources(IEnumerable<Country> primarySource, IEnumerable<Country> secondarySource)
    {
        var countriesDictionary = primarySource
            .ToDictionary(
                country => country.Name,
                country => country
            );

        secondarySource
            .ToList()
            .ForEach(country => countriesDictionary.TryAdd(country.Name, country));

        return countriesDictionary.Values;
    }
}
