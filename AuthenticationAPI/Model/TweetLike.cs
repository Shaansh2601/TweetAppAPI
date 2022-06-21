using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TweetAppAPI.Model
{
    public class TweetLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }

        public int UserId { get; set; }

        public int TweetId { get; set; }
        public DateTime LikedAt { get; } = DateTime.Now;
    }
}
