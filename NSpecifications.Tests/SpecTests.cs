using System.Linq;
using FluentAssertions;
using NSpecifications.Tests.Entities;
using NUnit.Framework;

namespace NSpecifications.Tests
{
    [TestFixture]
    public class SpecTests
    {
        [Test]
        public void WhiskeyAndCold()
        {
            // Arrange
            Drink coldWhiskeyCandidate = Drink.ColdWhiskey();
            Drink appleJuiceCandidate = Drink.AppleJuice();
            Spec<Drink> whiskeySpec = Spec.Of<Drink>(d => d.Name.ToLower() == "whiskey");
            Spec<Drink> coldDrinkSpec = Spec.Of<Drink>(d => d.With.Any(w => w.ToLower() == "ice"));

            // Act
            var coldWhiskey = whiskeySpec & coldDrinkSpec;

            // Assert
            coldWhiskey.IsSatisfiedBy(coldWhiskeyCandidate).Should().BeTrue();
            coldWhiskey.IsSatisfiedBy(appleJuiceCandidate).Should().BeFalse();
            // And
            coldWhiskeyCandidate.Is(coldWhiskey).Should().BeTrue();
            appleJuiceCandidate.Is(coldWhiskey).Should().BeFalse();
        }

        [Test]
        public void AppleOrOrangeJuice()
        {
            // Arrange
            Drink blackberryJuice = Drink.BlackberryJuice();
            Drink appleJuice = Drink.AppleJuice();
            Drink orangeJuice = Drink.OrangeJuice();
            Spec<Drink> juiceSpec = Spec.Of<Drink>(d => d.With.Any(w => w.ToLower().Contains("juice")));
            Spec<Drink> appleSpec = Spec.Of<Drink>(d => d.With.Any(w => w.ToLower().Contains("apple")));
            Spec<Drink> orangeSpec = Spec.Of<Drink>(d => d.With.Any(w => w.ToLower().Contains("orange")));

            // Act
            var appleOrOrangeJuice = juiceSpec & (appleSpec | orangeSpec);

            // Assert
            appleOrOrangeJuice.IsSatisfiedBy(appleJuice).Should().BeTrue();
            appleOrOrangeJuice.IsSatisfiedBy(orangeJuice).Should().BeTrue();
            appleOrOrangeJuice.IsSatisfiedBy(blackberryJuice).Should().BeFalse();
            // And
            new[] {appleJuice, orangeJuice}.Are(appleOrOrangeJuice).Should().BeTrue();
            blackberryJuice.Is(appleOrOrangeJuice).Should().BeFalse();
        }

        [Test]
        public void And()
        {
            // Arrange
            Drink coldWhiskey = Drink.ColdWhiskey();
            Drink appleJuice = Drink.AppleJuice();
            Spec<Drink> whiskeySpec = Spec.Of<Drink>(d => d.Name.ToLower() == "whiskey");
            Spec<Drink> coldSpec = Spec.Of<Drink>(d => d.With.Any(a => a.ToLower() == "ice"));

            // Act
            var coldWhiskeySpec = whiskeySpec & coldSpec;

            // Assert
            coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
            // And
            coldWhiskey.Is(coldWhiskeySpec).Should().BeTrue();
            appleJuice.Is(coldWhiskeySpec).Should().BeFalse();
        }

    }
}
