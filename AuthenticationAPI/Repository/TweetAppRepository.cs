using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TweetAppAPI.Model;
using TweetAppAPI.Model.Data;

namespace TweetAppAPI.Repository
{
    public class TweetAppRepository : ITweetAppRepository
    {
        private readonly UserDbContext _context;

        public TweetAppRepository(UserDbContext context)
        {
            _context = context;
        }

        public List<Tweet> GetAllTweets()
        {
            List<Tweet> allTweets = new List<Tweet>();
            try
            {
                allTweets = _context.Tweets.Include("TweetReplies").Include("TweetLikes").ToList();
                if (allTweets!=null)
                {
                    foreach (Tweet tweet in allTweets)
                    {
                        User tweetUser = _context.Users.Where(u => u.UserId == tweet.UserId).FirstOrDefault();
                        if (tweetUser != null) {
                            tweet.User = new User();
                            tweet.User.First_Name = tweetUser.First_Name;
                            tweet.User.Last_Name = tweetUser.Last_Name;
                            tweet.User.DOB = tweetUser.DOB;
                            tweet.User.Email = tweetUser.Email;
                            tweet.User.Gender = tweetUser.Gender;
                            tweet.User.CreatedAt = tweetUser.CreatedAt;
                        }
                        tweet.User=null;
                    }
                }
                return allTweets;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                var users = _context.Users.ToList();
                return users;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                var user = _context.Users.Where(u => u.Email.Equals(username)).FirstOrDefault();
                return user;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Tweet> GetUserTweets(string username)
        {
            List<Tweet> userTweets = new List<Tweet>();
            try
            {
                User user = _context.Users.Where(t => t.Email == username).FirstOrDefault();
                if (user == null)
                    return null;
                userTweets = _context.Tweets.Include("TweetReplies").Include("TweetLikes").Where(t => t.UserId == user.UserId).ToList();
                if (userTweets != null)
                {
                    foreach (Tweet tweet in userTweets)
                    {
                        User tweetUser = _context.Users.Where(u => u.UserId == tweet.UserId).FirstOrDefault();
                        if (tweetUser != null)
                        {
                            tweet.User = new User();
                            tweet.User.First_Name = tweetUser.First_Name;
                            tweet.User.Last_Name = tweetUser.Last_Name;
                            tweet.User.DOB = tweetUser.DOB;
                            tweet.User.Email = tweetUser.Email;
                            tweet.User.Gender = tweetUser.Gender;
                            tweet.User.CreatedAt = tweetUser.CreatedAt;
                        }
                        tweet.User = null;
                    }
                }
                return userTweets;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public TweetResponse Tweet(Tweet tweet,string username)
        {
            TweetResponse tweetResponse = new TweetResponse();
            try
            {
                User userDB = _context.Users.Where(t => t.Email== username).FirstOrDefault();
                _context.Tweets.Add(new Tweet() { Tweet_Message = tweet.Tweet_Message, Tweet_Time = DateTime.Now, UpdatedAt = DateTime.Now, UserId = userDB.UserId });
                _context.SaveChanges();
                tweetResponse.TweetId = tweet.TweetId;
                tweetResponse.Message = "Tweeted";
                return tweetResponse;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public TweetResponse Update(Tweet tweet,string username,int id)
        {
            TweetResponse tweetResponse = new TweetResponse();
            try
            {
                User userDB = _context.Users.Where(u => u.Email.Equals(username)).FirstOrDefault();
                Tweet tweetDB = _context.Tweets.Where(t => t.TweetId == id && t.User.Email.Equals(username)).FirstOrDefault();
                tweetDB.Tweet_Message = tweet.Tweet_Message;
                tweetDB.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                tweetResponse.TweetId = tweet.TweetId;
                tweetResponse.Message = "Updated";
                return tweetResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public TweetResponse Delete(string username,int tweetId)
        {
            TweetResponse tweetResponse = new TweetResponse();
            try
            {
                User userDB = _context.Users.Where(u => u.Email.Equals(username)).FirstOrDefault();
                Tweet tweetDB = _context.Tweets.Where(t => t.TweetId == tweetId && t.User.Email.Equals(username)).FirstOrDefault();
                _context.Tweets.Remove(tweetDB);
                _context.SaveChanges();
                tweetResponse.Message = "Deleted..";
                return tweetResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public TweetResponse Like(string username, int tweetId)
        {
            TweetResponse likeResponse = new TweetResponse();
            try
            {  
                User userDB = _context.Users.Where(u => u.Email.Equals(username)).FirstOrDefault();
                Tweet tweetDB = _context.Tweets.Where(t => t.TweetId == tweetId).FirstOrDefault();
                TweetLike likeDB = _context.TweetLikes.Where(t => t.TweetId == tweetId && t.UserId==userDB.UserId).FirstOrDefault();
                TweetLike like = new TweetLike()
                {
                    TweetId = tweetId,
                    UserId = userDB.UserId
                };
                _context.TweetLikes.Add(like);
                _context.SaveChanges();
                likeResponse.Message = "Liked";
                return likeResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public TweetResponse Reply(TweetReply reply, string username, int tweetId)
        {
            TweetResponse replyResponse = new TweetResponse();
            try
            {
                User userDB = _context.Users.Where(u => u.Email.Equals(username)).FirstOrDefault();
                Tweet tweetDB = _context.Tweets.Where(t => t.TweetId == tweetId).FirstOrDefault();
                _context.TweetReplies.Add(new TweetReply() { Reply_Message = reply.Reply_Message,UserId = userDB.UserId,TweetId=tweetId });
                _context.SaveChanges();
                replyResponse.Message = "Replied";
                return replyResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }
       
    }
}
