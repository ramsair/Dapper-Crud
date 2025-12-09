using Dapper_Crud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;

namespace Dapper_Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class StateController : ControllerBase
    {
        private readonly IStateRepo _stateRepo;

        public StateController(IDbConnection dbConnection)
        {
            _stateRepo = new StateRepository(dbConnection);//countryRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll() => Ok(await _stateRepo.GetAllAsync());

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetById(int id)
        {
            var state = await _stateRepo.GetByIdAsync(id);
            if (state == null) return NotFound();
            return Ok(state);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody] State state)
        {
            var id = await _stateRepo.CreateAsync(state);
            return Ok(new { StateId = id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update([FromBody] State state)
        {
            var success = await _stateRepo.UpdateAsync(state);
            return success ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _stateRepo.DeleteAsync(id);
            return success ? Ok("Deleted successfully") : NotFound();
        }
    }
}
