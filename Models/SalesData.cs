namespace DominosReportingOptimization.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Location { get; set; }
        public string? Manager { get; set; }
        public DateTime OpenedDate { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public int DeliveryTimeMinutes { get; set; }
        public string? OrderStatus { get; set; }
        public int ItemCount { get; set; }
    }

    public class Product 
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}