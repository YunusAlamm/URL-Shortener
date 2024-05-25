

namespace UrlShortener.Models
{
    public class User
    {
        private List<Url> urls;

        public string UserId { get; set;}=string.Empty;
        public string UserName { get; set;}=string.Empty;
        public string UserPassword { get; set;}=string.Empty;
        public string Salt { get; set; } =string.Empty;   //needed for hashing the password.
        public List<Url> Urls { get => urls; set => urls = value; }



    }

    
    
}