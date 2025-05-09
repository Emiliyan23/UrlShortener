using System.Threading.RateLimiting;

using Microsoft.AspNetCore.HttpOverrides;
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
		policy.WithOrigins("https://emiliyanvasilev.net", "https://www.emiliyanvasilev.net", "http://localhost:5173")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

builder.Services.AddRateLimiter(options =>
{
	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
	{
		if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
		{
			return RateLimitPartition.GetFixedWindowLimiter("OPTIONS", _ => new FixedWindowRateLimiterOptions
			{
				PermitLimit = int.MaxValue, // Allow nearly unlimited requests for OPTIONS
				Window = TimeSpan.FromSeconds(1),
				QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
				QueueLimit = 0,
			});
		}

		string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
		return RateLimitPartition.GetFixedWindowLimiter(clientIp, key => new FixedWindowRateLimiterOptions
		{
			PermitLimit = 1,
			Window = TimeSpan.FromMinutes(1),
			QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
			QueueLimit = 0,
		});
	});
	options.OnRejected = (context, token) =>
	{
		context.HttpContext.Response.StatusCode = 429;
		return ValueTask.CompletedTask;
	};
});

builder.Services.AddScoped<IShortenService, ShortenService>();

builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	// Trust headers sent by the Azure frontend
	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
	app.UseHsts();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowVueFrontend");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
