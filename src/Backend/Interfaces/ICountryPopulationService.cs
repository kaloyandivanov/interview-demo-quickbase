using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces;

public interface ICountryPopulationService
{
    Task<IEnumerable<Country>> GetCountryPopulation();
}
