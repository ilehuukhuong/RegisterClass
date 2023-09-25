using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

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
        [Route("income")]
        public async Task<IActionResult> DownloadExcel([FromQuery] SearchParams searchParams)
        {
            var tuitionFees = await _uow.TuitionFeeRepository.GetTuitionFeesByDay(searchParams);
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells["A1"].Value = "STT";
                workSheet.Cells["B1"].Value = "Mã Học Viên";
                workSheet.Cells["C1"].Value = "Họ Tên";
                workSheet.Cells["D1"].Value = "Lớp Học";
                workSheet.Cells["E1"].Value = "Học Phí";
                workSheet.Cells["F1"].Value = "Giáo Viên";

                int row = 2;
                int i = 1;
                foreach (var tuitionFee in tuitionFees)
                {
                    workSheet.Cells["A" + row].Value = i;
                    workSheet.Cells["B" + row].Value = tuitionFee.StudentCode;
                    workSheet.Cells["C" + row].Value = tuitionFee.StudentName;
                    workSheet.Cells["D" + row].Value = tuitionFee.Class;
                    workSheet.Cells["E" + row].Value = tuitionFee.TuitionFeeAmount;
                    workSheet.Cells["F" + row].Value = tuitionFee.TeacherName;
                    row++;
                    i++;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "income.xlsx");
            }
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
