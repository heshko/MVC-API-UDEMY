using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HeshkoSwaggerParks")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;

        }


        /// <summary>
        /// Get All National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<NationalParkDto>))]
       
        public ActionResult GetAllNationalParks()
        {
            var objList = _npRepo.GetNationalParks();

            var objTdoList = new List<NationalParkDto>();

            foreach (var nationalPark in objList)
            {
                objTdoList.Add(_mapper.Map<NationalParkDto>(nationalPark));
            }
            return Ok(objTdoList);
        }

        /// <summary>
        /// Get one National Park by id
        /// </summary>
        /// ///<param name="id">National park Id</param>
        /// <returns></returns>

        [HttpGet("{id:int}",Name = "GetNationalParkById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize]
        public ActionResult GetNationalParkById(int id)
        {
            if (_npRepo.NationalParkExists(id))
            {
                var park = _npRepo.GetNationalPark(id);
                var parkDto = _mapper.Map<NationalParkDto>(park);
                return Ok(parkDto);
            }
            else
            {
                return NotFound();
            }
           
          
        }
        /// <summary>
        /// Create   National Park
        /// without id 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateNationalPark([FromBody] NationalParkDto parkDto)
        {
        
            if (parkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(parkDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Park Exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var park = _mapper.Map<NationalPark>(parkDto);
   
            if (_npRepo.CreateNationalParkExists(park))
            {
                return CreatedAtAction("GetNationalParkById", new { id = park.Id }, park);
            }
           
                ModelState.AddModelError(string.Empty, $"Something went wrong when saving the record {park.Name}");
                return StatusCode(500,ModelState);
            
           
            
        }

        /// <summary>
        /// Update  National Park by id
        /// 
        /// </summary>
        ///<param name="id">National park Id</param>
        /// <returns></returns>

        [HttpPut("{id:int}",Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(int id,[FromBody] NationalParkDto parkDto)
        {
            if (parkDto == null || parkDto.Id != id)
            {
                return BadRequest(ModelState);
            }

            var park = _mapper.Map<NationalPark>(parkDto);

            if (!_npRepo.NationalParkExists(park.Id))
            {
                return NotFound(ModelState);
            }
            if (!_npRepo.UpdateNationalParkExists(park))
            {

                ModelState.AddModelError(string.Empty, $"Somthing went wrong when update this park {park.Name}");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// delete  National Park by id
        /// 
        /// </summary>
        ///<param name="id">National park Id</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult DeleteNationalPark(int id)
        {
            
            if (!_npRepo.NationalParkExists(id))
            {
                ModelState.AddModelError(string.Empty, "This Nationaal is not found");
                return NotFound(ModelState);
            }

            var park = _npRepo.GetNationalPark(id);

           
            if (!_npRepo.DeleteNationalParkExists(park))
            {

                ModelState.AddModelError(string.Empty, $"Somthing went wrong when Deleting this park {park.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}