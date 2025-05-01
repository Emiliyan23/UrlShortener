namespace UrlShortener.Infrastructure
{
	using System.ComponentModel.DataAnnotations;

	public class UrlEntity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Url { get; set; } = null!;

		public string? Base62 { get; set; }

		public DateTime CreatedAt { get; set; }

		[Required]
		public DateTime ExpirationDate { get; set; }

		public int ClickCount { get; set; }
	}
}
