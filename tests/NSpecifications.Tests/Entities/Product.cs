namespace NSpecifications.Tests.Entities;

public sealed record Product(int Id, string Name, ProductCategory Category, decimal Price, double Rating, bool IsAvailable) : IProduct
{
    public static readonly Product TShirt       = new(1, "T-Shirt",       ProductCategory.Clothing,    20m,   3.8, true);
    public static readonly Product Pullover     = new(2, "Pullover",      ProductCategory.Clothing,    45m,   4.1, false);
    public static readonly Product Laptop       = new(3, "Laptop",        ProductCategory.Electronics, 999m,  4.5, true);
    public static readonly Product UsbHub       = new(4, "USB Hub",       ProductCategory.Electronics, 25m,   4.0, true);
    public static readonly Product Monitor      = new(5, "4K Monitor",    ProductCategory.Electronics, 1500m, 4.8, true);
    public static readonly Product GraphicsCard = new(6, "Graphics Card", ProductCategory.Electronics, 800m,  4.7, false);
}
