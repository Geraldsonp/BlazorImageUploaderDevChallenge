using System.Net.Http.Json;
using ImageUploader.Shared.Models;
using ImageUploader.Shared.Models.Commands.Images;
using Microsoft.AspNetCore.Components.Forms;

namespace ImageUploader.Client.Services;

public class ImageService : IImageService
{
    private string ApiAddress { get; set; } = "/api/Images";
    
    private readonly HttpClient _httpClient;

    public ImageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UploadResult> UploadToBlobAsync(ImageUploadRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiAddress, request);

        var result = new UploadResult
        {
            IsSuccess = response.IsSuccessStatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            result.ImgUrl = (await response.Content.ReadFromJsonAsync<string>()).Trim('"');
        }
        else
        {
            result.Error = (await response.Content.ReadFromJsonAsync<string>()).Trim('"');
        }

        return result;
    }
    
}