using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SalariesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public SalariesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateSalary(Salary salary)
        {
            _uow.SalaryRepository.AddSalary(salary);

            if (await _uow.Complete()) return Ok("The salary has been created successfully.");

            return BadRequest("Failed to create salary");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateSalary(Salary salary, int id)
        {
            salary.Id = id;

            _uow.SalaryRepository.UpdateSalary(salary);

            if (await _uow.Complete()) return Ok("The salary has been updated successfully.");

            return BadRequest("Unable to update the salary. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteSalary(int id)
        {
            if (await _uow.SalaryRepository.GetSalaryById(id) == null) return NotFound();

            _uow.SalaryRepository.DeleteSalary(id);

            if (await _uow.Complete()) return Ok("The salary has been deleted successfully");

            return BadRequest("Unable to delete the salary. Please try again later.");
        }
    }
}
