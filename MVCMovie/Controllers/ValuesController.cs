using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCMovie.Controllers
{
    //restful api
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // Get /api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Get /api/values/5
        [HttpGet("{selectId}")]     
        public ActionResult<string> GetSelectId(int selectId)  //此处方法名无所谓
        {
            return "value:" + selectId;
        }

        // Post /api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return "post successfule value:" + value;
        }

        [HttpPut("{selectId}")]
        public ActionResult<string> Put(int selectId, [FromBody] string value)
        {
            return "post successfule value:" + value;
        }

        [HttpDelete("{selectId}")]
        public ActionResult<string> Delete(int selectId)
        {
            Console.WriteLine("delete : " + selectId);
            return "HttpDelete successfule value:" + selectId;
        }
    }
}
