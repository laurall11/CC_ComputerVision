using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;

namespace CC.AzureVision.BackendConsole
{
    class Program
    {
        // Add your Computer Vision subscription key and endpoint
        public static string subscriptionKey = "78556231cd4c409a928718285db5316c";
        public static string endpoint = "https://ccsp.cognitiveservices.azure.com/";
        public static ImageAnalysis result;

        // URL image used for analyzing an image (image of puppy)
        private const string ANALYZE_URL_IMAGE = "C:\\Users\\Flo\\Downloads\\test.jpg";
        private const string ANALYZE_LOCAL_IMAGE = "C:\\Users\\Flo\\AppData\\Local\\Temp\\tmpB172.tmp";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting analyzing service");

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Analyze an image to get features and other properties.

            // AnalyzeImageLocal(client, ANALYZE_LOCAL_IMAGE).Wait();

            // AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
            // Extract text (OCR) from a URL image using the Read API
            // ReadFileUrl(client, READ_TEXT_URL_IMAGE).Wait();
            // Extract text (OCR) from a local image using the Read API
            // ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE).Wait();

            AnalyzeLocalImageFromApi(ANALYZE_LOCAL_IMAGE);
        }

        public static string AnalyzeLocalImageFromApi(string filePath)
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            AnalyzeImageLocal(client, filePath).Wait();

            return result.ToString();
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
        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
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

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, features);

            Console.WriteLine(results);
        }

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
            string subscription = "9452bdccf9f54506afb6c7affd3067bd";
            string region = "westeurope";
            var config = SpeechConfig.FromSubscription(subscription, region);
            using var audioConfig = AudioConfig.FromWavFileOutput(System.AppDomain.CurrentDomain.BaseDirectory + @"hello.wav"); //Path to File
            using var synthesizer = new SpeechSynthesizer(config, audioConfig); //using for saving a file
            //using var synthesizer = new SpeechSynthesizer(config); //using without saving a file

            await synthesizer.SpeakTextAsync(toRead);
        }
    }
}
