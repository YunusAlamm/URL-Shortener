using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

namespace Controllers.UrlController
{

    [Route("[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlContext _context;
        private readonly JwtSettings _jwtSettings;
        public UrlController(UrlContext context ,IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(string UserName,string UserPassword)
        {
            if ( UserName == null || UserPassword == null)
                return BadRequest("Fill the Name & password to sign-up");
            
            if( await _context.Users.AnyAsync(u => u.UserName == UserName))
                return Ok("User already exists, please sign-in");
           
           
            User user = new User();
            var salt = GenerateSalt();
            var hashedPassword = HashPasswordWithSalt(UserPassword, Encoding.UTF8.GetBytes(Convert.ToBase64String(salt)));
            user.UserPassword = Convert.ToBase64String(hashedPassword);
            user.Salt = Convert.ToBase64String(salt);
            user.UserName = UserName;




            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User Registered!");    

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(String UserName, String UserPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user == null)
                return BadRequest("User Not found!");
            
            var ConfirmPassword = HashPasswordWithSalt(UserPassword,Encoding.UTF8.GetBytes(user.Salt));
            
            var flag =  Convert.ToBase64String(ConfirmPassword);
            if (user.UserPassword ==flag)
            {
                var token = GenerateJwtToken(user);

                return Ok(new{Token = token});
            }

            return Unauthorized("Wrong Password!");

        }
       




        [HttpPost("create")]
        [Authorize]
        public async  Task<IActionResult> Create(string OriginalUrl,string Alias)
        {
           

            
            var q = await _context.Urls.AnyAsync(x => x.OriginalUrl == OriginalUrl);
            if (!q )
            {
                Url url = new Url();
                url.OriginalUrl = OriginalUrl;
                url.ShortenedUrl = ShortUrlGenerator(url.OriginalUrl);
                url.Alias = Alias;

                

                int id =Convert.ToInt32( HttpContext.User.FindFirstValue("Id"));
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);


                user.Urls.Add(url);
                url.User = user;

                 _context.Urls.Add(url);
                await _context.SaveChangesAsync();
            
                 return  Ok("your generated ShortLink: " + RedirectAddress(url.ShortenedUrl));
           }
        var existUrl =  _context.Urls.FirstOrDefault(x => x.OriginalUrl == OriginalUrl);
         return Ok("The URL already exists, the ShortLink is: "+ RedirectAddress(existUrl.ShortenedUrl));
            

            

        }


    

    
    
    [HttpGet("showMyUrls")]
    [Authorize]
    public async Task<IActionResult> ShowMyUrls(){

        int id =Convert.ToInt32( HttpContext.User.FindFirstValue("Id"));
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        var MyUrls =  _context.Urls.Where(x => x.User.Id == id).Select(u => new{u.OriginalUrl,u.ShortenedUrl,u.Alias});
        return Ok(MyUrls);
       

        
    }



[HttpDelete("delete")]
[Authorize]
public async Task<ActionResult> Delete(int id)
{

    int Id =Convert.ToInt32( HttpContext.User.FindFirstValue("Id"));
    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
                
   

    var url = await _context.Urls.FindAsync(id);
    if (url == null)
    {
        return NotFound();
    }

    user.Urls.Remove(url);
   
    await _context.SaveChangesAsync();

    return Ok("URL Deleted!");
}


    [HttpGet("{redirectAddress}")]
    [Authorize]
    public async Task<IActionResult> RedirectAddress(string ShortenedUrl){

        var q = await _context.Urls.FirstOrDefaultAsync(s => s.ShortenedUrl == ShortenedUrl);

        

        return Redirect(q.OriginalUrl);
    }



        private string ShortUrlGenerator(string OriginalUrl)
        {
            var urlHash = OriginalUrl.GetHashCode();
            return Convert.ToBase64String(BitConverter.GetBytes(urlHash)).Replace("=","").Replace("+","").Replace("/","");
        }


        private static byte[] GenerateSalt()
{
    using (var rng = new RNGCryptoServiceProvider())
    {
        var randomNumber = new byte[128 / 8]; // Generate a 128-bit salt
        rng.GetBytes(randomNumber);
        return randomNumber;
    }
}

    private static byte[] HashPasswordWithSalt(string password, byte[] salt)
{
    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); // Use PBKDF2 with 10000 iterations
    return pbkdf2.GetBytes(256 / 8); // Generate a 256-bit hash
}

 
    private string GenerateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim("Id", Convert.ToString(user.Id)),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
        Expires = DateTime.UtcNow.AddMinutes(30),
        Issuer = _jwtSettings.Issuer,
        Audience = _jwtSettings.Audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}




    
    }
}