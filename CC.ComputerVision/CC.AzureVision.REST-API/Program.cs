using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CC.AzureVision.REST_API
{
    public class Program
    {
        // Add your Computer Vision subscription key and endpoint
        public static string subscriptionKey = "VISION SUBSCRIPTIONKEY";
        public static string endpoint = "ENDPOINT";
        public static ImageAnalysis result;



        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Console.WriteLine("Starting analyzing service");

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static string AnalyzeLocalImageFromApi(string filePath)
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            AnalyzeImageLocal(client, filePath).Wait();

            string resultString = "In this picture is " + result.Description.Captions[0].Text;

            if (result.Description.Tags.Count > 1)
            {
                int cnt = 0;
                resultString = resultString + ". The tags for this image are ";
                foreach (var tag in result.Description.Tags)
                {
                    resultString = resultString + tag;
                    cnt++;
                    if (cnt < result.Description.Tags.Count)
                    {
                        resultString = resultString + " and ";
                    }
                }
            }
            else
            {
                resultString = resultString + ". The tag for this image is " + result.Description.Tags[0];
            }

            if (result.Categories.Count > 1)
            {
                int cnt = 0;
                resultString = resultString + ". The categories which represent this image are ";
                foreach (var category in result.Categories)
                {
                    resultString = resultString + category.Name.Replace("_", " ");
                    cnt++;
                    if (cnt < result.Categories.Count)
                    {
                        resultString = resultString + " and ";
                    }
                }
            }
            else if (result.Categories.Count > 0)
            {
                resultString = resultString + ". The category which represents this image is " + result.Categories[0].Name.Replace("_", " ");
            }

            if (result.Faces.Count > 1)
            {
                int cnt = 0;
                resultString = resultString + ". There are " + result.Faces.Count + " faces in this image.";
                foreach (var face in result.Faces)
                {
                    resultString = resultString + " There is a " + face.Gender + " Face with the age of " + face.Age;
                    cnt++;
                    if (cnt < result.Faces.Count)
                    {
                        resultString = resultString + " and ";
                    }
                    else
                    {
                        resultString = resultString + " in this image";
                    }

                }

            }
            else if (result.Faces.Count > 0)
            {
                resultString = resultString + ". There is one " + result.Faces[0].Gender + " face with the age of " + result.Faces[0].Age + " in this image";
            }


            resultString = resultString + ". The dominant color in the background is " +
                           result.Color.DominantColorBackground;
            resultString = resultString + ". The dominant color in the foreground is " +
                           result.Color.DominantColorForeground;



            if (result.Color.DominantColors.Count > 1)
            {
                string colorPrev = "";
                int cnt = 0;
                resultString = resultString + ". The dominant colors in this image are ";
                foreach (var color in result.Color.DominantColors)
                {
                    if (!color.Equals(colorPrev))
                    {
                        resultString = resultString + color;
                        colorPrev = color;
                        cnt++;
                        if (cnt < result.Color.DominantColors.Count)
                        {
                            resultString = resultString + " and ";
                        }

                    }
                }
            }
            else if (result.Color.DominantColors.Count > 0)
            {
                resultString = resultString + ". The dominant color in this image is " + result.Color.DominantColors[0];
            }

            if (result.Brands.Count > 0)
            {
                string brandPrev = "";
                foreach (var brand in result.Brands)
                {
                    if (!brand.Name.Equals(brandPrev))
                    {
                        resultString = resultString + ". This image contains information for the brand " + brand.Name;
                        brandPrev = brand.Name;
                    }
                }
            }
            else
            {
                resultString = resultString + ". This image contains no brand information";
            }

            if (result.Adult.IsAdultContent)
            {
                resultString = resultString + ". This image contains adult content";
            }
            else
            {
                resultString = resultString + ". This image contains no adult content";
            }

            if (result.Adult.IsGoryContent)
            {
                resultString = resultString + ". This image contains gory content";
            }
            else
            {
                resultString = resultString + ". This image contains no gory content";
            }

            if (result.Adult.IsRacyContent)
            {
                resultString = resultString + ". This image contains racy content";
            }
            else
            {
                resultString = resultString + ". This image contains no racy content";
            }



            SynthesizeAudioAsync(resultString).Wait();

            Console.WriteLine(resultString);

            return resultString;
        }

        //Authenticate Client
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };
            return client;
        }

        /* 
         * ANALYZE IMAGE - URL IMAGE
         * Analyze URL image. Extracts captions, categories, tags, objects, faces, racy/adult content,
         * brands, celebrities, landmarks, color scheme, and image types.
         */
        
        public static async Task AnalyzeImageLocal(ComputerVisionClient client, string imageLocal)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - LOCAL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageLocal)}...");
            Console.WriteLine();

            using (Stream analyzeImageStream = File.OpenRead(imageLocal))
            {
                // Analyze the URL image 
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(analyzeImageStream, features);

                result = results;

                Console.WriteLine(results);
            }

        }

        static async Task SynthesizeAudioAsync(string toRead)
        {
            string subscription = "COGNITIVE AUDIO SUBSCRIPTIONKEY";
            string region = "westeurope";
            var config = SpeechConfig.FromSubscription(subscription, region);
            //config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio24Khz96KBitRateMonoMp3);
            using var audioConfig = AudioConfig.FromWavFileOutput(System.AppDomain.CurrentDomain.BaseDirectory + @"hello.wav"); //Path to File
            using var synthesizer = new SpeechSynthesizer(config, audioConfig); //using for saving a file
            //using var synthesizer = new SpeechSynthesizer(config); //using without saving a file

            await synthesizer.SpeakTextAsync(toRead);
        }
    }
}
