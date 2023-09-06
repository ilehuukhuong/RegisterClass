using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SemestersController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public SemestersController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<SemesterDto>>> GetSemesters([FromQuery] SearchParams searchParams)
        {
            var semesters = await _uow.SemesterRepository.GetSemesters(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(semesters.CurrentPage, semesters.PageSize, semesters.TotalCount, semesters.TotalPages));

            return Ok(semesters);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateSemester(SemesterDto semesterDto)
        {
            if (await _uow.SemesterRepository.GetSemesterByCode(semesterDto.Code) != null) return BadRequest("This code has taken");

            _uow.SemesterRepository.AddSemester(semesterDto);

            if (await _uow.Complete()) return Ok("The semester has been created successfully.");

            return BadRequest("Failed to create semester");
        }

        [HttpPost("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CopySemester(int id)
        {
            var semester = await _uow.SemesterRepository.GetSemesterById(id);

            if (semester == null) return NotFound();

            _uow.SemesterRepository.AddSemester(_uow.SemesterRepository.CopySemester(semester));

            if (await _uow.Complete()) return Ok("The semester has been copy successfully.");

            return BadRequest("Failed to copy semester");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateSemester(SemesterDto semesterDto, int id)
        {
            semesterDto.Id = id;

            var checkSemester = await _uow.SemesterRepository.GetSemesterByCode(semesterDto.Code);

            if (checkSemester.Id != semesterDto.Id && checkSemester.Code == semesterDto.Code) return BadRequest("This code has taken");

            _uow.SemesterRepository.UpdateSemester(semesterDto);

            if (await _uow.Complete()) return Ok("The semester has been updated successfully.");

            return BadRequest("Unable to update the semester. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteSemester(int id)
        {
            var semester = await _uow.SemesterRepository.GetSemesterById(id);

            if (semester == null) return NotFound();

            if (_uow.SemesterRepository.DeleteSemester(semester) == false) return BadRequest("This semester is being used");

            if (await _uow.Complete()) return Ok("The semester has been deleted successfully");

            return BadRequest("Unable to delete the semester. Please try again later.");
        }
    }
}
