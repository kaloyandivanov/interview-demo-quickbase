using Backend.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Backend.Interfaces;

public interface IDbManager
{
    Task<DbConnection> GetConnection();

    Task<IEnumerable<Country>> GetCountries(DbConnection connection);
}
