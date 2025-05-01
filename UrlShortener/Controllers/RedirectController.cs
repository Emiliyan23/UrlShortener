using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
	[ApiController]
	[Route("/")]
	public class RedirectController : Controller
	{
		private readonly IShortenService _shortenerService;
		private readonly IConfiguration _configuration;
		private readonly string _baseUrl;

		public RedirectController(IShortenService shortenerService, IConfiguration configuration)
		{
			_shortenerService = shortenerService;
			_configuration = configuration;
			_baseUrl = _configuration["AppSettings:ShortenerBaseUrl"] ?? throw new InvalidOperationException("ShortenerBaseUrl not configured");
			_baseUrl = _baseUrl.TrimEnd('/');
		}

		[HttpGet]
		[Route("{shortenedUrl}")]
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
