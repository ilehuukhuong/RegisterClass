using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GradeCategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public GradeCategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-coursegradecategories")]
        public async Task<ActionResult<PagedList<CourseGradeCategoryDto>>> GetCourseGradeCategories([FromQuery]SearchParams searchParams) 
        {
            var courseGradeCategories = await _uow.GradeCategoryRepository.GetCourseGradeCategories(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(courseGradeCategories.CurrentPage, courseGradeCategories.PageSize, courseGradeCategories.TotalCount, courseGradeCategories.TotalPages));

            return Ok(courseGradeCategories);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-coursegradecategory")]
        public async Task<ActionResult> CreateCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory)
        {
            var gradecategory = await _uow.GradeCategoryRepository.GetGradeCategoryById(createCourseGradeCategory.GradeCategoryId);
            if (gradecategory == null) return NotFound();
            var course = await _uow.CourseRepository.GetCourseById(createCourseGradeCategory.CourseId);
            if (course == null) return NotFound();
            
            _uow.GradeCategoryRepository.CreateCourseGradeCategory(createCourseGradeCategory);
            if(await _uow.Complete()) return Ok("Create successfully");
            return BadRequest("Problem create new");
        }

        [HttpPut("update-coursegradecategory")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory)
        {
            if (await _uow.GradeCategoryRepository.GetGradeCategoryById(createCourseGradeCategory.GradeCategoryId) == null) return NotFound();
            if (await _uow.CourseRepository.GetCourseById(createCourseGradeCategory.CourseId) == null) return NotFound();      

            await _uow.GradeCategoryRepository.UpdateCourseGradeCategory(createCourseGradeCategory);

            if (await _uow.Complete()) return Ok("The course grade category has been updated successfully.");

            return BadRequest("Unable to update the course grade category. Please check your input and try again.");
        }

        [HttpDelete("delete-coursegradecategory")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory)
        {
            if (await _uow.CourseRepository.GetCourseById(createCourseGradeCategory.CourseId) == null) return NotFound();
            if (await _uow.GradeCategoryRepository.GetGradeCategoryById(createCourseGradeCategory.GradeCategoryId) == null) return NotFound();

            _uow.GradeCategoryRepository.DeleteCourseGradeCategory(createCourseGradeCategory);

            if (await _uow.Complete()) return Ok("Delete successfully");

            return BadRequest("Unable to delete. Please try again later.");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeCategory>>> GetGradeCategories()
        {
            return Ok(await _uow.GradeCategoryRepository.GetGradeCategories());
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateGradeCategory(GradeCategory GradeCategory)
        {
            if (await _uow.GradeCategoryRepository.GetGradeCategoryByName(GradeCategory.Name) != null) return BadRequest("This name has taken");

            _uow.GradeCategoryRepository.AddGradeCategory(GradeCategory);

            if (await _uow.Complete()) return Ok("The grade category has been created successfully.");

            return BadRequest("Failed to create grade category");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateGradeCategory(GradeCategory GradeCategory, int id)
        {

            if (await _uow.GradeCategoryRepository.GetGradeCategoryByName(GradeCategory.Name) != null) return BadRequest("This name has taken");

            GradeCategory.Id = id;

            _uow.GradeCategoryRepository.UpdateGradeCategory(GradeCategory);

            if (await _uow.Complete()) return Ok("The grade category has been updated successfully.");

            return BadRequest("Unable to update the grade category. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteGradeCategory(int id)
        {
            if (_uow.GradeCategoryRepository.GetGradeCategoryById(id) == null) return NotFound();
            if (_uow.GradeCategoryRepository.DeleteGradeCategory(id) == false) return BadRequest("This grade category is being used");

            if (await _uow.Complete()) return Ok("The grade category has been deleted successfully");

            return BadRequest("Unable to delete the grade category. Please try again later.");
        }
    }
}
