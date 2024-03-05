using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/stocksofusers")]
    [ApiController]
    public class StocksOfUsersController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly IStocksOfUserService _stocksOfUserService;

        public StocksOfUsersController(IStocksOfUserService stocksOfUserService)
        {
            _stocksOfUserService = stocksOfUserService;
        }
        
        // GET: api/stocksofusers
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _stocksOfUserService.GetAllStocksOfUserAsync());
        }

        // GET: api/stocksofusers/id
        [HttpGet("{userId}/{stockId}", Name = "GetStocksOfUser")]
        public async Task<IActionResult> Get(int userId, int stockId)
        {
            var stocksOfUser = await _stocksOfUserService.GetAStocksOfUserAsync(userId, stockId);
            if (stocksOfUser == null)
            {
                return NotFound("StocksOfUsers with given id does not exist in the database! ");
            }

            return Ok(stocksOfUser);
        }

        // POST: api/stocksofusers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StocksOfUser stocksOfUser)
        {
            await _stocksOfUserService.AddStocksOfUserAsync(stocksOfUser);
            return StatusCode(201,new {Message = "New StocksOfUser record added to the database!", StocksOfUser = stocksOfUser});
        }

        // PUT: api/stocksofusers/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StocksOfUser newStocksOfUser)
        {
            var stocksOfUser = await _stocksOfUserService.UpdateStocksOfUserAsync(id, newStocksOfUser);
            if (stocksOfUser == null)
            {
                return NotFound("StocksOfUsers with given id does not exist in the database! ");
            }
            return Ok(new {Message = "Record with given id updated successfuly!", StocksOfUser = stocksOfUser});

        }

        // DELETE: api/stocksofusers/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stocksOfUser = await _stocksOfUserService.DeleteStocksOfUserAsync(id);
            if (stocksOfUser == null)
            {
                return NotFound("StocksOfUsers with given id does not exist in the database! ");
            }
            
            return Ok(new {Message = "Record with given id deleted successfuly!", StocksOfUser = stocksOfUser});
        }
    }
}
