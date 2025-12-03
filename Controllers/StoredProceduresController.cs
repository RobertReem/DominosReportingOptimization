using Microsoft.AspNetCore.Mvc;
using DominosReportingOptimization.Services;

namespace DominosReportingOptimization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoredProceduresController : ControllerBase
    {
        private readonly StoredProcedureService _storedProcedureService;

        public StoredProceduresController(StoredProcedureService storedProcedureService)
        {
            _storedProcedureService = storedProcedureService;
        }

        //Simple SELECT without join
        [HttpGet("orders-unoptimized")]
        public async Task<IActionResult> GetOrdersUnoptimized()
        {
            try
            {
                var result = await _storedProcedureService.GetOrdersWithStoresUnoptimized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Single query with JOIN and window functions
        [HttpGet("orders-optimized")]
        public async Task<IActionResult> GetOrdersOptimized()
        {
            try
            {
                var result = await _storedProcedureService.GetOrdersWithStoresOptimized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Missing index on FK
        [HttpGet("products-unoptimized")]
        public async Task<IActionResult> GetProductsUnoptimized()
        {
            try
            {
                var result = await _storedProcedureService.GetProductSalesUnoptimized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // With index on ProductId
        [HttpGet("products-optimized")]
        public async Task<IActionResult> GetProductsOptimized()
        {
            try
            {
                var result = await _storedProcedureService.GetProductSalesOptimized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Correlated subqueries (N+1 problem)
        [HttpGet("stores-unoptimized")]
        public async Task<IActionResult> GetStoresUnoptimized()
        {
            try
            {
                var result = await _storedProcedureService.GetStoreRankingsUnoptimized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Window functions and CTE
        [HttpGet("stores-optimized")]
        public async Task<IActionResult> GetStoresOptimized()
        {
            try
            {
                var result = await _storedProcedureService.GetStoreRankingsOptimized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}