using FluentAssertions;
using NSpecifications.Tests.Entities;
using NUnit.Framework;

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
        var whiskeySpec = new Spec<Drink>(d => d.Name.Equals("whiskey", StringComparison.OrdinalIgnoreCase));
        var coldSpec = new Spec<Drink>(d => d.With.Any(w => w.Equals("ice", StringComparison.OrdinalIgnoreCase)));

        // Act
        var coldWhiskeySpec = whiskeySpec.And(coldSpec);

        // Assert
        coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
        coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
    }

    [Test]
    public void AppleOrOrangeJuice()
    {
        // Arrange
        var blackberryJuice = Drink.BlackberryJuice();
        var appleJuice = Drink.AppleJuice();
        var orangeJuice = Drink.OrangeJuice();
        var juiceSpec = new Spec<Drink>(d => d.Name.Contains("juice", StringComparison.OrdinalIgnoreCase));
        var appleSpec = new Spec<Drink>(d => d.Name.Contains("apple", StringComparison.OrdinalIgnoreCase));
        var orangeSpec = new Spec<Drink>(d => d.Name.Contains("orange", StringComparison.OrdinalIgnoreCase));

        // Act
        var appleOrOrangeJuiceSpec = juiceSpec.And(appleSpec.Or(orangeSpec));

        // Assert
        appleOrOrangeJuiceSpec.IsSatisfiedBy(appleJuice).Should().BeTrue();
        appleOrOrangeJuiceSpec.IsSatisfiedBy(orangeJuice).Should().BeTrue();
        appleOrOrangeJuiceSpec.IsSatisfiedBy(blackberryJuice).Should().BeFalse();
    }
}
