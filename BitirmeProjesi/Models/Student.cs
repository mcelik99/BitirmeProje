using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BitirmeProjesi.Models
{
    public class Student
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsVerifed { get; set; }
        public string VerifedCode { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public virtual List<ChatMessage>? ChatMessages { get; set; }
        public virtual List<Chat>? Chats { get; set; }
        public virtual List<Participant>? Participants { get; set; }



        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}
