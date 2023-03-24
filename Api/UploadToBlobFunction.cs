using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ImageUploader.Shared.Models.Commands.Images;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

namespace ApiIsolated
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("Upload")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var log = executionContext.GetLogger("Upload");
            log.LogInformation("C# HTTP trigger function processed a request.");

            var connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var containerName = Environment.GetEnvironmentVariable("ContainerName");

            var blobContainer = new BlobContainerClient(connection, containerName);

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var imageUploadRequest = JsonConvert.DeserializeObject<ImageUploadRequest>(body);

            var blobClient = blobContainer.GetBlobClient(imageUploadRequest.ImgName);

            using var ms = new MemoryStream(imageUploadRequest.ImageBytes);

            if (!blobClient.Exists())
            {
                await blobClient.UploadAsync(ms);
                log.LogInformation($"Uploaded Blob {imageUploadRequest.ImgName}");
                var uri = blobClient.Uri.AbsoluteUri;
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(uri);

                return  response;
            }
            else
            {
                log.LogInformation($"Blob {imageUploadRequest.ImgName} already exist, Returning URI");
                var uri = blobClient.Uri.AbsoluteUri;
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(uri);

                return response;
            }

        }
    }
}
