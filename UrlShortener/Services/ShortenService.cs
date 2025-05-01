namespace UrlShortener.Services
{
	using Infrastructure;
	using Utils;

	public class ShortenService : IShortenService
	{
		private readonly UrlShortenerDbContext _dbContext;

		public ShortenService(UrlShortenerDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<string> ShortenUrl(string url)
		{
			UrlEntity urlEntity = new UrlEntity
			{
				Url = url,
				CreatedAt = DateTime.UtcNow,
				ExpirationDate = DateTime.UtcNow.AddDays(30),
				ClickCount = 0
			};

			await _dbContext.Urls.AddAsync(urlEntity);
			await _dbContext.SaveChangesAsync();

			string base62 = Base62.Encode(urlEntity.Id);
			urlEntity.Base62 = base62;

			_dbContext.Urls.Update(urlEntity);
			await _dbContext.SaveChangesAsync();

			return base62;
		}

		public async Task<string> GetOriginalUrl(string base62)
		{
			int id = Base62.Decode(base62);
			UrlEntity? urlEntity = await _dbContext.Urls.FindAsync(id);

			if (urlEntity == null || urlEntity.ExpirationDate < DateTime.UtcNow)
			{
				throw new Exception("URL not found or expired.");
			}

			urlEntity.ClickCount++;

			_dbContext.Urls.Update(urlEntity);
			await _dbContext.SaveChangesAsync();

			return urlEntity.Url;
		}
	}
}
