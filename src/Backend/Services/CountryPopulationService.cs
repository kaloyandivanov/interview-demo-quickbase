using Backend.Interfaces;
using Backend.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Backend.Services;

public class CountryPopulationService : ICountryPopulationService
{
    private readonly IStatService _statService;
    private readonly ICountryStandardizationService _countryStandardizationService;
    private readonly IDbManager _dbManager;

    public CountryPopulationService(
        IStatService statService,
        ICountryStandardizationService countryStandardizationService,
        IDbManager dbManager)
    {
        _statService = statService;
        _countryStandardizationService = countryStandardizationService;
        _dbManager = dbManager;
    }

    /// <summary>
    /// Retrieve country population from DbManager and StatService, filter the results, standardize the country names and combine the lists into one
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Country>> GetCountryPopulation()
    {
        using DbConnection conn = await _dbManager.GetConnection();

        IEnumerable<Country> dbPopulation = await _dbManager.GetCountries(conn);
        IEnumerable<Country> statPopulation = await _statService.GetCountryPopulationsAsync();

        var filteredDbPopulation = _countryStandardizationService.FilterInvalidRecords(dbPopulation);
        var filteredStatPopulation = _countryStandardizationService.FilterInvalidRecords(statPopulation);

        var standardizedDbPopulation = _countryStandardizationService.GetStandardCountryNames(filteredDbPopulation);
        var standardizedStatPopulation = _countryStandardizationService.GetStandardCountryNames(filteredStatPopulation);

        return _countryStandardizationService.CombineSources(standardizedDbPopulation, standardizedStatPopulation);
    }
}
