using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlueStarMVC.Models;

namespace BlueStarMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChuyenbayController : ControllerBase
    {
        private readonly BluestarContext _dbContext;
        public ChuyenbayController(BluestarContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetChuyenbays")]
        public IActionResult GetChuyenbays() { 
            List<Chuyenbay> list = _dbContext.Chuyenbays.ToList();
            return StatusCode(StatusCodes.Status200OK, list);
        }
    }
}
