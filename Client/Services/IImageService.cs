using ImageUploader.Shared.Models;
using ImageUploader.Shared.Models.Commands.Images;
using Microsoft.AspNetCore.Components.Forms;

namespace ImageUploader.Client.Services;

public interface IImageService
{
    Task<UploadResult> UploadAsync(ImageUploadRequest request);
    Task<UploadResult> UploadToBlobAsync(ImageUploadRequest request);
}