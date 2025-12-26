namespace WebstoreAIU.Services;

public static class ImageGenerator
{
    public static byte[] GeneratePlaceholderImage(string text, int width = 400, int height = 300)
    {
        var svg = $@"<svg width=""{width}"" height=""{height}"" xmlns=""http://www.w3.org/2000/svg"">
            <defs>
                <linearGradient id=""grad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
                    <stop offset=""0%"" style=""stop-color:#667eea;stop-opacity:1"" />
                    <stop offset=""100%"" style=""stop-color:#764ba2;stop-opacity:1"" />
                </linearGradient>
            </defs>
            <rect width=""{width}"" height=""{height}"" fill=""url(#grad)""/>
            <text x=""50%"" y=""50%"" text-anchor=""middle"" dominant-baseline=""middle"" 
                  font-family=""Arial, sans-serif"" font-size=""28"" font-weight=""bold"" fill=""white"">{text}</text>
        </svg>";
        
        return System.Text.Encoding.UTF8.GetBytes(svg);
    }
    
    public static byte[] GeneratePlaceholderImageAsJpeg(string text, int width = 400, int height = 300)
    {
        return GeneratePlaceholderImage(text, width, height);
    }
}

