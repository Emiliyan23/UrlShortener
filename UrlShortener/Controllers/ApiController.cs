namespace UrlShortener.Controllers
{
	using Models;

	using Microsoft.AspNetCore.Mvc;
	using Services;

	[ApiController]
	public class ApiController : BaseController
	{
		private readonly IShortenService _shortenerService;

		public ApiController(IShortenService shortenerService, IConfiguration configuration) : base(configuration)
		{
			_shortenerService = shortenerService;
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

			if (BlockedDomains.Any(domain =>
				    string.Equals(uriResult.Host, domain, StringComparison.OrdinalIgnoreCase) ||
				    uriResult.Host.EndsWith("." + domain, StringComparison.OrdinalIgnoreCase)))
			{
				return BadRequest("URLs from this domain are not accepted.");
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
