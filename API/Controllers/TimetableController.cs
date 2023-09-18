using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TimetableController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public TimetableController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<TimetableDto>>> GetTimetables([FromQuery] SearchParams searchParams)
        {
            var Timetables = await _uow.TimetableRepository.GetTimetables(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(Timetables.CurrentPage, Timetables.PageSize, Timetables.TotalCount, Timetables.TotalPages));

            return Ok(Timetables);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateTimetable(Timetable timetable)
        {
            _uow.TimetableRepository.AddTimetable(timetable);

            if (await _uow.Complete()) return Ok("The timetable has been created successfully.");

            return BadRequest("Failed to create timetable");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateTimetable(Timetable timetable, int id)
        {
            if (await _uow.TimetableRepository.GetTimetableById(id) == null) return NotFound();

            timetable.Id = id;

            _uow.TimetableRepository.UpdateTimetable(timetable);

            if (await _uow.Complete()) return Ok("The timetable has been updated successfully.");

            return BadRequest("Unable to update the timetable. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteTimetable(int id)
        {
            var timetable = await _uow.TimetableRepository.GetTimetableById(id);

            if (timetable == null) return NotFound();

            if (_uow.TimetableRepository.DeleteTimetable(timetable) == false) return BadRequest("This timetable is being used");

            if (await _uow.Complete()) return Ok("The timetable has been deleted successfully");

            return BadRequest("Unable to delete the timetable. Please try again later.");
        }
    }
}
