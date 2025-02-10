using System.Collections.Generic;

namespace Backend.Interfaces;

public interface ICountryNameService
{
    string GetStandardCountryName(string country);
}
