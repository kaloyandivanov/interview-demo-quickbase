using Backend.Models;
using System.Collections.Generic;

namespace Backend.Interfaces;

public interface ICountryStandardizationService
{
    IEnumerable<Country> GetStandardCountryNames(IEnumerable<Country> countries);
    IEnumerable<Country> FilterInvalidRecords(IEnumerable<Country> countries);
    IEnumerable<Country> CombineSources(IEnumerable<Country> primarySource, IEnumerable<Country> secondarySource);
}
