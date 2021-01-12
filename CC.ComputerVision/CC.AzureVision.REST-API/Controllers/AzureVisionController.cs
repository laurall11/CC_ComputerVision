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

        [Microsoft.AspNetCore.Mvc.HttpGet("/api/AzureVision2")]
        public Task Test54()
        {
            Response.ContentType = "audio/wav";

            String FileName;
            FileStream MyFileStream;
            FileName = System.AppDomain.CurrentDomain.BaseDirectory + @"hello.wav";

            MyFileStream = new FileStream(FileName, FileMode.Open);

            //MyFileStream.Close();

            return Response.SendFileAsync(FileName);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/api/AzureVision")]
        public void Test()
        {
            //bla
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/api/getAudio")]
        public HttpResponseMessage Test2()
        {

            string fileDestination =
                System.AppDomain.CurrentDomain.BaseDirectory + @"hello.wav";


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

            return response;
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
