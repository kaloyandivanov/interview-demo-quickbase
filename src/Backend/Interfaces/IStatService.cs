using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces;

public interface IStatService
{
    IEnumerable<Country> GetCountryPopulations();
    Task<IEnumerable<Country>> GetCountryPopulationsAsync();
}
