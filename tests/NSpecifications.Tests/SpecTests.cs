using NSpecifications.Tests.Entities;
using NUnit.Framework;
using Shouldly;

namespace NSpecifications.Tests;

[TestFixture]
public sealed class SpecTests
{
    private static readonly Spec<Product> Electronics = new(p => p.Category == ProductCategory.Electronics);
    private static readonly Spec<Product> Available = new(p => p.IsAvailable);

    // -------------------------------------------------------------------------
    // IsSatisfiedBy
    // -------------------------------------------------------------------------

    [Test]
    public void IsSatisfiedBy_Matching_ReturnsTrue()
        => Electronics
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();

    [Test]
    public void IsSatisfiedBy_NotMatching_ReturnsFalse()
        => Electronics
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeFalse();

    // -------------------------------------------------------------------------
    // & operator (AND)
    // -------------------------------------------------------------------------

    [Test]
    public void AndOperator_BothSatisfied_ReturnsTrue()
        => (Electronics & Available)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();

    [Test]
    public void AndOperator_OnlyOneSatisfied_ReturnsFalse()
        => (Electronics & Available)
            .IsSatisfiedBy(Product.GraphicsCard)
            .ShouldBeFalse();

    // -------------------------------------------------------------------------
    // | operator (OR)
    // -------------------------------------------------------------------------

    [Test]
    public void OrOperator_OneSatisfied_ReturnsTrue()
        => (Electronics | Available)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();

    [Test]
    public void OrOperator_NoneSatisfied_ReturnsFalse()
        => (Electronics | Available)
            .IsSatisfiedBy(Product.Pullover)
            .ShouldBeFalse();

    // -------------------------------------------------------------------------
    // ! operator (NOT)
    // -------------------------------------------------------------------------

    [Test]
    public void NotOperator_Satisfied_ReturnsFalse()
        => (!Electronics)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();

    [Test]
    public void NotOperator_NotSatisfied_ReturnsTrue()
        => (!Electronics)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();

    // -------------------------------------------------------------------------
    // == operator (conditional negation)
    // -------------------------------------------------------------------------

    [Test]
    public void EqualityOperator_SpecEqualsTrue_ReturnsSameSpec()
    {
        (Electronics == true)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
        (true == Electronics)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeFalse();
    }

    [Test]
    public void EqualityOperator_SpecEqualsFalse_ReturnsNegation()
    {
        (Electronics == false)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();
        (false == Electronics)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    // -------------------------------------------------------------------------
    // != operator (conditional negation)
    // -------------------------------------------------------------------------

    [Test]
    public void InequalityOperator_SpecNotEqualsFalse_ReturnsSameSpec()
    {
        (Electronics != false)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
        (false != Electronics)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeFalse();
    }

    [Test]
    public void InequalityOperator_SpecNotEqualsTrue_ReturnsNegation()
    {
        (Electronics != true)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();
        (true != Electronics)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    // -------------------------------------------------------------------------
    // Is / Are
    // -------------------------------------------------------------------------

    [Test]
    public void Is_Satisfied_ReturnsTrue()
        => Product
            .Laptop
            .Is(Electronics)
            .ShouldBeTrue();

    [Test]
    public void Is_NotSatisfied_ReturnsFalse()
        => Product
            .TShirt
            .Is(Electronics)
            .ShouldBeFalse();

    [Test]
    public void Are_AllSatisfied_ReturnsTrue()
        => new[] { Product.Laptop, Product.UsbHub }
            .Are(Electronics)
            .ShouldBeTrue();

    [Test]
    public void Are_NotAllSatisfied_ReturnsFalse()
        => new[] { Product.Laptop, Product.TShirt }
            .Are(Electronics)
            .ShouldBeFalse();

    // -------------------------------------------------------------------------
    // CastUp
    // -------------------------------------------------------------------------

    [Test]
    public void CastUp_FiltersCorrectly()
    {
        var electronicsSpec = Spec.Create<IProduct>(p => p.Category == ProductCategory.Electronics);
        var castedSpec = electronicsSpec.CastUp<Product>();

        new[] { Product.Laptop, Product.TShirt }
            .Where(castedSpec)
            .ShouldNotContain(Product.TShirt);
    }

    // -------------------------------------------------------------------------
    // Implicit conversions
    // -------------------------------------------------------------------------

    [Test]
    public void ImplicitConversion_ToExpression()
    {
        var productQuery = new[] { Product.Laptop, Product.TShirt }.AsQueryable();
        productQuery
            .Any(Electronics)
            .ShouldBeTrue();
        productQuery
            .All(Electronics)
            .ShouldBeFalse();
    }

    [Test]
    public void ImplicitConversion_ToFunc()
    {
        var products = new[] { Product.Laptop, Product.TShirt };
        products
            .Any(Electronics)
            .ShouldBeTrue();
        products
            .All(Electronics)
            .ShouldBeFalse();
    }

    // -------------------------------------------------------------------------
    // Factories and pre-built specs
    // -------------------------------------------------------------------------

    [Test]
    public void SpecAny_AlwaysSatisfied()
    {
        var products = new[] { Product.Laptop, Product.TShirt, Product.GraphicsCard };
        products
            .Are(Spec.Any<Product>())
            .ShouldBeTrue();
    }

    [Test]
    public void SpecNone_NeverSatisfied()
    {
        var products = new[] { Product.Laptop, Product.TShirt };
        products
            .Are(Spec.None<Product>())
            .ShouldBeFalse();
    }

    [Test]
    public void SpecCreate_CreatesWorkingSpec()
    {
        var spec = Spec.Create<Product>(p => p.IsAvailable);
        spec.IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
        spec.IsSatisfiedBy(Product.GraphicsCard)
            .ShouldBeFalse();
    }

    [Test]
    public void SpecT_Any_AlwaysSatisfied()
    {
        var products = new[] { Product.Laptop, Product.TShirt, Product.GraphicsCard };
        products
            .Are(Spec<Product>.Any)
            .ShouldBeTrue();
    }

    [Test]
    public void SpecT_None_NeverSatisfied()
    {
        var products = new[] { Product.Laptop, Product.TShirt };
        products
            .Are(Spec<Product>.None)
            .ShouldBeFalse();
    }

    [Test]
    public void SpecT_Create_CreatesWorkingSpec()
    {
        var spec = Spec<Product>.Create(p => p.IsAvailable);
        spec.IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
        spec.IsSatisfiedBy(Product.GraphicsCard)
            .ShouldBeFalse();
    }
}
