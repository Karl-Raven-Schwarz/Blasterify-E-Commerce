using Blasterify.Models.Requests;
using Blasterify.Models.Responses;
using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly DataContext _context;

        public CountryController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(CountryRequest countryRequest)
        {
            try
            {
                // Validate request, check if name or code are empty
                if (string.IsNullOrEmpty(countryRequest.Name))
                {
                    return BadRequest(new { message = "Name is required" });
                }
                else if (string.IsNullOrEmpty(countryRequest.Code))
                {
                    return BadRequest(new { message = "Code is required" });
                }

                var newCountry = await _context.Countries!.AddAsync(new Country
                {
                    Name = countryRequest.Name,
                    Code = countryRequest.Code,
                });

                await _context.SaveChangesAsync();

                return Ok(new { data = newCountry.Entity });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var country = await _context.Countries!.FindAsync(id);

                if (country == null)
                {
                    return NotFound(new { message = "Country not found" });
                }

                return Ok(new { data = country });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllForClientUser")]
        public async Task<ActionResult> GetAllForClientUser()
        {
            try
            {
                var countries = await _context.Countries!.Select(c => new CountryResponse 
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                }).ToListAsync();

                if (countries.Count == 0)
                {
                    return NotFound(new { message = "No countries found" });
                }

                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Country country)
        {
            var getCountry = await _context.Countries!.FindAsync(id);
            getCountry!.Name = country.Name;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _context.Countries!.FindAsync(id);
            _context.Countries.Remove(country!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}