namespace UrlShortener.Models
{
    public class Url
    {
        public int Id { get; set; }

        public string OriginalUrl { get; set; } =string.Empty;
        public string ShortenedUrl { get; set; } =string.Empty;
        public string Alias { get; set; } =string.Empty;
        public int Counter { get; set; } = 0;
        public User User { get; set; } = new User();
    }

}