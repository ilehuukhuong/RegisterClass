using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ClassesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public ClassesController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<ClassDto>>> GetClasss([FromQuery] SearchParams searchParams)
        {
            var Classs = await _uow.ClassRepository.GetClasses(searchParams);

            Response.AddPaginationHeader(new PaginationHeader(Classs.CurrentPage, Classs.PageSize, Classs.TotalCount, Classs.TotalPages));

            return Ok(Classs);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateClass([FromForm]CreateClassDto createDto)
        {
            if (await _uow.ClassRepository.GetClassByCode(createDto.Code) != null) return BadRequest("This code has taken");
            var Class = _mapper.Map<Class>(createDto);

            _uow.ClassRepository.AddClass(Class);

            if (createDto.File != null)
            {
                var resultPhoto = await _photoService.AddPhotoAsync(createDto.File);

                if (resultPhoto.Error != null) return BadRequest(resultPhoto.Error.Message);

                Class.PhotoUrl = resultPhoto.SecureUrl.AbsoluteUri;
                Class.PhotoId = resultPhoto.PublicId;
            }

            if (await _uow.Complete()) return Ok("The class has been created successfully.");

            return BadRequest("Failed to create class");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateClass([FromForm]UpdateClassDto updateClassDto, int id)
        {
            if (await _uow.ClassRepository.GetClassById(id) == null) return NotFound();

            updateClassDto.Id = id;

            var checkClass = await _uow.ClassRepository.GetClassByCode(updateClassDto.Code);
            if (checkClass != null)
                if (checkClass.Id != updateClassDto.Id && checkClass.Code == updateClassDto.Code) return BadRequest("This code has taken");

            var updateClass = await _uow.ClassRepository.GetClassById(id);
            _mapper.Map(updateClassDto, updateClass);

            if (updateClassDto.File != null)
            {
                var resultPhoto = await _photoService.AddPhotoAsync(updateClassDto.File);
                if (resultPhoto.Error != null) return BadRequest(resultPhoto.Error.Message);
                if (updateClass.PhotoId != null)
                {
                    var resultDeletePhoto = await _photoService.DeletePhotoAsync(updateClass.PhotoId);
                    if (resultDeletePhoto.Error != null) return BadRequest(resultDeletePhoto.Error.Message);
                }
                updateClass.PhotoUrl = resultPhoto.SecureUrl.AbsoluteUri;
                updateClass.PhotoId = resultPhoto.PublicId;
            }

            if (await _uow.Complete()) return Ok("The class has been updated successfully.");

            return BadRequest("Unable to update the class. Please check your input and try again.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteClass(int id)
        {
            var Class = await _uow.ClassRepository.GetClassById(id);

            if (Class == null) return NotFound();

            if (_uow.ClassRepository.DeleteClass(Class) == false) return BadRequest("This class is being used");

            if (await _uow.Complete()) return Ok("The class has been deleted successfully");

            return BadRequest("Unable to delete the class. Please try again later.");
        }
    }
}
