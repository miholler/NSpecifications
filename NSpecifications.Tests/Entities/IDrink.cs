namespace NSpecifications.Tests.Entities;

public interface IDrink
{
    int Id { get; }

    string Name { get; }

    DateTime ManufacturedOn { get; }

    IList<string> With { get; }
}
