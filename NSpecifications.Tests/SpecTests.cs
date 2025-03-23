using NSpecifications.Tests.Entities;
using NUnit.Framework;
using Shouldly;

namespace NSpecifications.Tests;

[TestFixture]
public sealed class SpecTests
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
        var coldWhiskeySpec = whiskeySpec & coldSpec;

        // Assert
        coldWhiskeySpec
            .IsSatisfiedBy(coldWhiskey)
            .ShouldBeTrue();
        coldWhiskeySpec
            .IsSatisfiedBy(appleJuice)
            .ShouldBeFalse();
        // And
        coldWhiskey
            .Is(coldWhiskeySpec)
            .ShouldBeTrue();
        appleJuice
            .Is(coldWhiskeySpec)
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
        var appleOrOrangeJuiceSpec = juiceSpec & (appleSpec | orangeSpec);

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
        // And
        Enumerable
            .AsEnumerable([appleJuice, orangeJuice])
            .Are(appleOrOrangeJuiceSpec)
            .ShouldBeTrue();
        blackberryJuice
            .Is(appleOrOrangeJuiceSpec)
            .ShouldBeFalse();
    }

    [Test]
    public void CastUp()
    {
        // Arrange
        var coldWhiskey = Drink.ColdWhiskey();
        var appleJuice = Drink.AppleJuice();
        var whiskeySpec = Spec.Create<IDrink>(d => d.Name.Equals("whiskey", StringComparison.OrdinalIgnoreCase));
        var coldSpec = Spec.Create<IDrink>(d => d.With.Any(w => w.Equals("ice", StringComparison.OrdinalIgnoreCase)));

        // Act
        var coldWhiskeySpec = whiskeySpec & coldSpec;

        //Assert
        Enumerable
            .AsEnumerable([coldWhiskey, appleJuice])
            .Where(coldWhiskeySpec.CastUp<Drink>())
            .ShouldNotContain(appleJuice);
    }

    [Test]
    public void Any()
    {
        // Arrange
        var blackberryJuice = Drink.BlackberryJuice();
        var appleJuice = Drink.AppleJuice();
        var orangeJuice = Drink.OrangeJuice();

        // Assert
        Enumerable
            .AsEnumerable([blackberryJuice, appleJuice, orangeJuice])
            .Are(Spec.Any<Drink>())
            .ShouldBeTrue();
    }

    [Test]
    public void None()
    {
        // Arrange
        var blackberryJuice = Drink.BlackberryJuice();
        var appleJuice = Drink.AppleJuice();
        var orangeJuice = Drink.OrangeJuice();

        // Assert
        Enumerable
            .AsEnumerable([blackberryJuice, appleJuice, orangeJuice])
            .Are(Spec.None<Drink>())
            .ShouldBeFalse();
    }
}
