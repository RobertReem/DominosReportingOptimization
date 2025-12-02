using Microsoft.EntityFrameworkCore;
using DominosReportingOptimization.Data;
using System.Diagnostics;

namespace DominosReportingOptimization.Services
{
    public class StoredProcedureService
    {
        private readonly ApplicationDbContext _context;

        public StoredProcedureService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Example 1: Orders with Stores - Unoptimized
        public async Task<dynamic> GetOrdersWithStoresUnoptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _context.Database
                .SqlQueryRaw<OrderDTO>("EXEC sp_GetOrdersWithStoresUnoptimized")
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"GetOrdersWithStoresUnoptimized -- Total time took: {totalElapsed.TotalMilliseconds} ms");

            return new
            {
                Data = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                QueryType = "Unoptimized",
                Description = "Simple SELECT without JOIN - requires additional queries to get store info"
            };
        }

        // Example 1: Orders with Stores - Optimized
        public async Task<dynamic> GetOrdersWithStoresOptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _context.Database
                .SqlQueryRaw<OrderStoreDTO>("EXEC sp_GetOrdersWithStoresOptimized")
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"GetOrdersWithStoresOptimized -- Total time took: {totalElapsed.TotalMilliseconds} ms");

            return new
            {
                Data = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                QueryType = "Optimized",
                Description = "Single query with JOIN and window functions for store-level metrics"
            };
        }

        // Example 2: Product Sales - Unoptimized
        public async Task<dynamic> GetProductSalesUnoptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _context.Database
                .SqlQueryRaw<ProductSalesDTO>("EXEC sp_GetProductSalesUnoptimized")
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"GetProductSalesUnoptimized -- Total time took: {totalElapsed.TotalMilliseconds} ms");

            return new
            {
                Data = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                QueryType = "Unoptimized",
                Description = "Missing index on foreign key - table scan required"
            };
        }

        // Example 2: Product Sales - Optimized
        public async Task<dynamic> GetProductSalesOptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _context.Database
                .SqlQueryRaw<ProductSalesDTO>("EXEC sp_GetProductSalesOptimized")
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"GetProductSalesOptimized -- Total time took: {totalElapsed.TotalMilliseconds} ms");

            return new
            {
                Data = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                QueryType = "Optimized",
                Description = "Index on OrderItems.ProductId for efficient joins"
            };
        }

        // Example 3: Store Rankings - Unoptimized
        public async Task<dynamic> GetStoreRankingsUnoptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _context.Database
                .SqlQueryRaw<StoreRankingUnoptimizedDTO>("EXEC sp_GetStoreRankingsUnoptimized")
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"GetStoreRankingsUnoptimized -- Total time took: {totalElapsed.TotalMilliseconds} ms");

            return new
            {
                Data = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                QueryType = "Unoptimized",
                Description = "Correlated subqueries - runs subquery for each store (N+1 problem)"
            };
        }

        // Example 3: Store Rankings - Optimized
        public async Task<dynamic> GetStoreRankingsOptimized()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _context.Database
                .SqlQueryRaw<StoreRankingOptimizedDTO>("EXEC sp_GetStoreRankingsOptimized")
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"GetStoreRankingsOptimized -- Total time took: {totalElapsed.TotalMilliseconds} ms");

            return new
            {
                Data = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                QueryType = "Optimized",
                Description = "Window functions and CTE - single pass through data"
            };
        }
    }

    // DTOs for stored procedure results
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public int DeliveryTimeMinutes { get; set; }
    }

    public class OrderStoreDTO
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Location { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public int DeliveryTimeMinutes { get; set; }
        public int OrderCountPerStore { get; set; }
        public decimal AvgOrderValuePerStore { get; set; }
    }

    public class ProductSalesDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class StoreRankingUnoptimizedDTO
    {
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public int? OrderCount { get; set; }
        public decimal? TotalRevenue { get; set; }
        public int? AvgDeliveryTime { get; set; }
    }

    public class StoreRankingOptimizedDTO
    {
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int? AvgDeliveryTime { get; set; }
        public long? RevenueRank { get; set; }
    }
}