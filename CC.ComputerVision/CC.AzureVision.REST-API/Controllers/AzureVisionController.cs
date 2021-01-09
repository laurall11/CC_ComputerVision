using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace CC.AzureVision.REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureVisionController : ControllerBase
    {
        [EnableCors]
        [HttpGet()]
        public string Test()
        {
            return "hallo";
        }

        [EnableCors]
        [HttpGet("/api/test")]
        public string Test2()
        {
            return "halloTest";
        }

        [DisableCors]
        [HttpPost("/api/analyzeImage")]
        public string AnalzyeImage([FromBody] object image)
        {
            return "hallo";
        }
    }
}
