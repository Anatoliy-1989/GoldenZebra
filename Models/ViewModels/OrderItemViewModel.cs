using BlazorGoldenZebra.Utills;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorGoldenZebra.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }

        public int MetalType { get; set; }

        public int ProductType { get; set; }

        public int OrderId { get; set; }

        public decimal WeightDirty { get; set; }

        public decimal? WeightClean { get; set; }

        public int Fineness { get; set; }

        [DisplayName("Металл")]
        public string MetalTypeName { get; set; }

        [DisplayName("Тип")]
        public string ProductTypeName { get; set; }

        public int DefaultMetalFiness { get; set; }

        public decimal Cost { get; set; }

        public string DefaultMetalFinessWeight 
        { 
            get 
            {
                if (this.DefaultMetalFiness > 0 && this.WeightClean > 0 && this.Fineness > 0)
                {
                    var inDefaultFiness = MetalWorker.GetDefaultFinessWeight(this.WeightClean, this.Fineness, this.DefaultMetalFiness);
                    return inDefaultFiness.ToString("F2");
                }

                return string.Empty;
            } 
        }

        public void InitFromOrderItem(OrderItem orderItem)
        { 
            this.Id = orderItem.Id;
            this.WeightDirty = orderItem.WeightDirty;
            this.WeightClean = orderItem.WeightClean;
            this.Fineness = orderItem.Fineness;
            this.MetalType = orderItem.MetalType;
            this.ProductType = orderItem.ProductType;
            this.OrderId = orderItem.OrderId;
            this.Cost = orderItem.Cost;
        }
    }
}
