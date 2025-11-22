namespace BlazorGoldenZebra.Models.ViewModels
{
    public class OrderStatisticsViewModel
    {
        public string Place { get; set; }

        public int OrderAmount { get; set; }
        public int Positions { get; set; }

        public List<OrderItemExportModel> OrderItems { get; set; } = new List<OrderItemExportModel>();

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
