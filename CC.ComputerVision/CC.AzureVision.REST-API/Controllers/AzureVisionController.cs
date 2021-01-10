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
        public string OnPostUploadAsync([FromForm] List<IFormFile> file)
        {
            long size = file.Sum(f => f.Length);
            string path = "";

            foreach (var formFile in file)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    path = filePath;

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyToAsync(stream).Wait();
                    }
                }
            }

            Program.AnalyzeLocalImageFromApi(path);

            return "hurensohn";
        }
    }
}
