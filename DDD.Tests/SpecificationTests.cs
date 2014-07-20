using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DDD.Tests
{
    [TestClass]
    public class SpecificationTests
    {
        EvenNumberSpecification _evenSpec = new EvenNumberSpecification();
        PositiveNumberSpecification _positiveSpec = new PositiveNumberSpecification();

        [TestMethod]
        public void IsSatisfiedBy()
        {
            // Assert
            Assert.IsTrue(_evenSpec.IsSatisfiedBy(2));
            Assert.IsFalse(_evenSpec.IsSatisfiedBy(1));

            // Assert
            Assert.IsTrue(_positiveSpec.IsSatisfiedBy(15));
            Assert.IsFalse(_positiveSpec.IsSatisfiedBy(-5));
        }

        [TestMethod]
        public void AndOperator()
        {
            Assert.IsTrue(_positiveSpec.And(_evenSpec.Not()).IsSatisfiedBy(5));
            Assert.IsTrue(_positiveSpec.And(_evenSpec).IsSatisfiedBy(4));
            Assert.IsFalse(_positiveSpec.And(_evenSpec.Not()).IsSatisfiedBy(4));
            Assert.IsFalse(_positiveSpec.And(_evenSpec).IsSatisfiedBy(5));

            Assert.IsTrue((_positiveSpec & !_evenSpec).IsSatisfiedBy(5));
            Assert.IsTrue((_positiveSpec & _evenSpec).IsSatisfiedBy(4));
            Assert.IsFalse((_positiveSpec & !_evenSpec).IsSatisfiedBy(4));
            Assert.IsFalse((_positiveSpec & _evenSpec).IsSatisfiedBy(5));
        }

        [TestMethod]
        public void Paged() 
        {
            Assert.IsFalse((_positiveSpec & _evenSpec).Paged(10, 1).IsSatisfiedBy(5));
        }

        [TestMethod]
        public void SpecificationFactoryMethod()
        {
            var positiveNumber = Specification.Create<int>(num => num >= 0);
            Assert.IsTrue(positiveNumber.IsSatisfiedBy(2));
            Assert.IsFalse(positiveNumber.IsSatisfiedBy(-15));
        }

        class EvenNumberSpecification : Specification<int>
        {
            public override bool IsSatisfiedBy(int number)
            {
                return number % 2 == 0;
            }
        }

        class PositiveNumberSpecification : Specification<int>
        {
            public override bool IsSatisfiedBy(int number)
            {
                return number > 0;
            }
        }
    }
}
