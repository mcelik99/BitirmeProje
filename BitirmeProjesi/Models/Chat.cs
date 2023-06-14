using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public  class Chat
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public DateTime CreateAt { get; set; }

        public  Student Student { get; set; } = null!;
        public  User User { get; set; } = null!;

        public virtual List<ChatMessage>? ChatMessages { get; set; }
    }
}
