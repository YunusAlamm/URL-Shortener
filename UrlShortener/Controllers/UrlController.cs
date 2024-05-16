using Microsoft.AspNetCore.Mvc;
using Data.UrlContext;
using Models.Url;
using Microsoft.EntityFrameworkCore;

namespace Controllers.UrlController
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly UrlContext _context;
        public UrlController(UrlContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(string OriginalUrl)
        {
            Url url = new Url();
            url.OriginalUrl = OriginalUrl;
            url.ShortenedUrl = ShortUrlGenerator(OriginalUrl);

            foreach (var item in _context.Urls)     // checks that if the Original Url already exists in db, prevent to save it again.
            {
                if (item.OriginalUrl == OriginalUrl)
                return Redirect(url.ShortenedUrl);
            }
            _context.Add(url);
            _context.SaveChanges();
            return Redirect(url.ShortenedUrl);

        }





        public string ShortUrlGenerator(string OriginalUrl)
        {
            var urlHash = OriginalUrl.GetHashCode();
            return Convert.ToBase64String(BitConverter.GetBytes(urlHash)).Replace("=","").Replace("+","").Replace("/","");
        }



    }
    
}