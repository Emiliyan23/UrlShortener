namespace UrlShortener.Controllers
{
	using Models;

	using Microsoft.AspNetCore.Mvc;
	using Services;

	[ApiController]
	[Route("api/shorten")]
	public class UrlShortenerController : ControllerBase
	{
		private readonly IShortenService _shortenerService;
		private readonly IConfiguration _configuration;
		private readonly string _baseUrl;

		public UrlShortenerController(IShortenService shortenerService, IConfiguration configuration)
		{
			_shortenerService = shortenerService;
			_configuration = configuration;
			_baseUrl = _configuration["AppSettings:ShortenerBaseUrl"] ?? throw new InvalidOperationException("ShortenerBaseUrl not configured");
			_baseUrl = _baseUrl.TrimEnd('/');
		}

		[HttpPost]
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
				string shortenedUrl = $"{_baseUrl}/short/{base62}";
				return Ok(new { ShortenedUrl = shortenedUrl });
			}
			catch (Exception e)
			{
				return StatusCode(500, $"Internal server error.");
			}
		}

		[HttpGet]
		[Route("/short/{shortenedUrl}")]
		public async Task<IActionResult> GetOriginalUrl(string shortenedUrl)
		{
			if (string.IsNullOrEmpty(shortenedUrl))
			{
				return BadRequest("Shortened URL cannot be null or empty.");
			}

			try
			{
				string originalUrl = await _shortenerService.GetOriginalUrl(shortenedUrl);
				return RedirectPermanent(originalUrl);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
