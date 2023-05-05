using TweetAppAPI.Model;
using TweetAppAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TweetAppAPI.Controllers
{
    [Route("api/v1.0/tweets/")]
    [ApiController]
    [Authorize]
    public class TweetAppController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITweetAppRepository _tweetAppRepository;

        public TweetAppController(IAuthRepository authRepository,ITweetAppRepository tweetAppRepository)
        {
            _authRepository = authRepository;
            _tweetAppRepository = tweetAppRepository;
        }


//         [AllowAnonymous]
//         [HttpPost("[action]")]
//         public IActionResult Register([FromBody] User userRequest)
//         {
//                 UserCreationStatus response = _authRepository.Register(userRequest);
//                 return Ok(response);
//         }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] UserRequest userRequest)
        {
                UserResponse response = _authRepository.Login(userRequest);
                if (response.Id != 0)
                    return Ok(response);
                else
                    return NotFound(response);
            
        }

        [HttpGet("all")]
        public IActionResult GetAllTweets()
        {

                List<Tweet> tweets = _tweetAppRepository.GetAllTweets();
                return Ok(tweets);
        }

        [HttpGet("users/all")]
        public IActionResult GetAllUsers()
        {
                List<User> users = _tweetAppRepository.GetAllUsers();
                return Ok(users);
        }

        [HttpGet("user/search/{username}")]
        public IActionResult GetUserByUserName(string username)
        {
                User user = _tweetAppRepository.GetUserByUsername(username);
                return Ok(user);
        }

        [HttpGet("{username}")]
        public IActionResult GetUserTweets(string username)
        {
                List<Tweet> tweets = _tweetAppRepository.GetUserTweets(username);
                return Ok(tweets);
        }

        [HttpPost("{username}/[action]")]
        public IActionResult Add(string username,[FromBody] Tweet tweet) 
        {
                TweetResponse tweetResponse = _tweetAppRepository.Tweet(tweet,username);
                if (tweetResponse.TweetId!=-1)
                    return Ok(tweetResponse);
                else
                    return BadRequest(tweetResponse);
        }

        [HttpPut("{username}/[action]/{id}")]
        public IActionResult Update([FromBody] Tweet tweet, string username, string id)
        {
                int tweetID;
                bool res=int.TryParse(id, out tweetID);
                if (res == false)
                    return BadRequest("Invalid Tweet ID");
                TweetResponse tweetResponse = _tweetAppRepository.Update(tweet,username,tweetID);
                return Ok(tweetResponse);
        }

        [HttpDelete("{username}/[action]/{id}")]
        public IActionResult Delete(string username, string id)
        {
                int tweetId;
                bool res = int.TryParse(id, out tweetId);
                if (res == false)
                    return BadRequest("Invalid Tweet ID");
                TweetResponse deletedResponse = _tweetAppRepository.Delete(username,tweetId);
                return Ok(deletedResponse);
            
        }

        [HttpPut("{username}/[action]/{id}")]
        public IActionResult Like(string username, string id)
        {
                int tweetId;
                bool res = int.TryParse(id, out tweetId);
                if (res == false)
                    return BadRequest("Invalid Tweet ID");
                TweetResponse likeResponse = _tweetAppRepository.Like(username, tweetId);
                return Ok(likeResponse);
        }

        [HttpPut("{username}/[action]/{id}")]
        public IActionResult Reply(TweetReply reply,string username, string id)
        {
                int tweetId;
                bool res = int.TryParse(id, out tweetId);
                if (res == false)
                    return BadRequest("Invalid Tweet ID");
                TweetResponse replyResponse = _tweetAppRepository.Reply(reply, username, tweetId);
                return Ok(replyResponse);
        }

        [HttpPost("{username}/[action]")]
        public IActionResult Reset([FromBody] ResetPassword resetPassword,string username)
        {
             UserResponse response = _authRepository.ResetPassword(resetPassword, username);
             return Ok(response);
        }
    }
}
