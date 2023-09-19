using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HolidaySchedulesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public HolidaySchedulesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<HolidaySchedule>>> GetHolidaySchedules([FromQuery] SearchParams searchParams)
        {
            var holidaySchedule = await _uow.HolidayScheduleRepository.GetHolidaySchedules(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(holidaySchedule.CurrentPage, holidaySchedule.PageSize, holidaySchedule.TotalCount, holidaySchedule.TotalPages));

            return Ok(holidaySchedule);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateHolidaySchedule(HolidaySchedule holidaySchedule)
        {
            _uow.HolidayScheduleRepository.AddHolidaySchedule(holidaySchedule);

            if (await _uow.Complete()) return Ok("The holiday schedule has been created successfully.");

            return BadRequest("Failed to create holiday schedule");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateHolidaySchedule(HolidaySchedule holidaySchedule, int id)
        {
            holidaySchedule.Id = id;

            _uow.HolidayScheduleRepository.UpdateHolidaySchedule(holidaySchedule);

            if (await _uow.Complete()) return Ok("The holiday schedule has been updated successfully.");

            return BadRequest("Unable to update the holiday schedule. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteHolidaySchedule(int id)
        {
            if (await _uow.HolidayScheduleRepository.GetHolidayScheduleById(id) == null) return NotFound();

            _uow.HolidayScheduleRepository.DeleteHolidaySchedule(id);

            if (await _uow.Complete()) return Ok("The holiday schedule has been deleted successfully");

            return BadRequest("Unable to delete the holiday schedule. Please try again later.");
        }
    }
}
