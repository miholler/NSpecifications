# NSpecifications

A .NET library implementing the [Specification pattern](http://martinfowler.com/apsupp/spec.pdf) — small, composable, named rule objects that encapsulate query logic. Specifications (specs) work identically against **in-memory collections** (`IEnumerable<T>`) and **(database) queries** (`IQueryable<T>`), compose with `&`, `|`, and `!` operators, and are well suited to be used with LINQ.

## Install

```
dotnet add package NSpecifications
```

Or via Package Manager Console:

```
Install-Package NSpecifications
```

## Features

### Creating a `Spec<T>`

A `Spec<T>` wraps a predicate expression and can be used directly anywhere a collection needs filtering:

```csharp
var highRated = new Spec<Product>(p => p.Rating >= 4.5);

products.Where(highRated);                         // in-memory
dbContext.Products.Where(highRated).ToList();      // database — same code
```

### Compose with operators

Specs can be defined inline or split into smaller named parts and combined with `&`, `|`, and `!`:

```csharp
var electronics = new Spec<Product>(p => p.Category == ProductCategory.Electronics);
var affordable  = new Spec<Product>(p => p.Price < 500m);

var affordableElectronics = electronics & affordable;   // AND
var anyGoodDeal           = electronics | affordable;   // OR
var expensiveElectronics  = electronics & !affordable;  // NOT
```

### Drop in as `Expression<Func<T, bool>>` or `Func<T, bool>`

`Spec<T>` implicitly converts to both delegate types, so it can be passed anywhere they are expected.

```csharp
var affordable = new Spec<Product>(p => p.Price < 500m);

repository.Find(affordable);               // where Find expects Expression<Func<Product, bool>>
products.Where(affordable).ToList();       // works as Func<Product, bool> too
```

### Conditional negation with `==` and `!=`

The `==` and `!=` operators conditionally negate a spec based on a `bool` value, which is useful for optional filters. `spec == true` and `spec != false` both return the spec as-is; `spec == false` and `spec != true` both return its negation.

```csharp
// defined as a static member on the entity or in a dedicated specs class
static readonly Spec<Product> Available = new(p => p.IsAvailable);

// isAvailable = null  → all products
// isAvailable = true  → only available
// isAvailable = false → only unavailable
public Product[] FindProducts(bool? isAvailable = null)
{
    var spec = Spec.Any<Product>();

    if (isAvailable.HasValue)
        spec = spec & (Available == isAvailable.Value);

    return _repository.Find(spec);
}
```

### `Is` / `Are` extension methods

```csharp
var inStock = new Spec<Product>(p => p.IsAvailable);

product.Is(inStock);                                  // single object
new[] { product1, product2, product3 }.Are(inStock);  // all must satisfy
```

### Universal operators on `ISpecification<T>`

Operators (`!`, `&`, `|`, `==`, `!=`) also work on any `ISpecification<T>` implementation via C# extension members.

```csharp
ISpecification<Product> electronics = new Spec<Product>(p => p.Category == ProductCategory.Electronics);
ISpecification<Product> highRated   = new Spec<Product>(p => p.Rating > 4.0);

var combined = electronics & highRated;
```

> **Note:** `ISpecification<T>` operators return `ISpecification<T>`, which cannot be converted to an `Expression<Func<T, bool>>`. See [`Spec<T>` vs `ISpecification<T>` operators](#spect-vs-ispecificationt-operators) in the Technical Reference.

## Real Use Case

Specs are most useful when stored close to the entity they describe — as static members, in a dedicated `ProductSpecs` class, or in a shared `Specs` class. Here is a complete example:

```csharp
public class Product
{
    public string Name { get; }
    public ProductCategory Category { get; }
    public bool IsAvailable { get; }

    public static readonly Spec<Product> Available = new(p => p.IsAvailable);

    public static Spec<Product> InCategory(ProductCategory category) =>
        new(p => p.Category == category);
}
```

```csharp
public IEnumerable<Product> FindProducts(ProductCategory? category = null, bool? isAvailable = null)
{
    var spec = Spec.Any<Product>();

    if (category.HasValue)
        spec = spec & Product.InCategory(category.Value);

    if (isAvailable.HasValue)
        spec = spec & (Product.Available == isAvailable.Value);

    return _repository.Find(spec);
}
```

Specs defined as `static readonly` fields are instantiated once and reused. Specs that depend on a parameter are factory methods that create a new instance per call. Neither needs to be mocked in unit tests.

## Technical Reference

### `Spec<T>`

`Spec<T>` is a `record` that stores a predicate expression, making it implicitly convertible to `Expression<Func<T, bool>>` for use with `IQueryable<T>` providers (e.g. Entity Framework), and to `Func<T, bool>` for any method expecting a predicate delegate.

Operators on `Spec<T>` always return a new `Spec<T>`, so the stored expression is preserved through composition.

Naming tip: following Eric Evans' convention, name specs as objects rather than predicates — `affordable`, not `isAffordable`. A spec is more than a boolean; it's a reusable, composable rule.

### `ISpecification<T>`

For cases where implicit conversion to `Expression<Func<T, bool>>` is not needed, any class can implement `ISpecification<T>` directly:

```csharp
public class InStockSpec : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product product) =>
        product.IsAvailable && product.StockQuantity > 0;
}
```

Composition via `.And()`, `.Or()`, `.Not()` extension methods and operators works the same way, but results are `ISpecification<T>` — suitable for in-memory validation only.

### `Spec<T>` vs `ISpecification<T>` operators

Both types support operators, but the return type is determined by the **declared type** of the operands:

| Left operand         | Right operand         | Result               | Implicitly convertible? |
|----------------------|-----------------------|----------------------|-------------------------|
| `Spec<T>`            | `Spec<T>`             | `Spec<T>`            | ✅ Yes                  |
| `ISpecification<T>`  | `ISpecification<T>`   | `ISpecification<T>`  | ❌ No                   |
| `Spec<T>`            | `ISpecification<T>`   | `ISpecification<T>`  | ❌ No                   |
| `ISpecification<T>`  | `Spec<T>`             | `ISpecification<T>`  | ❌ No                   |

```csharp
Spec<Product> available   = new(p => p.IsAvailable);
Spec<Product> electronics = new(p => p.Category == ProductCategory.Electronics);

var availableElectronics = available & electronics;             // Spec<T>            ✅
dbContext.Products.Where(availableElectronics).ToList();

ISpecification<Product> custom = new MyCustomSpec();
var availableAndCustom = available & custom;                    // ISpecification<T>  ❌
dbContext.Products.Where(availableAndCustom);                   // won't compile
inMemoryList.Where(availableAndCustom.IsSatisfiedBy);           // ✅ in-memory only
```

**Rule of thumb:** keep all operands as `Spec<T>` when database queries are involved.

### Pre-built specifications

```csharp
Spec.Any<Product>()                        // always satisfied  — also Spec<Product>.Any
Spec.None<Product>()                       // never satisfied   — also Spec<Product>.None
Spec.Create<Product>(p => p.IsAvailable)   // explicit factory  — also Spec<Product>.Create(...)
```

## References

- [Specifications — Martin Fowler & Eric Evans](http://martinfowler.com/apsupp/spec.pdf)
- [Domain-Driven Design](https://domainlanguage.com/ddd/)