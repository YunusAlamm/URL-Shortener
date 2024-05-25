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


        [HttpPost("Register")]
        public async Task<IActionResult> Register(string UserName,string UserPassword)
        {
            if ( UserName == null || UserPassword == null)
                return BadRequest("Fill the Name & password to sign-up");
            

            User user = new User();
            var salt = GenerateSalt();
            var hashedPassword = HashPasswordWithSalt(UserPassword, salt);
            user.UserPassword = Convert.ToBase64String(hashedPassword);
            user.Salt = Convert.ToBase64String(salt);
            user.UserName = UserName;




            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User Registered!");    

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(String UserName, String UserPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user == null)
                return BadRequest("User Not found!");
            
            var ConfirmSubject = HashPasswordWithSalt(UserPassword,Encoding.UTF8.GetBytes(user.Salt));
            var flag =  Convert.ToBase64String(ConfirmSubject);
            if (user.UserPassword ==flag)
            {
                var token = GenerateJwtToken(user);

                return Ok(new{Token = token});
            }

            return Unauthorized("ok nistim");

        }
       




        [HttpPost("Create")]
        [Authorize]
        public async  Task<IActionResult> Create(string OriginalUrl)
        {
            var q = await _context.Urls.FirstOrDefaultAsync(i => i.OriginalUrl == OriginalUrl);
            if (q == null)
            {
                Url url = new Url();
            url.OriginalUrl = OriginalUrl;
            url.ShortenedUrl = ShortUrlGenerator(url.OriginalUrl);


           var userName = User.Identity.Name;
           var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
           user.Urls.Add(url);
           url.User = user;

            _context.Add(url);
           await _context.SaveChangesAsync();
            
            return  Ok("your generated ShortLink: " + RedirectAddress(url.ShortenedUrl));
            }
            return Ok("The URL already exists, the ShortLink is: "+ RedirectAddress(q.ShortenedUrl));
            

            

        }


    

    
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ShowAll(){

        var userName = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName ==userName);

        return (IActionResult)user.Urls.ToList();
       

        
    }

    [HttpDelete]
    [Authorize]
    public  ActionResult Delete(){
        _context.Database.ExecuteSqlRaw("TRUNCATE TABLE Urls");
        
        return Ok("DataBase Cleared!");
    }

    [HttpGet("{RedirectAddress}")]
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
            new Claim("Id", user.UserId),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
        Expires = DateTime.UtcNow.AddMinutes(5),
        Issuer = _jwtSettings.Issuer,
        Audience = _jwtSettings.Audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}




    
    }
}