using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GradesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public GradesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<Grade>>> GetGrades([FromQuery] SearchParams searchParams)
        {
            if (searchParams.ClassId == null || searchParams.CourseId == null) return BadRequest("Please input class and course");
            if (await _uow.ClassRepository.GetClassById(searchParams.ClassId.Value) == null || await _uow.CourseRepository.GetCourseById(searchParams.CourseId.Value) == null) return NotFound();
            
            var grade = await _uow.GradeRepository.GetGrades(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(grade.CurrentPage, grade.PageSize, grade.TotalCount, grade.TotalPages));

            return Ok(grade);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateGrade(Grade grade)
        {
            _uow.GradeRepository.AddGrade(grade);

            if (await _uow.Complete()) return Ok("The grade has been created successfully.");

            return BadRequest("Failed to create grade");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateGrade(Grade grade, int id)
        {
            grade.Id = id;

            _uow.GradeRepository.UpdateGrade(grade);

            if (await _uow.Complete()) return Ok("The grade has been updated successfully.");

            return BadRequest("Unable to update the grade. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteGrade(int id)
        {
            if (await _uow.GradeRepository.GetGradeById(id) == null) return NotFound();

            _uow.GradeRepository.DeleteGrade(id);

            if (await _uow.Complete()) return Ok("The grade has been deleted successfully");

            return BadRequest("Unable to delete the grade. Please try again later.");
        }
    }
}
