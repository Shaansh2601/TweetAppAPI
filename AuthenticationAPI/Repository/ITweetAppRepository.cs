using System.Collections.Generic;
using TweetAppAPI.Model;

namespace TweetAppAPI.Repository
{
    public interface ITweetAppRepository
    {
        public List<Tweet> GetAllTweets();
        public List<User> GetAllUsers();
        public User GetUserByUsername(string username);
        public List<Tweet> GetUserTweets(string username);
        TweetResponse Tweet(Tweet tweet,string username);
        TweetResponse Update(Tweet tweet,string username,int tweetId);
        public TweetResponse Delete(string username, int tweetId);
        public TweetResponse Like(string username, int tweetId);
        public TweetResponse Reply(TweetReply reply, string username, int tweetId);
    }
}
