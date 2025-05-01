namespace UrlShortener.Utils
{
	public class Base62
	{
		private const string _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		private static readonly int _base = _alphabet.Length;

		public static string Encode(int value)
		{
			if (value == 0)
				return _alphabet[0].ToString();

			var result = string.Empty;
			while (value > 0)
			{
				var remainder = value % _base;
				result = _alphabet[remainder] + result;
				value /= _base;
			}

			return result;
		}

		public static int Decode(string base62)
		{
			if (string.IsNullOrEmpty(base62))
			{
				throw new ArgumentNullException(nameof(base62), "Input cannot be null or empty.");
			}

			int result = 0;
			foreach (var c in base62)
			{
				int index = _alphabet.IndexOf(c);
				if (index < 0)
					throw new FormatException($"Invalid character '{c}' in Base62 string.");

				result = result * _base + index;
			}

			return result;
		}
	}
}
