using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HeshkoSwaggerTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailsRepository _tRepo;
        private readonly INationalParkRepository _nPRepo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailsRepository tRepo, IMapper mapper, INationalParkRepository nPRepo)
        {
            _tRepo = tRepo;
            _mapper = mapper;
            _nPRepo = nPRepo;

        }


        /// <summary>
        /// Get AllTrails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<TrailDto>))]
       
        public ActionResult GetAllTrails()
        {
            var objList = _tRepo.GetTrails();

            var objTdoList = new List<TrailDto>();

            foreach (var trail in objList)
            {
                objTdoList.Add(_mapper.Map<TrailDto>(trail));
            }
            return Ok(objTdoList);
        }

        /// <summary>
        /// Get oneTrail by id
        /// </summary>
        /// ///<param name="id">Trail Id</param>
        /// <returns></returns>

        [HttpGet("{id:int}",Name = "GetTrailsById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public ActionResult GetTrailsById(int id)
        {
            if (_tRepo.TrailsExists(id))
            {
                var trail = _tRepo.GetTrailById(id);
                var trailDto = _mapper.Map<TrailDto>(trail);
                return Ok(trailDto);
            }
            else
            {
                return NotFound();
            }
           
          
        }

        /// <summary>
        /// Get oneTrail by National id
        /// </summary>
        /// ///<param name="id">National  Id</param>
        /// <returns></returns>

        [HttpGet("[action]/{id:int}", Name = "GetTrailsByNationalParkID")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public ActionResult GetTrailsByNationalParkID(int id)
        {
            if (!_nPRepo.NationalParkExists(id))
            {
                ModelState.AddModelError(string.Empty, $"I cant Found this National Park with this id {id}");
                return NotFound(ModelState);
            }
            if (_tRepo.GetTrailsInNationalPark(id).Trails.Count() == 0)
            {
                ModelState.AddModelError(string.Empty, $" this National Park with this id {id} have not any Trails");
                return Ok(ModelState);
                
            }

            var nationalPark = _tRepo.GetTrailsInNationalPark(id);
            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            var trails = nationalPark.Trails;
            var trailsDto = new List<TrailDto>();

            foreach (var trail in trails)
            {
                trailsDto.Add(_mapper.Map<TrailDto>(trail));
            }
            nationalParkDto.trailDtos = trailsDto;
            return Ok(nationalParkDto);
        }

        /// <summary>
        /// Create  Trail
        /// without id 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created,Type =typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateTrial([FromBody] TrailCreateDto trailsDto)
        {
        
            if (trailsDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_tRepo.TrailsExists(trailsDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Trail Exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trail = _mapper.Map<Trail>(trailsDto);
   
            if (_tRepo.CreateTrail(trail))
            {
                return CreatedAtAction("GetTrailsById", new { id = trail.Id }, trail);
            }
           
                ModelState.AddModelError(string.Empty, $"Something went wrong when saving the record {trail.Name}");
                return StatusCode(500,ModelState);

        }

        /// <summary>
        /// Update Trail by id
        /// 
        /// </summary>
        ///<param name="id">Trail Id</param>
        /// <returns></returns>

        [HttpPut("{id:int}",Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int id,[FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailDto.Id != id)
            {
                return BadRequest(ModelState);
            }

            var trail = _mapper.Map<Trail>(trailDto);

            if (!_tRepo.TrailsExists(trail.Id))
            {
                return NotFound(ModelState);
            }
            if (!_tRepo.UpdateTrail(trail))
            {

                ModelState.AddModelError(string.Empty, $"Somthing went wrong when update this trail {trail.Name}");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// delete Trail by id
        /// 
        /// </summary>
        ///<param name="id">Trail Id</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult DeleteTrail(int id)
        {
            
            if (!_tRepo.TrailsExists(id))
            {
                ModelState.AddModelError(string.Empty, "This Trail is not found");
                return NotFound(ModelState);
            }

            var trail = _tRepo.GetTrailById(id);

           
            if (!_tRepo.DeleteTrail(trail))
            {

                ModelState.AddModelError(string.Empty, $"Somthing went wrong when Deleting this trail {trail.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}