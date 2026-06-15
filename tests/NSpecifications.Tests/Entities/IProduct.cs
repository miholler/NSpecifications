namespace NSpecifications.Tests.Entities;

public interface IProduct
{
    int Id { get; }

    string Name { get; }

    ProductCategory Category { get; }

    decimal Price { get; }

    double Rating { get; }

    bool IsAvailable { get; }
}
