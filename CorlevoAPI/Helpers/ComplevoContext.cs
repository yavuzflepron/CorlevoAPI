using Microsoft.EntityFrameworkCore;

public class ComplevoContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    public ComplevoContext(DbContextOptions<ComplevoContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    internal void WriteErrorLog(Exception ex)
    {
        var message = ex.InnerException?.Message ?? ex.Message;
        ErrorLogs.Add(new(message));
        SaveChanges();
    }
}