using Microsoft.EntityFrameworkCore;
using UrlShortener.Infrastructure;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowVueFrontend", policy =>
	{
		policy.WithOrigins("https://emiliyanvasilev.net")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

builder.Services.AddScoped<IShortenService, ShortenService>();

builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseCors("AllowVueFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHsts();

app.UseAuthorization();

app.MapControllers();

app.Run();
