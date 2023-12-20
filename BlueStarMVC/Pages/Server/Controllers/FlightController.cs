using BlueStarMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueStarMVC.Pages.Server.DTOs;
using System.Globalization;

namespace BlueStarMVC.Pages.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly BluestarContext BlueContext;
        public FlightController(BluestarContext blueContext)
        {
            BlueContext = blueContext;
        }
        [HttpGet]
        public async Task<IActionResult> SearchFlight([FromQuery] QueryFlightDTOs query)
        {

            string departureDay = null;

            // Extract the day value and convert it to a string
            string day = query.DepatureDay.Substring(8, 2);
                string month = query.DepatureDay.Substring(5, 2);
            string year = query.DepatureDay.Substring(0, 4);

            departureDay = $"{day}-{month}-{year}";
               
            Console.WriteLine(departureDay);

            var flightsQuery = BlueContext.Chuyenbays
             .Where(c =>
                 c.FromLocation == query.FromLocation &&
                 c.ToLocation == query.ToLocation &&
                 c.DepartureDay == departureDay
             );

            if (query.DepartureTime != null && query.ArrivalTime != null)
            {
                TimeSpan departureTimeSpan = TimeSpan.Parse(query.DepartureTime);
                TimeSpan arrivalTimeSpan = TimeSpan.Parse(query.ArrivalTime);

                flightsQuery = flightsQuery
                    .Where(f => TimeSpan.Parse(f.DepartureTime) >= departureTimeSpan && TimeSpan.Parse(f.DepartureTime) <= arrivalTimeSpan);
            }


            var flights = await flightsQuery.ToListAsync();
            int totalFlights = flights.Count;


            var result = new
            {
                total_flight = totalFlights,
                flight = flights
            };

            return Ok(result);
        }
    }
}