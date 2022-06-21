using System.ComponentModel.DataAnnotations;

namespace TweetAppAPI.Model
{
    public class UserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
