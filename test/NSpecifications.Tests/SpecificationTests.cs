using NSpecifications.Tests.Entities;
using NUnit.Framework;
using Shouldly;

namespace NSpecifications.Tests;

[TestFixture]
public sealed class SpecificationTests
{
    private static readonly Spec<Product> Electronics = new(p => p.Category == ProductCategory.Electronics);
    private static readonly Spec<Product> Available = new(p => p.IsAvailable);

    // -------------------------------------------------------------------------
    // .Not() / .And() / .Or() extension methods
    // -------------------------------------------------------------------------

    [Test]
    public void Not_Satisfied_ReturnsFalse()
    {
        ISpecification<Product> spec = Electronics;
        spec.Not()
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();
    }

    [Test]
    public void Not_NotSatisfied_ReturnsTrue()
    {
        ISpecification<Product> spec = Electronics;
        spec.Not()
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    [Test]
    public void And_BothSatisfied_ReturnsTrue()
    {
        ISpecification<Product> spec = Electronics.And(Available);
        spec.IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
    }

    [Test]
    public void And_OnlyOneSatisfied_ReturnsFalse()
    {
        ISpecification<Product> spec = Electronics.And(Available);
        spec.IsSatisfiedBy(Product.GraphicsCard)
            .ShouldBeFalse();
    }

    [Test]
    public void Or_OneSatisfied_ReturnsTrue()
    {
        ISpecification<Product> spec = Electronics.Or(Available);
        spec.IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    [Test]
    public void Or_NoneSatisfied_ReturnsFalse()
    {
        ISpecification<Product> spec = Electronics.Or(Available);
        spec.IsSatisfiedBy(Product.Pullover)
            .ShouldBeFalse();
    }

    // -------------------------------------------------------------------------
    // Extension member operators
    // -------------------------------------------------------------------------

    [Test]
    public void NotOperator_Satisfied_ReturnsFalse()
    {
        ISpecification<Product> spec = Electronics;
        (!spec)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();
    }

    [Test]
    public void NotOperator_NotSatisfied_ReturnsTrue()
    {
        ISpecification<Product> spec = Electronics;
        (!spec)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    [Test]
    public void AndOperator_BothSatisfied_ReturnsTrue()
    {
        ISpecification<Product> left = Electronics;
        ISpecification<Product> right = Available;
        (left & right)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
    }

    [Test]
    public void AndOperator_OnlyOneSatisfied_ReturnsFalse()
    {
        ISpecification<Product> left = Electronics;
        ISpecification<Product> right = Available;
        (left & right)
            .IsSatisfiedBy(Product.GraphicsCard)
            .ShouldBeFalse();
    }

    [Test]
    public void OrOperator_OneSatisfied_ReturnsTrue()
    {
        ISpecification<Product> left = Electronics;
        ISpecification<Product> right = Available;
        (left | right)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    [Test]
    public void OrOperator_NoneSatisfied_ReturnsFalse()
    {
        ISpecification<Product> left = Electronics;
        ISpecification<Product> right = Available;
        (left | right)
            .IsSatisfiedBy(Product.Pullover)
            .ShouldBeFalse();
    }

    [Test]
    public void EqualityOperator_SpecEqualsTrue_ReturnsSameSpec()
    {
        ISpecification<Product> spec = Electronics;
        (spec == true)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
        (true == spec)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeFalse();
    }

    [Test]
    public void EqualityOperator_SpecEqualsFalse_ReturnsNegation()
    {
        ISpecification<Product> spec = Electronics;
        (spec == false)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();
        (false == spec)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }

    [Test]
    public void InequalityOperator_SpecNotEqualsFalse_ReturnsSameSpec()
    {
        ISpecification<Product> spec = Electronics;
        (spec != false)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeTrue();
        (false != spec)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeFalse();
    }

    [Test]
    public void InequalityOperator_SpecNotEqualsTrue_ReturnsNegation()
    {
        ISpecification<Product> spec = Electronics;
        (spec != true)
            .IsSatisfiedBy(Product.Laptop)
            .ShouldBeFalse();
        (true != spec)
            .IsSatisfiedBy(Product.TShirt)
            .ShouldBeTrue();
    }
}
