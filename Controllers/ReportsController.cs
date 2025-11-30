using Microsoft.AspNetCore.Mvc;
using DominosReportingOptimization.Services;

namespace DominosReportingOptimization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales-unoptimized")]

        public async Task<IActionResult> GetSalesReportUnoptimized(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _reportService.GetSalesReportUnoptimized(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sales-optimized")]
        public async Task<IActionResult> GetSalesReportOptimized( [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _reportService.GetSalesReportOptimized(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int topCount = 10)
        {
            try
            {
                var result = await _reportService.GetTopProductsBySales(topCount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetStorePerformance()
        {
            try
            {
                var result = await _reportService.GetStorePerformance();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}