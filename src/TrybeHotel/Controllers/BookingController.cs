using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                var userByEmail = User.FindFirstValue(ClaimTypes.Email);
                var response = _repository.Add(bookingInsert, userByEmail);
                return Created("", response);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var userByEmail = token?.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Email)?.Value;
            BookingResponse? bookingResponse = _repository.GetBooking(Bookingid, userByEmail!);

            if (bookingResponse == null)
            {
                return Unauthorized();
            }
            return Ok(bookingResponse);
        }
    }
}