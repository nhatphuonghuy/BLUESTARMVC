using BlueStarMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueStarMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BluestarContext _dbContext;
        public AccountController(BluestarContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetAccounts")]
        public IActionResult GetAccounts()
        {
            List<Account> list = _dbContext.Accounts.ToList();
            return StatusCode(StatusCodes.Status200OK, list);
        }

        [HttpPost]
        [Route("AddAccount")]
        public IActionResult AddAccount([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest("Invalid customer data");
            }

            try
            {
                _dbContext.Accounts.Add(account);
                _dbContext.SaveChanges();
                return Ok("Account added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
