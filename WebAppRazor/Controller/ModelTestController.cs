using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppRazor.Models;

namespace WebAppRazor.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelTestController : ControllerBase
    {
        private readonly ILogger<ModelTestController> _logger;
        public ModelTestController(ILogger<ModelTestController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public void Post(FuturesDTO data)
        {
            _logger.LogInformation(data.Name + " " + data.Age + " " + data.ReleaseDate);
        }
        [HttpGet]
        public string Get([Range(1, 100)] int age)
        {
            return (age.ToString());
        }
    }
}
