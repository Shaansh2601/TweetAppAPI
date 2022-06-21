using TweetAppAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace TweetAppAPI.Model.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<TweetReply> TweetReplies { get; set; }
        public DbSet<TweetLike> TweetLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
