using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorGoldenZebra.Models.ViewModels
{
    public class OrderExportModel
    {
        public string Date { get; set; }
        public string Place { get; set; }
        public string User { get; set; }
        public int Positions { get; set; }

        public List<OrderItemExportModel> OrderItems { get; set; }
    }
}
