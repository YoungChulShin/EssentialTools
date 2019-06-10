using System;
using System.Linq;
using EssentialTools.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EssentialTools.Tests
{
    [TestClass]
    public class LinqValueCalculatorTest
    {
        private Product[] products =
        {
            new Product {Name = "Kayak", Category="Watersports", Price=275M},
            new Product {Name = "Lifejacket", Category="Watersports", Price=48.95M},
            new Product {Name = "Soccer ball", Category="Soccer", Price=19.50M},
            new Product {Name = "Corner flag", Category="Soccer", Price=34.95M}
        };

        [TestMethod]
        public void Sum_Products_Correctly()
        {
            // Arrange
            var discounter = new MinimumDiscountHelper();
            var target = new LinqValueCalculator(discounter);
            var goalTotal = products.Sum(e => e.Price);

            // Act
            var result = target.ValueProducts(products);

            // Assert
            Assert.AreEqual(goalTotal, result);
        }
    }
}
