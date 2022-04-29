using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ErrorLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime LogTime { get; set; } = DateTime.Now;
    public DateTime LogTimeUTC { get; set; } = DateTime.UtcNow;
    public string? Message { get; set; }

    public ErrorLog()
    {

    }

    public ErrorLog(string message)
    {
        Message = message;
    }
}

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public double Price { get; set; }
}