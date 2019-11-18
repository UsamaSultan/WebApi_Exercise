using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await  _repository.GetAllCampsAsync();
                
                return Ok(_mapper.Map<CampModel[]>(results));
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> GetwithMoniker(string moniker)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker);

                if (result == null) return NotFound();

                return _mapper.Map<CampModel>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchbyDate([FromQuery] DateTime dateTime, bool includetalks=false)
        {
            try
            {
                var result = await _repository.GetAllCampsByEventDate(dateTime,includetalks);

                if (!result.Any()) return NotFound();

                return _mapper.Map<CampModel[]>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
