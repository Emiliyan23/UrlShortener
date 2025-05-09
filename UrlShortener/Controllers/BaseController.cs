namespace UrlShortener.Controllers
{
	using Microsoft.AspNetCore.Mvc;

	[ApiController]
	public class BaseController : ControllerBase
	{
		protected readonly IConfiguration _configuration;
		protected readonly string _baseUrl;
		protected readonly string[] BlockedDomains;

		protected BaseController(IConfiguration configuration)
		{
			_configuration = configuration;
			_baseUrl = _configuration["AppSettings:ShortenerBaseUrl"] 
			           ?? throw new InvalidOperationException("ShortenerBaseUrl not configured");
			_baseUrl = _baseUrl.TrimEnd('/');
			BlockedDomains = _configuration.GetSection("AppSettings:BlockedDomains").Get<string[]>();
			if (BlockedDomains == null || BlockedDomains.Length == 0)
			{
				throw new InvalidOperationException("BlockedDomains not configured");
			}
		}
	}
}
