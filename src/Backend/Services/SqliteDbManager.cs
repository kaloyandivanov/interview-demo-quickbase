using Backend.Interfaces;
using Backend.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Backend.Services;

public class SqliteDbManager : IDbManager
{
    public async Task<DbConnection> GetConnection()
    {
        try
        {
            var connection = new SqliteConnection("Data Source=citystatecountry.db;Mode=ReadWrite");
            await connection.OpenAsync();
            return connection;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Country>> GetCountries(DbConnection connection)
    {
        string query = @"
            SELECT
                DISTINCT country.CountryName as Name,
                CAST(SUM(city.Population) AS INT) as Population 
            FROM City city 
            INNER JOIN State state on state.StateId = city.StateId 
            INNER JOIN Country country on country.CountryId = state.CountryId 
            GROUP BY country.CountryName";

        var queryResult = await connection.QueryAsync<Country>(query);

        return queryResult;
    }
}
