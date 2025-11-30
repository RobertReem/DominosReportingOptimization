using DominosReportingOptimization.Models;

namespace DominosReportingOptimization.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Stores.Any())
            {
                return;
            }
        

            // Add Stores
            var stores = new List<Store>
            {
                new Store { StoreName = "Ann Arbor Downtown", Location = "Ann Arbor, MI", Manager = "John Smith", OpenedDate = new DateTime(2010, 5, 15) },
                new Store { StoreName = "Ann Arbor West Side", Location = "Ann Arbor, MI", Manager = "Sarah Johnson", OpenedDate = new DateTime(2012, 3, 20) },
                new Store { StoreName = "Ypsilanti Main", Location = "Ypsilanti, MI", Manager = "Mike Davis", OpenedDate = new DateTime(2008, 1, 10) },
                new Store { StoreName = "Canton Center", Location = "Canton, MI", Manager = "Jennifer Lee", OpenedDate = new DateTime(2015, 7, 25) },
                new Store { StoreName = "Plymouth North", Location = "Plymouth, MI", Manager = "Robert Martinez", OpenedDate = new DateTime(2011, 11, 5) }
            };

            context.Stores.AddRange(stores);
            context.SaveChanges();
        
        
            // Add Products
            var products = new List<Product>
            {
                new Product { ProductName = "Large Pepperoni Pizza", Category = "Pizza", Price = 14.99m },
                new Product { ProductName = "Large ExtravaganZZa", Category = "Pizza", Price = 18.99m },
                new Product { ProductName = "Medium MeatZZa Mania", Category = "Pizza", Price = 12.99m },
                new Product { ProductName = "Cali Chicken Bacon Ranch", Category = "Pizza", Price = 13.99m },
                new Product { ProductName = "Honolulu Hawaiian", Category = "Pizza", Price = 13.99m },
                new Product { ProductName = "Buffalo Chicken", Category = "Wings", Price = 7.99m },
                new Product { ProductName = "Marinated Buffalo Chicken", Category = "Wings", Price = 8.99m },
                new Product { ProductName = "Parmesan Bread Bites", Category = "Sides", Price = 5.99m },
                new Product { ProductName = "Marbled Cookie Brownie", Category = "Dessert", Price = 3.99m },
                new Product { ProductName = "Coca-Cola 2L", Category = "Beverage", Price = 2.99m }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // Add Orders
            var random = new Random(42); // Seed for reproducibility
            var orders = new List<Order>();

            for (int i = 0; i < 500; i++)
            {
                var storeId = stores[random.Next(stores.Count)].StoreId;
                var orderDate = DateTime.Now.AddDays(-random.Next(90)); // Orders from last 90 days
                var itemCount = random.Next(1, 6);
                var orderTotal = (decimal)(random.NextDouble() * 60 + 15); // $15-$75

                orders.Add(new Order
                {
                    StoreId = storeId,
                    OrderDate = orderDate,
                    OrderTotal = Math.Round(orderTotal, 2),
                    DeliveryTimeMinutes = random.Next(15, 60),
                    OrderStatus = "Delivered",
                    ItemCount = itemCount
                });
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();

            // Add Order Items
            var orderItems = new List<OrderItem>();

            foreach (var order in context.Orders.ToList())
            {
                int itemsInOrder = random.Next(1, 4);
                decimal runningTotal = 0;

                for (int i = 0; i < itemsInOrder; i++)
                {
                    var product = products[random.Next(products.Count)];
                    var quantity = random.Next(1, 3);
                    var lineTotal = product.Price * quantity;
                    runningTotal += lineTotal;

                    orderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        LineTotal = Math.Round(lineTotal, 2)
                    });
                }
            }

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();
        }
    }
}