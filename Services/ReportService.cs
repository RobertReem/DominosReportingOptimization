using Microsoft.EntityFrameworkCore;
using DominosReportingOptimization.Data;
using DominosReportingOptimization.Models;
using System.Diagnostics;

namespace DominosReportingOptimization.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<dynamic> GetSalesReportUnoptimized(DateTime startDate, DateTime endDate)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .ToListAsync();

                if (!orders.Any())
                {
                    return new { TotalOrders = 0, TotalRevenue = 0m, AverageOrderValue = 0m, StoreCount = 0, AverageDeliveryTime = 0d };
                }

                var storeIds = orders.Select(o => o.StoreId).Distinct().ToList();
                var stores = await _context.Stores
                    .Where(s => storeIds.Contains(s.StoreId))
                    .ToListAsync();

                stopwatch.Stop();
                TimeSpan totalElapsed = stopwatch.Elapsed;
                Console.WriteLine($"Total time took: {totalElapsed.TotalMilliseconds} ms");

                var result = new
                {
                    TotalOrders = orders.Count,
                    TotalRevenue = orders.Sum(o => o.OrderTotal),
                    AverageOrderValue = orders.Average(o => o.OrderTotal),
                    StoreCount = stores.Count,
                    AverageDeliveryTime = orders.Average(o => o.DeliveryTimeMinutes)
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetSalesReportUnoptimized: {ex.Message}", ex);
            }
        }

        public async Task<dynamic> GetSalesReportOptimized(DateTime startDate, DateTime endDate)
        {

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Join(
                    _context.Stores,
                    order => order.StoreId,
                    store => store.StoreId,
                    (order, store) => new { order, store }
                )
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(x => x.order.OrderTotal),
                    AverageOrderValue = g.Average(x => x.order.OrderTotal),
                    StoreCount = g.Select(x => x.store.StoreId).Distinct().Count(),
                    AverageDeliveryTime = g.Average(x => x.order.DeliveryTimeMinutes)
                })
                .FirstOrDefaultAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"Total time took: {totalElapsed.TotalMilliseconds} ms");

            if (result == null)
            {
                return new { TotalOrders = 0, TotalRevenue = 0m, AverageOrderValue = 0m, StoreCount = 0, AverageDeliveryTime = 0d };
            }

            return result;
        }

        public async Task<List<dynamic>> GetTopProductsBySales(int topCount = 10)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = await _context.OrderItems
                .Join(
                    _context.Products,
                    oi => oi.ProductId,
                    p => p.ProductId,
                    (oi, p) => new { oi, p }
                )
                .GroupBy(x => new { x.p.ProductId, x.p.ProductName })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantitySold = g.Sum(x => x.oi.Quantity),
                    TotalRevenue = g.Sum(x => x.oi.LineTotal),
                    AverageSalePrice = g.Average(x => x.oi.LineTotal)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(topCount)
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"Total time took: {totalElapsed.TotalMilliseconds} ms");

            if (result == null || !result.Any())
            {
                return new List<dynamic>();
            }

            
            return result.Cast<dynamic>().ToList();
        }

        public async Task<List<dynamic>> GetStorePerformance()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = await _context.Orders
                .Join(
                    _context.Stores,
                    order => order.StoreId,
                    store => store.StoreId,
                    (order, store) => new { order, store }
                )
                .GroupBy(x => new { x.store.StoreId, x.store.StoreName, x.store.Location })
                .Select(g => new
                {
                    StoreId = g.Key.StoreId,
                    StoreName = g.Key.StoreName,
                    Location = g.Key.Location,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(x => x.order.OrderTotal),
                    AverageOrderValue = g.Average(x => x.order.OrderTotal),
                    AverageDeliveryTime = g.Average(x => x.order.DeliveryTimeMinutes)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToListAsync();

            stopwatch.Stop();
            TimeSpan totalElapsed = stopwatch.Elapsed;
            Console.WriteLine($"Total time took: {totalElapsed.TotalMilliseconds} ms");

            if (result == null || !result.Any())
            {
                return new List<dynamic>();
            }
            
            return result.Cast<dynamic>().ToList();
        }
    }
}