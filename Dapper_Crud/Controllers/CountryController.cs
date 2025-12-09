using Dapper_Crud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepo _countryRepo;

        public CountryController(ICountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
           ///_countryRepo = new CountryRepository(dbConnection, 1, "");  , IDbConnection dbConnection

        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll() => Ok(await _countryRepo.GetAllAsync());

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _countryRepo.GetByIdAsync(id);
            if (country == null) return NotFound();
            return Ok(country);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody] Country country)
        {
            var id = await _countryRepo.CreateAsync(country);
            return Ok(new { CountryId = id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update([FromBody] Country country)
        {
            var success = await _countryRepo.UpdateAsync(country);
            return success ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _countryRepo.DeleteAsync(id);
            return success ? Ok("Deleted successfully") : NotFound();
        }
    }
}
