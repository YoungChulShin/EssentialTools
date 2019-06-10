using System;
using System.Linq;
using EssentialTools.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

            //var discounter = new MinimumDiscountHelper();
            //var target = new LinqValueCalculator(discounter);
            //var goalTotal = products.Sum(e => e.Price);

            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();   // IDiscounterHelper 인터페이스를 구현하는 Mock 개체를 사용하려고 한다

            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))   // LinqValueCalculator 테스트를 위한 ApplyDiscount 메서드를 선택
                                                                    // It 클래스를 이용해서 매개변수의 값을 설정 
                                                                    // IsAny는 모든 T형식의 값을 지. 이 외에도 조건이나 범위 설정이 가능
                .Returns<decimal>(total => total);  // ApplyDiscount의 결과 값을 리턴
            var target = new LinqValueCalculator(mock.Object);

            // Act
            var result = target.ValueProducts(products);

            // Assert
            Assert.AreEqual(products.Sum(e => e.Price), result);
        }

        private Product[] CrateProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {
            // MinimumDiscounterHelper의 동작을 Mock를 이용해서 구현

            // Arrange
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))   // Setup은 역순으로 동작들을 평가한다
                .Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0)))
                .Throws<System.ArgumentOutOfRangeException>();
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100)))
                .Returns<decimal>(total => total * 0.9M);
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive)))
                .Returns<decimal>(total => total -5);
            var target = new LinqValueCalculator(mock.Object);

            // Act
            decimal FiveDollarDiscount = target.ValueProducts(CrateProduct(5));
            decimal TenDollarDiscount = target.ValueProducts(CrateProduct(10));
            decimal FiftyDollarDiscount = target.ValueProducts(CrateProduct(50));
            decimal HundredDollarDiscount = target.ValueProducts(CrateProduct(100));
            decimal FiveHundredDollarDiscount = target.ValueProducts(CrateProduct(500));

            // Assert
            Assert.AreEqual(5, FiveDollarDiscount, "$5 Fail");
            Assert.AreEqual(5, TenDollarDiscount, "$10 Fail");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 Fail");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 Fail");
            Assert.AreEqual(450, FiveHundredDollarDiscount, "$500 Fail");
            target.ValueProducts(CrateProduct(0));  // Throw ArgumentOutOfRangeException 테스트
        }
    }
}
