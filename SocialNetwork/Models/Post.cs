using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public enum PostType
    {
        Normal,
        PhotoOnly,
        MainPhoto
    }
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public int? OwnerId { get; set; }

        public PostType Type { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public User Owner { get; set; }

        public Post()
        {
            Photos = new List<Photo>();
            Likes = new HashSet<Like>();
            Comments = new List<Comment>();
        }
    }
}
