using NSpecifications.Tests.Entities;
using NSpecifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpecifications.Tests.Interface
{
    [TestFixture]
    public class CompositionTests
    {


        [Test]
        public void WhiskeyAndCold()
        {
            // Arrange
            Drink coldWhiskey = Drink.ColdWhiskey();
            Drink appleJuice = Drink.AppleJuice();
            ISpecification<Drink> whiskeySpec = Spec.For<Drink>(d => d.Name.ToLower() == "whiskey");
            ISpecification<Drink> coldSpec = Spec.For<Drink>(d => d.With.Any(w => w.ToLower() == "ice"));

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
            ISpecification<Drink> juiceSpec = Spec.For<Drink>(d => d.With.Any(w => w.ToLower().Contains("juice")));
            ISpecification<Drink> appleSpec = Spec.For<Drink>(d => d.With.Any(w => w.ToLower().Contains("apple")));
            ISpecification<Drink> orangeSpec = Spec.For<Drink>(d => d.With.Any(w => w.ToLower().Contains("orange")));

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
            ISpecification<Drink> whiskeySpec = Spec.For<Drink>(d => d.Name.ToLower() == "whiskey");
            ISpecification<Drink> coldSpec = Spec.For<Drink>(d => d.With.Any(a => a.ToLower() == "ice"));

            // Act
            var coldWhiskeySpec = whiskeySpec.And(coldSpec);

            // Assert
            coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
        }

    }
}
