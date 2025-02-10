namespace Backend.Models;

public record Country
{
    public string Name { get; set; }
    public int Population { get; set; }

    public Country() { }

    public Country(string name, int population)
    {
        Name = name;
        Population = population;
    }
}
