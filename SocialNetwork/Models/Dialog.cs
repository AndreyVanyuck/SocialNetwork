﻿using System;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public class Dialog
    {
        public int Id { get; set; }

        public int User1Id { get; set; }
        public int User2Id { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}