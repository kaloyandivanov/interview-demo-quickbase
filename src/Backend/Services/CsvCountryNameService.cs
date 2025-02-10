using Backend.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace Backend.Services;

public class CsvCountryNameService : ICountryNameService
{
    private Dictionary<string, string> _countryNames;
    private string filename = "country_aliases.csv";

    public CsvCountryNameService()
    {
        LoadFromCVS();
    }

    private void LoadFromCVS()
    {
        _countryNames = new Dictionary<string, string>();

        using TextFieldParser parser = new TextFieldParser(filename);

        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        while (!parser.EndOfData)
        {
            string[] fields = parser.ReadFields();
            // First column is the standard name, always available
            for (int column = 0; column < fields.Length; column++)
            {
                if (!string.IsNullOrWhiteSpace(fields[column]))
                    // All aliases are stored as keys, the standard name - as a value
                    _countryNames.TryAdd(CleanKey(fields[column]), fields[0]);
            }
        }
    }

    // Keys should always be trimmed and in lower case
    private string CleanKey(string key) => key.Trim().ToLower();

    public string GetStandardCountryName(string countryName)
    {
        string cleanCountryName = CleanKey(countryName);
        if (_countryNames.ContainsKey(cleanCountryName))
            return _countryNames[cleanCountryName];

        // Default to incoming value if no match is found
        return countryName;
    }
}
