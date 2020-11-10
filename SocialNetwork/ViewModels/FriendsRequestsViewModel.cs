﻿using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.ViewModels
{
    public class FriendsRequestsViewModel
    {
        public List<User> Friends { get; set; }
        public List<User> Requests { get; set; }
    }
}