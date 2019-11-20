﻿using System;
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
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CampController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repository.GetAllCampsAsync();

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
        public async Task<ActionResult<CampModel[]>> SearchByDate([FromQuery] DateTime dateTime, bool includetalks = false)
        {
            try
            {
                var result = await _repository.GetAllCampsByEventDate(dateTime, includetalks);

                if (!result.Any()) return NotFound();

                return _mapper.Map<CampModel[]>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampModel>> Create(CampModel model)
        {
            try
            {
                var Uri = _linkGenerator.GetPathByAction("GET",
                    "Camp",
                    new { moniker = model.Moniker });

                if (string.IsNullOrEmpty(Uri))
                {
                    return BadRequest("could not use current moniker");
                }

                var camp = _mapper.Map<Camp>(model);

                _repository.Add(camp);

                if (await _repository.SaveChangesAsync())
                {
                    return Created(Uri, _mapper.Map<CampModel>(camp));
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
        {
            try
            {
                var oldcamp = _repository.GetCampAsync(moniker);
                if (oldcamp == null) return NotFound($"Camp with {moniker} not found.");

                await  _mapper.Map(model, oldcamp);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(oldcamp);
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }

    }
}
