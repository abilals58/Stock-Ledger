using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/stocks")]
    [ApiController]
    public class StocksController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }
        
        // GET: api/stocks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _stockService.GetAllStocksAsync());
        }

        // GET: api/stocks/id
        [HttpGet("{id}", Name = "GetStockById")]
        public async Task<IActionResult> Get(int id)
        {
            var stock = await _stockService.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound("Stock with given id does not exist in the database!");
            }

            return Ok(stock);
        }
        // POST: api/stocks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Stock stock)
        {
            await _stockService.AddStockAsync(stock);
            return StatusCode(201, new {Message = "New record added to the database successfuly!", Stock = stock});
        }

        // PUT: api/stocks/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Stock newStock)
        {
            var stock = await _stockService.UpdateStockAsync(id, newStock);
            if (stock == null)
            {
                return NotFound("Stock with given id does not exist in the database!");
            }
            return Ok(new {Message = "Stock with given id updated successfuly!", Stock = stock});
        }

        // DELETE: api/stocks/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stock = await _stockService.DeleteStockAsync(id);
            if (stock == null)
            {
                return NotFound("Stock with given id does not exist in the database!");
            }
            return Ok(new {Message = "Stock with given id deleted successfuly!", Stock = stock});
        }
        
    }
}
