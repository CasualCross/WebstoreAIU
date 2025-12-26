namespace WebstoreAIU.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public byte[]? MainImage { get; set; }
    public string AdditionalImagesJson { get; set; } = "[]";
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}

