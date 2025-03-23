using NSpecifications.Tests.Entities;
using NUnit.Framework;
using Shouldly;

namespace NSpecifications.Tests;

[TestFixture]
public sealed class SpecificationTests
{
    [Test]
    public void WhiskeyAndCold()
    {
        // Arrange
        var coldWhiskey = Drink.ColdWhiskey();
        var appleJuice = Drink.AppleJuice();
        var whiskeySpec = Spec.Create<Drink>(d => d.Name.Equals("whiskey", StringComparison.OrdinalIgnoreCase));
        var coldSpec = Spec.Create<Drink>(d => d.With.Any(w => w.Equals("ice", StringComparison.OrdinalIgnoreCase)));

        // Act
        var coldWhiskeySpec = whiskeySpec.And(coldSpec);

        // Assert
        coldWhiskeySpec
            .IsSatisfiedBy(coldWhiskey)
            .ShouldBeTrue();
        coldWhiskeySpec
            .IsSatisfiedBy(appleJuice)
            .ShouldBeFalse();
    }

    [Test]
    public void AppleOrOrangeJuice()
    {
        // Arrange
        var blackberryJuice = Drink.BlackberryJuice();
        var appleJuice = Drink.AppleJuice();
        var orangeJuice = Drink.OrangeJuice();
        var juiceSpec = Spec.Create<Drink>(d => d.Name.Contains("juice", StringComparison.OrdinalIgnoreCase));
        var appleSpec = Spec.Create<Drink>(d => d.Name.Contains("apple", StringComparison.OrdinalIgnoreCase));
        var orangeSpec = Spec.Create<Drink>(d => d.Name.Contains("orange", StringComparison.OrdinalIgnoreCase));

        // Act
        var appleOrOrangeJuiceSpec = juiceSpec.And(appleSpec.Or(orangeSpec));

        // Assert
        appleOrOrangeJuiceSpec
            .IsSatisfiedBy(appleJuice)
            .ShouldBeTrue();
        appleOrOrangeJuiceSpec
            .IsSatisfiedBy(orangeJuice)
            .ShouldBeTrue();
        appleOrOrangeJuiceSpec
            .IsSatisfiedBy(blackberryJuice)
            .ShouldBeFalse();
    }
}
