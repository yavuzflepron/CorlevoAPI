public class AddProductRequest
{
    public string Name { get; set; }
    public double Price { get; set; }
}

public class UpdateProductRequest
{
    public string? Name { get; set; }
    public double? Price { get; set; }
}

public class SearchProductRequest
{
    public string? SearchText { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
}