using Microsoft.AspNetCore.Mvc;
using Data.UrlContext;
using Models.Url;
using Microsoft.EntityFrameworkCore;

namespace Controllers.UrlController
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlContext _context;
        public UrlController(UrlContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async  Task<IActionResult> Create(string OriginalUrl)
        {
            var q = await _context.Urls.FirstOrDefaultAsync(i => i.OriginalUrl == OriginalUrl);
            if (q == null)
            {
                Url url = new Url();
            url.OriginalUrl = OriginalUrl;
            url.ShortenedUrl = ShortUrlGenerator(url.OriginalUrl);

           
            _context.Add(url);
           await _context.SaveChangesAsync();
            
            return  Ok("your generated ShortLink: " + RedirectAddress(url.ShortenedUrl));
            }
            return Ok("The URL already exists, the ShortLink is: "+ RedirectAddress(q.ShortenedUrl));
            

            

        }


    

    
    
    [HttpGet]
    public List<Url> ShowAll(){
        return _context.Urls.ToList();
    }

    [HttpDelete]
    public  ActionResult Delete(){
        _context.Database.ExecuteSqlRaw("TRUNCATE TABLE Urls");
        
        return Ok("DataBase Cleared!");
    }

    [HttpGet("/s/{ShortenedUrl}")]
    public async Task<IActionResult> RedirectAddress(string ShortenedUrl){

        var q = await _context.Urls.FirstOrDefaultAsync(s => s.ShortenedUrl == ShortenedUrl);

        

        return Redirect(q.ShortenedUrl);
    }



        private string ShortUrlGenerator(string OriginalUrl)
        {
            var urlHash = OriginalUrl.GetHashCode();
            return Convert.ToBase64String(BitConverter.GetBytes(urlHash)).Replace("=","").Replace("+","").Replace("/","");
        }



    }
    
}