using System.Linq;
using FluentAssertions;
using NSpecifications.Tests.Entities;
using NUnit.Framework;

namespace NSpecifications.Tests
{
    [TestFixture]
    public class ISpecificationTests
    {
        [Test]
        public void WhiskeyAndCold()
        {
            // Arrange
            Drink coldWhiskey = Drink.ColdWhiskey();
            Drink appleJuice = Drink.AppleJuice();
            ISpecification<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ISpecification<Drink> coldSpec = new Spec<Drink>(d => d.With.Any(w => w.ToLower() == "ice"));

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
            Drink blackberryJuice = Drink.BlackberryJuice();
            Drink appleJuice = Drink.AppleJuice();
            Drink orangeJuice = Drink.OrangeJuice();
            ISpecification<Drink> juiceSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("juice"));
            ISpecification<Drink> appleSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("apple"));
            ISpecification<Drink> orangeSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("orange"));

            // Act
            var appleOrOrangeJuiceSpec = juiceSpec.And(appleSpec.Or(orangeSpec));

            // Assert
            appleOrOrangeJuiceSpec.IsSatisfiedBy(appleJuice).Should().BeTrue();
            appleOrOrangeJuiceSpec.IsSatisfiedBy(orangeJuice).Should().BeTrue();
            appleOrOrangeJuiceSpec.IsSatisfiedBy(blackberryJuice).Should().BeFalse();
        }

        [Test]
        public void And()
        {
            // Arrange
            Drink coldWhiskey = Drink.ColdWhiskey();
            Drink appleJuice = Drink.AppleJuice();
            ISpecification<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ISpecification<Drink> coldSpec = new Spec<Drink>(d => d.With.Any(a => a.ToLower() == "ice"));

            // Act
            var coldWhiskeySpec = whiskeySpec.And(coldSpec);

            // Assert
            coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
        }

    }
}
