using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TweetAppAPI.Model
{
    public class TweetReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        public string Reply_Message { get; set; }

        public int UserId { get; set; }

        public int TweetId { get; set; }
        public DateTime RepliedAt { get; } = DateTime.Now;
    }
}
