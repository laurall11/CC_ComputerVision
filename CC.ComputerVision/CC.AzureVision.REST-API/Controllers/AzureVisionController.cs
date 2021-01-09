using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CC.AzureVision.REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureVisionController : ControllerBase
    {
        [HttpPost("/api/analyzeImage")]
        public void AnalzyeImage()
        {
            
        }
    }
}
