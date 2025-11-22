namespace BlazorGoldenZebra.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateCreate { get; set; }

        public int PlaceId { get; set; }

        public string UserId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        public void InitFromOrder(Order order)
        { 
            this.Id = order.Id;
            this.Date = order.Date;
            this.DateCreate = order.DateCreate;
            this.PlaceId = order.PlaceId;
            this.UserId = order.UserId;

            foreach (var orderItem in order.OrderItems)
            {
                var orderItemView = new OrderItemViewModel();
                orderItemView.InitFromOrderItem(orderItem);
                this.OrderItems.Add(orderItemView);
            }
        }
    }
}
