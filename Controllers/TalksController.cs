using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/camp/{moniker}/talks")]
    public class TalksController : ControllerBase
    {
        public LinkGenerator LinkGenerator { get; }
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            LinkGenerator = linkGenerator;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var Talks = await _repository.GetTalksByMonikerAsync(moniker, true);
                if (Talks == null) return NotFound("Cannot find specific Talk.");

                return _mapper.Map<TalkModel[]>(Talks);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error - Failed to get talks with moniker: {moniker}");
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {
            try
            {
                var Talk = await _repository.GetTalkByMonikerAsync(moniker, id);
                if (Talk == null) return NotFound($"No record found with ID: {id}");

                return _mapper.Map<TalkModel>(Talk);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error - Failed to get talks with moniker: {moniker}");
            }

        }


        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel model)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Camp not exist");

                var talk = _mapper.Map<Talk>(model);
                talk.Camp = camp;

                var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                talk.Speaker = speaker;

                _repository.Add(talk);
                if (await _repository.SaveChangesAsync())
                {
                    var url = LinkGenerator.GetPathByAction(HttpContext,
                        "Get",
                        values: new { moniker, id = talk.TalkId });

                    return Created(url, _mapper.Map<TalkModel>(talk));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error - Failed to get talks with moniker: {moniker}");
            }

            return BadRequest($"ERROR-BadRequest: Cannot get talks with moniker: {moniker}");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Update(string moniker, int id, TalkModel model)
        {
            try
            {
                var oldtalk = await _repository.GetTalkByMonikerAsync(moniker, id,true);
                if (oldtalk == null) return NotFound();
                _mapper.Map(model, oldtalk);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TalkModel>(oldtalk);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error - Failed to get talk");
            }

            return BadRequest();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TalkModel>> Delete(string moniker, int id)
        {
            try
            {
                var talk = _repository.GetTalkByMonikerAsync(moniker, id);
                if (talk == null)
                {
                    return NotFound();
                }
                _repository.Delete(talk);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error - Failed to get talk");
            }

            return BadRequest("failed to delete talk");
        }
    }
}
