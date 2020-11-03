using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        
        public int? DialogId { get; set; }
        public Dialog Dialog { get; set; }

        public int? UserFromId { get; set; }
        public int? UserToId { get; set; }

        [InverseProperty("MessageFrom")]
        public User UserFrom { get; set; }
        [InverseProperty("MessageTo")]
        public User UserTo { get; set; }
        
        public override string ToString() => Date.ToString() + "\n" + UserFrom + ": \n" + Text + "\n";

    }
}
