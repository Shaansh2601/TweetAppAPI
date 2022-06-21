using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetAppAPI.Model
{
    public class Tweet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TweetId { get; set; }

        public string Tweet_Message { get; set; }
        public DateTime Tweet_Time { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<TweetLike> TweetLikes { get; set; }
        public List<TweetReply> TweetReplies { get; set; }

    }
}
