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
            ASpec<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ASpec<Drink> coldDrinkSpec = new Spec<Drink>(d => d.With.Any(w => w.ToLower() == "ice"));

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
            ASpec<Drink> juiceSpec = new Spec<Drink>(d => d.With.Any(w => w.ToLower().Contains("juice")));
            ASpec<Drink> appleSpec = new Spec<Drink>(d => d.With.Any(w => w.ToLower().Contains("apple")));
            ASpec<Drink> orangeSpec = new Spec<Drink>(d => d.With.Any(w => w.ToLower().Contains("orange")));

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
            ASpec<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ASpec<Drink> coldSpec = new Spec<Drink>(d => d.With.Any(a => a.ToLower() == "ice"));

            // Act
            var coldWhiskeySpec = whiskeySpec & coldSpec;

            // Assert
            coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
            // And
            coldWhiskey.Is(coldWhiskeySpec).Should().BeTrue();
            appleJuice.Is(coldWhiskeySpec).Should().BeFalse();
        }

        [Test]
        public void Any()
        {
            // Arrange
            Drink blackberryJuice = Drink.BlackberryJuice();
            Drink appleJuice = Drink.AppleJuice();
            Drink orangeJuice = Drink.OrangeJuice();
            
            // Assert
            new[] { blackberryJuice, appleJuice, orangeJuice }.Are(Spec.Any<Drink>()).Should().BeTrue();
        }

        [Test]
        public void Not()
        {
            // Arrange
            Drink blackberryJuice = Drink.BlackberryJuice();
            Drink appleJuice = Drink.AppleJuice();
            Drink orangeJuice = Drink.OrangeJuice();

            // Assert
            new[] { blackberryJuice, appleJuice, orangeJuice }.Are(Spec.Not<Drink>()).Should().BeFalse();
        }

    }
}
