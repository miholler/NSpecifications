namespace NSpecifications.Tests.Entities;

public class Drink : IDrink
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime ManufacturedOn { get; set; }

    public IList<string> With { get; set; } = new List<string>();

    public static Drink ColdWhiskey(int id = default)
        => new()
        {
            Id = id,
            Name = "Whiskey",
            With = { "Ice" },
            ManufacturedOn = DateTime.Now.AddYears(-11)
        };

    public static Drink AppleJuice(int id = default)
        => new()
        {
            Id = id,
            Name = "Apple Juice",
            ManufacturedOn = DateTime.Now.AddMonths(-1)
        };

    public static Drink OrangeJuice(int id = default)
        => new()
        {
            Id = id,
            Name = "Orange Juice",
            ManufacturedOn = DateTime.Now.AddMonths(-1)
        };

    public static Drink BlackberryJuice(int id = default)
        => new()
        {
            Id = id,
            Name = "Backberry Juice",
            ManufacturedOn = DateTime.Now.AddMonths(-1)
        };
}
