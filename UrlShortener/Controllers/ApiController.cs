namespace UrlShortener.Controllers
{
	using Models;

	using Microsoft.AspNetCore.Mvc;
	using Services;

	[ApiController]
	public class ApiController : ControllerBase
	{
		private readonly IShortenService _shortenerService;
		private readonly IConfiguration _configuration;
		private readonly string _baseUrl;

		public ApiController(IShortenService shortenerService, IConfiguration configuration)
		{
			_shortenerService = shortenerService;
			_configuration = configuration;
			_baseUrl = _configuration["AppSettings:ShortenerBaseUrl"] ?? throw new InvalidOperationException("ShortenerBaseUrl not configured");
			_baseUrl = _baseUrl.TrimEnd('/');
		}

		[HttpPost("shorten")]
		public async Task<IActionResult> ShortenUrl([FromBody] UrlRequest request)
		{
			if (string.IsNullOrEmpty(request.Url))
			{
				return BadRequest("URL cannot be null or empty.");
			}

			if (!Uri.TryCreate(request.Url, UriKind.Absolute, out Uri? uriResult))
			{
				return BadRequest("Invalid URL format.");
			}

			try
			{
				string base62 = await _shortenerService.ShortenUrl(request.Url);
				string shortenedUrl = $"{_baseUrl}/{base62}";
				return Ok(new { ShortenedUrl = shortenedUrl });
			}
			catch (Exception e)
			{
				return StatusCode(500, $"Internal server error.");
			}
		}
	}
}
