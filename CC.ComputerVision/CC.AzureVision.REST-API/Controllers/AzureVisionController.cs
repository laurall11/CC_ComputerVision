using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CC.AzureVision.REST_API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AzureVisionController : ControllerBase
    {

        [Microsoft.AspNetCore.Mvc.HttpGet()]
        public string Test()
        {
            return "hallo";
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/api/getAudio")]
        public string Test2()
        {
            return "halloTest";

            string fileDestination =
                System.AppDomain.CurrentDomain.BaseDirectory + @"hello.mp3";


            //converting .wav file into bytes array  
            var dataBytes = System.IO.File.ReadAllBytes(fileDestination);

            HttpResponseMessage response = null;


            response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(dataBytes)
            };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "hello.wav";
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = dataBytes.Length;
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

            Program.AnalyzeLocalImageFromApi(path);

            string fileDestination =
                System.AppDomain.CurrentDomain.BaseDirectory + @"hello.mp3";


            //converting .wav file into bytes array  
            var dataBytes = System.IO.File.ReadAllBytes(fileDestination);

            HttpResponseMessage response = null;


            response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(dataBytes)
            };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "hello.wav";
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = dataBytes.Length;

            //return response;
        }
    }
}
