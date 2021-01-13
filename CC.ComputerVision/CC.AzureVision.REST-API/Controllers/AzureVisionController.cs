using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CC.AzureVision.REST_API.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AzureVisionController : ControllerBase
    {
        private static string resultString;


        [Microsoft.AspNetCore.Mvc.HttpGet("/api/getAudio")]
        public Task SendAudio()
        {

            Response.ContentType = "audio/wav";

            String fileName = System.AppDomain.CurrentDomain.BaseDirectory + @"hello.wav";

            return Response.SendFileAsync(fileName);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/api/getDescription")]
        public Task SendDescription()
        {

            Response.ContentType = "text/plain";

            return Response.WriteAsync(resultString);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost("/api/analyzeImage")]
        public void AnalyzeImage([FromForm] List<IFormFile> file)
        {
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

            resultString = Program.AnalyzeLocalImageFromApi(path);
        }
    }
}
