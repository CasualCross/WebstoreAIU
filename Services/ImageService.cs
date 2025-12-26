using System.Text.Json;
using WebstoreAIU.Models;

namespace WebstoreAIU.Services;

public static class ImageService
{
    public static string GetImageDataUrl(byte[]? imageData, string contentType = "image/jpeg")
    {
        if (imageData == null || imageData.Length == 0)
        {
            return GetPlaceholderImage();
        }
        return $"data:{contentType};base64,{Convert.ToBase64String(imageData)}";
    }

    public static string GetPlaceholderImage()
    {
        var svg = "<svg width='400' height='300' xmlns='http://www.w3.org/2000/svg'><rect width='400' height='300' fill='#e0e0e0'/><text x='50%' y='50%' text-anchor='middle' fill='#999' font-family='Arial' font-size='18'>No Image</text></svg>";
        return $"data:image/svg+xml;base64,{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svg))}";
    }

    public static List<ItemImage> ParseAdditionalImages(string json)
    {
        if (string.IsNullOrWhiteSpace(json) || json == "[]")
        {
            return new List<ItemImage>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<ItemImage>>(json) ?? new List<ItemImage>();
        }
        catch
        {
            return new List<ItemImage>();
        }
    }

    public static string SerializeAdditionalImages(List<ItemImage> images)
    {
        return JsonSerializer.Serialize(images);
    }

    public static async Task<byte[]?> ConvertIFormFileToByteArray(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}

