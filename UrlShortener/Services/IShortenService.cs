namespace UrlShortener.Services
{
	public interface IShortenService
	{
		public Task<string> ShortenUrl(string url);

		public Task<string> GetOriginalUrl(string base62);
	}
}
