# Building an URL Shortener Web API using Asp.Net Core 8

We're going to build a project that takes a normal URL from the input and uses the algorithms we've defined to show us the shortened form of that link.
# Project Technologies
. Asp.Net Core 8<br>
. MVC Structure<br>
. Sql local database file<br>
. Database Migrations
# How It Works?
in our prject we have a model called **URL** which is like this:
```csharp
  public class Url
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Url]
        public string OriginalUrl { get; set; }
        
        public string ShortenedUrl { get; set; }
    }
```
and a DBcontext which its code is:
```csharp

public class UrlContext : DbContext
    {
        public UrlContext(DbContextOptions<UrlContext> options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }
    }
```
the program saves the normal form of a link in its **OriginalUrl** property during calling the **Create Action** then by using the **GenerateShortenedUrl** produce the ShortenedUrl and Saves it in our DB context.<br>
Create Action:
```csharp
[HttpPost]
    public async Task<IActionResult> Create([FromBody] ShortUrl shortUrl)
    {
        if (ModelState.IsValid)
        {
            shortUrl.ShortenedUrl = GenerateShortenedUrl(shortUrl.OriginalUrl);
            _context.Add(shortUrl);
            await _context.SaveChangesAsync();
            return Ok(shortUrl.ShortenedUrl);
        }
        return BadRequest(ModelState);
    }
```
And here is  our Generator:
```csharp
private string GenerateShortenedUrl(string originalUrl)
    {
        var urlHash = originalUrl.GetHashCode();
        return Convert.ToBase64String(BitConverter.GetBytes(urlHash)).Replace("=", "").Replace("+", "").Replace("/", "");
    }
```
but it is not done yet. the **ShortenedUrl** is just a String that is not redirected to its Original Address.<br>
for that purpose we use the **RedirectToOriginal** to finish the job, this action by receiving a Shortened Url looks for its match in our database among the saved entities(saved by Create action) and when it finds its match, uses the Redirect() method which is a pre-written method in the **ControllerBase.cs** and deliver us a **Shortened Url**, redirected to its Original address and ready to use.
RedirectToOriginal codes:
```csharp
[HttpGet("/s/{shortenedUrl}")]
    public async Task<IActionResult> RedirectToOriginal(string shortenedUrl)
    {
        var shortUrl = await _context.ShortUrls
            .FirstOrDefaultAsync(s => s.ShortenedUrl == shortenedUrl);

        if (shortUrl == null)
        {
            return NotFound();
        }

        return Redirect(shortUrl.OriginalUrl);
    }
```
# Additional Notes
for database i used a sql server db local file which is easy to make.<br>
 just open the SSMS, Create an empty database then Detach the database file and put it in the root directory of your project.<br>
for connecting a database local file to your program and configure your connection string should do some search in the internet.