using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/buyorders")]
    [ApiController]
    public class BuyOrdersController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly IBuyOrderService _buyOrderService;

        public BuyOrdersController(IBuyOrderService buyOrderService)
        {
            _buyOrderService = buyOrderService;
        }
        
        // GET: api/buyorders
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _buyOrderService.GetAllBuyOrdersAsync());
        }

        // GET: api/buyorders/id
        [HttpGet("{id}", Name = "GetBuyOrder")]
        public async Task<IActionResult> Get(int id)
        {
            var buyOrder = await _buyOrderService.GetBuyOrderByIdAsync(id);
            if (buyOrder == null)
            {
                return NotFound("BuyOrder with given id does not exist in the database!");
            }

            return Ok(buyOrder);
        }

        // POST: api/buyorders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BuyOrder buyOrder)
        {
            await _buyOrderService.AddBuyOrderAsync(buyOrder);
            return StatusCode(201, new {Message = "New BuyOrder added to the database succesfuly!", BuyOrder = buyOrder});
        }

        // PUT: api/buyorders/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BuyOrder newbuyOrder)
        {
            var buyOrder = await _buyOrderService.UpdateByOrderAsync(id, newbuyOrder);
            if (buyOrder == null) 
            {
                return NotFound("BuyOrder with given id does not exist in the database!");
            }
            return Ok(new {Message = "BuyOrder with given id updated successfuly!", BuyOrder = buyOrder});
        }

        // DELETE: api/buyorders/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var buyOrder = await _buyOrderService.DeleteBuyOrderAsync(id);
            if (buyOrder == null)
            {
                return NotFound("BuyOrder with given id does not exist in the database!");
            }
            return Ok(new {Message = "BuyOrder with given id deleted successfuly!", BuyOrder = buyOrder});

        }
    }
}
