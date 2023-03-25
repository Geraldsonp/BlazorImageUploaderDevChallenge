namespace ImageUploader.Shared.Models;

public class UploadResult
{
    public bool IsSuccess { get; set; }
    public string ImgUrl { get; set; }
    public string Error { get; set; }
}