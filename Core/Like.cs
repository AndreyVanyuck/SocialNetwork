﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Core
{
    public class Like
    {
        public int Id { get; set; }

        public Post Post { get; set; }
        public int? PostId { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
