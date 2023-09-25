using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class TuitionFeesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public TuitionFeesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<TuitionFee>>> GetTuitionFees([FromQuery] SearchParams searchParams)
        {

            var tuitionFee = await _uow.TuitionFeeRepository.GetTuitionFees(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(tuitionFee.CurrentPage, tuitionFee.PageSize, tuitionFee.TotalCount, tuitionFee.TotalPages));

            return Ok(tuitionFee);
        }

        [HttpPost]
        [Authorize(Policy = "RequireTeacherRole")]
        public async Task<ActionResult> CreateTuitionFee(TuitionFee TuitionFee)
        {
            _uow.TuitionFeeRepository.AddTuitionFee(TuitionFee);

            if (await _uow.Complete()) return Ok("The tuition fee has been created successfully.");

            return BadRequest("Failed to create tuition fee");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateTuitionFee(TuitionFee TuitionFee, int id)
        {
            TuitionFee.Id = id;

            _uow.TuitionFeeRepository.UpdateTuitionFee(TuitionFee);

            if (await _uow.Complete()) return Ok("The tuition fee has been updated successfully.");

            return BadRequest("Unable to update the tuition fee. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteTuitionFee(int id)
        {
            if (await _uow.TuitionFeeRepository.GetTuitionFeeById(id) == null) return NotFound();

            _uow.TuitionFeeRepository.DeleteTuitionFee(id);

            if (await _uow.Complete()) return Ok("The tuition fee has been deleted successfully");

            return BadRequest("Unable to delete the tuition fee. Please try again later.");
        }
    }
}
