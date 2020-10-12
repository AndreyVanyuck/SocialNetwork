using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Text { get; set; }

        public Post Post { get; set; }
        public int? PostId { get; set; }

        public int? OwnerId { get; set; }
        public User Owner { get; set; }

    }
}
