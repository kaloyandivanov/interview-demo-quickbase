using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("Started");

ICountryPopulationService countryPopulationService = new CountryPopulationService(
        new ConcreteStatService(),
        new CountryStandardizationService(new CsvCountryNameService()),
        new SqliteDbManager()
    );

IEnumerable<Country> countryPopulation = await countryPopulationService.GetCountryPopulation();
countryPopulation = countryPopulation.OrderByDescending(_ => _.Population);

foreach (var country in countryPopulation)
{
    Console.WriteLine($"Country {country.Name}: {country.Population} people.");
}