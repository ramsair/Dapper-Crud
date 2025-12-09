using Dapper_Crud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Dapper_Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictRepo _repo;

        public DistrictController(IDbConnection dbConnection)
        {
            _repo = new DistrictRepository(dbConnection);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("state/{stateId}")]
        public async Task<IActionResult> GetByStateId(int stateId) =>
            Ok(await _repo.GetByStateIdAsync(stateId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var district = await _repo.GetByIdAsync(id);
            if (district == null) return NotFound();
            return Ok(district);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] District district)
        {
            var id = await _repo.CreateAsync(district);
            return Ok(new { DistrictId = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] District district)
        {
            var success = await _repo.UpdateAsync(district);
            return success ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.DeleteAsync(id);
            return success ? Ok("Deleted successfully") : NotFound();
        }
    }
}
