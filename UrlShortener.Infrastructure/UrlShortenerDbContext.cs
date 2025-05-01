namespace UrlShortener.Infrastructure
{
	using Microsoft.EntityFrameworkCore;

	public class UrlShortenerDbContext : DbContext
	{
		public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
			: base(options)
		{
		}

		public DbSet<UrlEntity> Urls { get; set; }
	}
}
