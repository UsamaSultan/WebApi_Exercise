using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using  Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using IConfiguration = AutoMapper.Configuration.IConfiguration;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationsController : ControllerBase
    {
        private readonly IConfiguration _config;
        
        public OperationsController(IConfiguration config)
        {
            _config = config;}

        [HttpOptions("reload")]
        public IActionResult Reload()
        {
            try
            {
                var root = (IConfigurationRoot) _config;
                root.Reload();
                return Ok();
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}
