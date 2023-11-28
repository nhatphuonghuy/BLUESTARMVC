using BlueStarMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueStarMVC.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly BluestarContext _dbContext;
        public TicketController(BluestarContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetTickets")]
        public IActionResult GetTickets()
        {
            List<Ticket> list = _dbContext.Tickets.ToList();
            return StatusCode(StatusCodes.Status200OK, list);
        }
        [HttpPost]
        [Route("AddTicket")]
        public IActionResult AddTicket([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Invalid Ticket data");
            }

            try
            {
                _dbContext.Tickets.Add(ticket);
                _dbContext.SaveChanges();
                return Ok("Ticket added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("GetTicketDetails")]
        public IActionResult GetTicketDetails([FromQuery] string tIds)
        {
            try
            {
                if (string.IsNullOrEmpty(tIds))
                {
                    return BadRequest("Invalid Ticket IDs");
                }

                var ticketIds = tIds.Split(',');

                var ticketDetails = _dbContext.Tickets.Where(c => ticketIds.Contains(c.TId)).ToList();

                return Ok(ticketDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("UpdateTicket")]
        public async Task<IActionResult> UpdateTicket(Ticket objTicket)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }
                // Tìm kiếm khách hàng dựa trên id (hoặc mã khách hàng, tùy thuộc vào cách bạn xác định)
                var existingTicket = await _dbContext.Tickets.FindAsync(objTicket.TId);

                if (existingTicket == null)
                {
                    return NotFound("Ticket not found");
                }

                // Cập nhật thông tin của khách hàng từ dữ liệu mới
                existingTicket.TId = objTicket.TId;
                existingTicket.Cccd = objTicket.Cccd;
                existingTicket.Name = objTicket.Name;
                existingTicket.FlyId = objTicket.FlyId;
                existingTicket.KgId = objTicket.KgId;
                existingTicket.SeatId = objTicket.SeatId;
                existingTicket.FoodId = objTicket.FoodId;
                existingTicket.TicketPrice = objTicket.TicketPrice;
                existingTicket.Mail = objTicket.Mail;
                existingTicket.DisId = objTicket.DisId;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();

                // Trả về thông tin khách hàng đã được cập nhật
                return Ok(existingTicket);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về lỗi 500 nếu có lỗi xảy ra
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTickets([FromBody] List<string> ticketIds)
        {
            if (ticketIds == null || !ticketIds.Any())
            {
                return BadRequest("No Ticket IDs provided");
            }

            var Tickets = await _dbContext.Tickets.Where(c => ticketIds.Contains(c.TId)).ToListAsync();
            if (!Tickets.Any())
            {
                return NotFound("No matching Tickets found");
            }

            _dbContext.Tickets.RemoveRange(Tickets);
            await _dbContext.SaveChangesAsync();
            return Ok("Tickets deleted successfully");
        }
    }
}
