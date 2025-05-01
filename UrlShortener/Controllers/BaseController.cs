using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
	public class BaseController : ControllerBase
	{
		private readonly IShortenService _shortenerService;
		private readonly IConfiguration _configuration;
		private readonly string _baseUrl;

		public BaseController(IShortenService shortenerService, IConfiguration configuration)
		{
			_shortenerService = shortenerService;
			_configuration = configuration;
			_baseUrl = _configuration["AppSettings:ShortenerBaseUrl"] ?? throw new InvalidOperationException("ShortenerBaseUrl not configured");
			_baseUrl = _baseUrl.TrimEnd('/');
		}
	}
}
