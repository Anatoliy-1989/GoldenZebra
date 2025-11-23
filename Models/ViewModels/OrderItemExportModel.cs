using BlazorGoldenZebra.Data;
using BlazorGoldenZebra.Utills;

namespace BlazorGoldenZebra.Models.ViewModels
{
    public class OrderItemExportModel
    {
        public int MetalType { get; set; }

        public decimal? WeightDirty { get; set; }

        public decimal WeightClean { get; set; }

        public ProductTypesEnum ProductType { get; set; }

        public decimal Cost { get; set; }

        public decimal DefaultFinessWeight { get; set; }

        public static OrderItemExportModel CreateFromOrderItem(OrderItem oi, List<MetalType> metalTypes)
        {
            return new OrderItemExportModel()
            {
                MetalType = oi.MetalType,
                WeightDirty = oi.WeightDirty,
                WeightClean = oi.WeightClean,
                ProductType = (ProductTypesEnum)oi.ProductType,
                Cost = oi.Cost,
                DefaultFinessWeight = MetalWorker.GetDefaultFinessWeight(
                                    oi.WeightClean,
                                    oi.Fineness,
                                    metalTypes.Single(m => m.Id == oi.MetalType).DefaultFiness)
            };
        }
    }
}
