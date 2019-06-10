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
    }
}
