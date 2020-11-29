using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Core
{
    public class Photo
    {
        public int Id { get; set; }
        public string Image { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }
    }
}
