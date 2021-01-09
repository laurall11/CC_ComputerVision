using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CC.AzureVision.REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> OnPostUploadAsync([FromForm] List<IFormFile> file)
        {
            long size = file.Sum(f => f.Length);

            foreach (var formFile in file)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = file.Count, size });
        }


        public string AnalzyeImage()
        {
            IFormCollection test = Request.Form;
            return "hallo";
        }
    }
}
