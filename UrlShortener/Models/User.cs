

namespace UrlShortener.Models
{
    public class User
    {
      
        public int Id { get; set;}
        public string UserName { get; set;}=string.Empty;
        public string UserPassword { get; set;}=string.Empty;
        public string Salt { get; set; } =string.Empty;   //needed for hashing the password.
        public List<Url> Urls { get; set; } = new List<Url>();



    }

    
    
}