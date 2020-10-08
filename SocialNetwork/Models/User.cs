﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDay { get; set; }

        public string Email { get; set; }
        public string MobiePhone { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public string School { get; set; }
        public string University { get; set; }

        public string JobPlace { get; set; }
        public string JobPosition { get; set; }


        public ICollection<Friendship> IncomingFrienshipRequests { get; set; }
        public ICollection<Friendship> OutgoingFrienshipRequests { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        public ICollection<Message> MessageFrom { get; set; }
        public ICollection<Message> MessageTo { get; set; }


        public User()
        {
            IncomingFrienshipRequests = new List<Friendship>();
            OutgoingFrienshipRequests = new List<Friendship>();
            Posts = new List<Post>();
            Comments = new List<Comment>();
            Likes = new HashSet<Like>();
            MessageFrom = new List<Message>();
            MessageTo = new List<Message>();
        }

        public override string ToString() => Name + " " + Surname;
    }
}
