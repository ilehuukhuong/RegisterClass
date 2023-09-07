using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CoursesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CoursesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<CourseDto>>> GetCourses([FromQuery] SearchParams searchParams)
        {
            var courses = await _uow.CourseRepository.GetCourses(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(courses.CurrentPage, courses.PageSize, courses.TotalCount, courses.TotalPages));

            return Ok(courses);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateCourse(Course course)
        {
            if (await _uow.CourseRepository.GetCourseByCode(course.Code) != null) return BadRequest("This code has taken");

            _uow.CourseRepository.AddCourse(course);

            if (await _uow.Complete()) return Ok("The course has been created successfully.");

            return BadRequest("Failed to create course");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateCourse(Course course, int id)
        {
            if (await _uow.CourseRepository.GetCourseById(id) == null) return NotFound();

            course.Id = id;

            var checkCourse = await _uow.CourseRepository.GetCourseByCode(course.Code);
            if (checkCourse != null)
                if (checkCourse.Id != course.Id && checkCourse.Code == course.Code) return BadRequest("This code has taken");

            _uow.CourseRepository.UpdateCourse(course);

            if (await _uow.Complete()) return Ok("The course has been updated successfully.");

            return BadRequest("Unable to update the course. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var course = await _uow.CourseRepository.GetCourseById(id);

            if (course == null) return NotFound();

            if (_uow.CourseRepository.DeleteCourse(course) == false) return BadRequest("This course is being used");

            if (await _uow.Complete()) return Ok("The course has been deleted successfully");

            return BadRequest("Unable to delete the course. Please try again later.");
        }
    }
}
