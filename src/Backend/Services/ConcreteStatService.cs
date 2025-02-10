using Backend.Interfaces;
using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services;

public class ConcreteStatService : IStatService
{
    public IEnumerable<Country> GetCountryPopulations()
    {
        // Pretend this calls a REST API somewhere
        return
        [
                new Country("India",1182105000),
                new Country("United Kingdom",62026962),
                new Country("Chile",17094270),
                new Country("Mali",15370000),
                new Country("Greece",11305118),
                new Country("Armenia",3249482),
                new Country("Slovenia",2046976),
                new Country("Saint Vincent and the Grenadines",109284),
                new Country("Bhutan",695822),
                new Country("Aruba (Netherlands)",101484),
                new Country("Maldives",319738),
                new Country("Mayotte (France)",202000),
                new Country("Vietnam",86932500),
                new Country("Germany",81802257),
                new Country("Botswana",2029307),
                new Country("Togo",6191155),
                new Country("Luxembourg",502066),
                new Country("U.S. Virgin Islands (US)",106267),
                new Country("Belarus",9480178),
                new Country("Myanmar",59780000),
                new Country("Mauritania",3217383),
                new Country("Malaysia",28334135),
                new Country("Dominican Republic",9884371),
                new Country("New Caledonia (France)",248000),
                new Country("Slovakia",5424925),
                new Country("Kyrgyzstan",5418300),
                new Country("Lithuania",3329039),
                new Country("United States of America",309349689)
        ];
    }


    public Task<IEnumerable<Country>> GetCountryPopulationsAsync()
    {
        return Task.FromResult(GetCountryPopulations());
    }
}
