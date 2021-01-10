using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public HttpResponseMessage AnalyzeImage([FromForm] List<IFormFile> file)
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

            //converting .wav file into bytes array  
            var dataBytes = System.IO.File.ReadAllBytes(System.AppDomain.CurrentDomain.BaseDirectory + @"hello.mp3");

            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = new StreamContent(dataStream);

            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = "test";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return httpResponseMessage;
        }
    }
}
