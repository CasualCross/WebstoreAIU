namespace WebstoreAIU.Services;

public static class ImageHelper
{
    public static string GetImageSrc(byte[]? imageData)
    {
        if (imageData == null || imageData.Length == 0)
        {
            return ImageService.GetPlaceholderImage();
        }

        var imageText = System.Text.Encoding.UTF8.GetString(imageData);
        var isSvg = imageText.TrimStart().StartsWith("<svg", StringComparison.OrdinalIgnoreCase);
        
        return isSvg 
            ? $"data:image/svg+xml;base64,{Convert.ToBase64String(imageData)}" 
            : $"data:image/jpeg;base64,{Convert.ToBase64String(imageData)}";
    }
}

