using EssentialTools.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssentialTools.Tests
{
    // - 테스트 케이스
    // - 합계가 $100를 초과하면 10퍼센트의 할인율이 적용된다
    // - 합계가 $10에서 $100사이면 $5의 할인율이 적용된다
    // - 합계가 $10 미만이면 할인율은 적용되지 않는다
    // - 합계가 $0보다 작으면 `ArgumentOutOfRangeException` 예외가 발생한다

    [TestClass]
    public class DiscounterHelperTest
    {
        private IDiscountHelper GetTestObject()
        {
            return new MinimumDiscountHelper();
        }

        // 합계가 $100를 초과하면 10퍼센트의 할인율이 적용된다
        [TestMethod]
        public void Discount_Above_100()
        {
            // Arrange
            IDiscountHelper target = GetTestObject();
            decimal total = 200;

            // Act
            var discountedTotal = target.ApplyDiscount(total);

            // Assert
            Assert.AreEqual(total * 0.9M, discountedTotal);
        }

        // 합계가 $10에서 $100사이면 $5의 할인율이 적용된다
        [TestMethod]
        public void Discount_Between_10_And_100()
        {
            // Arange
            IDiscountHelper target = GetTestObject();

            // Act
            decimal tenDollorDiscount = target.ApplyDiscount(10);
            decimal hundredDollorDiscount = target.ApplyDiscount(100);
            decimal fiftyDollorDiscount = target.ApplyDiscount(50);

            // Assert
            Assert.AreEqual(5, tenDollorDiscount, "$10 discount is wrong");
            Assert.AreEqual(95, hundredDollorDiscount, "$100 discount is wrong");
            Assert.AreEqual(45, fiftyDollorDiscount, "$50 discount is wrong");
        }

        // 합계가 $10 미만이면 할인율은 적용되지 않는다
        [TestMethod]
        public void Discount_Less_Than_10()
        {
            // Arange
            IDiscountHelper target = GetTestObject();

            // Act
            decimal discount5 = target.ApplyDiscount(5);
            decimal discount0 = target.ApplyDiscount(0);

            // Assert
            Assert.AreEqual(5, discount5);
            Assert.AreEqual(0, discount0);
        }

        // 합계가 $0보다 작으면 `ArgumentOutOfRangeException` 예외가 발생한다
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Discount_Negative_Total()
        {
            // Arange
            IDiscountHelper target = GetTestObject();

            // Act
            target.ApplyDiscount(-1);
        }
    }
}
