using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace CC.AzureVision.REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200", headers: "accept", methods: "*")]
    public class AzureVisionController : ControllerBase
    {

        [HttpGet()]
        public string Test()
        {
            return "hallo";
        }

        [HttpGet("/api/test")]
        public string Test2()
        {
            return "halloTest";
        }

        [HttpPost("/api/analyzeImage")]
        public string AnalzyeImage([FromBody] object image)
        {
            return "hallo";
        }
    }
}
