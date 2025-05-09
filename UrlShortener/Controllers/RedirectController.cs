using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
	[ApiController]
	[Route("/")]
	public class RedirectController : BaseController
	{
		private readonly IShortenService _shortenerService;

		public RedirectController(IShortenService shortenerService, IConfiguration configuration) : base(configuration)
		{
			_shortenerService = shortenerService;
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
