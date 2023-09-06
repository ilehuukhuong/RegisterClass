using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DepartmentFacultiesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public DepartmentFacultiesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<DepartmentFacultyDto>>> GetDepartmentFaculties([FromQuery] SearchParams searchParams)
        {
            var departmentFaculties = await _uow.DepartmentFacultyRepository.GetDepartmentFaculties(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(departmentFaculties.CurrentPage, departmentFaculties.PageSize, departmentFaculties.TotalCount, departmentFaculties.TotalPages));

            return Ok(departmentFaculties);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateDepartmentFaculty(DepartmentFaculty departmentFaculty)
        {
            if (await _uow.DepartmentFacultyRepository.GetDepartmentFacultyByName(departmentFaculty.Name) != null) return BadRequest("This name has taken");

            _uow.DepartmentFacultyRepository.AddDepartmentFaculty(departmentFaculty);

            if (await _uow.Complete()) return Ok("The DepartmentFaculty has been created successfully.");

            return BadRequest("Failed to create DepartmentFaculty");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateDepartmentFaculty(DepartmentFaculty departmentFaculty, int id)
        {

            if (await _uow.DepartmentFacultyRepository.GetDepartmentFacultyByName(departmentFaculty.Name) != null) return BadRequest("This name has taken");

            departmentFaculty.Id = id;

            _uow.DepartmentFacultyRepository.UpdateDepartmentFaculty(departmentFaculty);

            if (await _uow.Complete()) return Ok("The department/faculty has been updated successfully.");

            return BadRequest("Unable to update the department/faculty. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteDepartmentFaculty(int id)
        {
            if (_uow.DepartmentFacultyRepository.DeleteDepartmentFaculty(id) == false) return BadRequest("This department/faculty is being used");

            if (await _uow.Complete()) return Ok("The department/faculty has been deleted successfully");

            return BadRequest("Unable to delete the department/faculty. Please try again later.");
        }
    }
}
