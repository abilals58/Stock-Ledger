using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/users")]
    [ApiController]
    public class UsersController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok( await _userService.GetAllUsersAsync());
        }

        // GET: api/users/id
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User with given id does not exist in the database!");
            }
            return Ok(user);
            
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var postedUser = await _userService.AddUserAsync(user);
            return StatusCode(201, new {Message ="New record added successfuly!", User = postedUser});
        }

        // PUT: api/users/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User newUser)
        {
            var user = await _userService.UpdateUserAsync(id, newUser);
            if (user == null)
            {
                return NotFound("User with given id does not exist in the database!");
            }
            
            return Ok(new {Message = "Record updated successfuly!", User = user});
        }

        // DELETE: api/users/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.DeleteUserAsync(id);
            if (user == null)
            {
                return NotFound("User with given id does not exist in the database!");
            }
            return Ok(new {Message = "Record deleted successfuly!", User = user});
        }
    }
}
