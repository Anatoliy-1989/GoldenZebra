using BlazorGoldenZebra.Data;
using BlazorGoldenZebra.Models;

namespace BlazorGoldenZebra.Utills
{
    public static class MetalWorker
    {
        public static decimal GetDefaultFinessWeight(decimal weightClean, int fineness, int defaultFiness)
        {
            return weightClean * fineness / defaultFiness;
        }

        public static decimal? GetScrapDefaultFinessWeight(Order order, MetalType metal)
        {
            var metalItems = order.OrderItems.Where(x => x.MetalType == metal.Id && x.ProductType == (int)ProductTypesEnum.Scrap);
            var metalDefaultFinessSum = metalItems.Sum(x =>
                MetalWorker.GetDefaultFinessWeight(x.WeightClean, x.Fineness, metal.DefaultFiness));

            metalDefaultFinessSum = Decimal.Round(metalDefaultFinessSum, 2);

            if (metalDefaultFinessSum == 0)
            {
                return null;
            }

            // Use reflection or a dictionary lookup to get the property value dynamically
            return metalDefaultFinessSum;
        }

        public static decimal? GetProductWeight(Order order)
        {
            var productOrderItems = order.OrderItems.Where(x => x.ProductType == (int)ProductTypesEnum.Product);
            var sum = productOrderItems.Sum(oi => oi.WeightClean);
            sum = Decimal.Round(sum, 2);
            return sum == 0 ? null : sum;
        }

        public static decimal? GetScrapSum(Order order)
        {
            var scrapOrderItems = order.OrderItems.Where(x => x.ProductType == (int)ProductTypesEnum.Scrap);
            var sum = scrapOrderItems.Sum(oi => oi.Cost);
            return sum == 0 ? null : sum;
        }

        public static decimal? GetProductSum(Order order)
        {
            var productOrderItems = order.OrderItems.Where(x => x.ProductType == (int)ProductTypesEnum.Product);
            var sum = productOrderItems.Sum(oi => oi.Cost);
            return sum == 0 ? null : sum;
        }
    }
}
