namespace EssentialTools.Models
{
    public interface IDiscountHelper
    {
        decimal ApplyDiscount(decimal totalParam);
    }

    public class DefaultDiscountHelper : IDiscountHelper
    {
        public decimal discountSize;

        public DefaultDiscountHelper(decimal discountSizeParam)
        {
            discountSize = discountSizeParam;
        }

        public decimal ApplyDiscount(decimal totalParam)
        {
            return (totalParam - (discountSize / 100m * totalParam));
        }
    }
}