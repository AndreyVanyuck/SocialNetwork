using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Core
{
    public enum FriendshipStatus
    {
        Confirmed,
        Waiting,
        Rejected
    }

    public class Friendship
    {
        public int Id { get; set; }

        [InverseProperty("OutgoingFrienshipRequests")]
        public User RequestFrom { get; set; }

        [InverseProperty("IncomingFrienshipRequests")]
        public User RequestTo { get; set; }

        public FriendshipStatus Status { get; set; }

    }
}
