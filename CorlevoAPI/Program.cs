using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(x => x.AddPolicy("CORS", z => z.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Corlevo API",
        Description = "This is a sample API for `Corlevo` developed to do basic CRUD operations. It uses .Net6.0, EFCore and SQLite database. Swagger is used as API documentation.",
        Contact = new OpenApiContact
        {
            Name = "Yavuz Cingoz",
            Email = "yavuz@flepron.com"
        }
    });
});

builder.Services.AddDbContext<ComplevoContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CORS");

app.Run();