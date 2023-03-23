using System;

namespace ImageUploader.Shared.Models.Commands.Images;

public class ImageUploadRequest
{
    public Byte[] ImageBytes { get; set; }
    public string ImgName { get; set; }
}