using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDD.Data;

namespace DDD.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange

            var rep = new Mock<CarRepository>().Object;
            
            rep.GetAll();
            
            // Act

            // Assert

        }

        class Car : Entity<int>
        {
            public Car(int id)
            {
                this.Id = id;
            }
            public int Id { get; private set; }

            public override int Identity
            {
                get { return Id; }
            }
        }

        class CarRepository : GenericRepository<Car, int>
        {

        }
    }
}
