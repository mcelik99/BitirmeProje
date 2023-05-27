using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BitirmeProjesi.Models
{
    public class User : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public bool IsAdvisor { get; set; }
        public int Quota { get; set; }

        public virtual List<ChatMessage>? ChatMessages { get; set; }
        public virtual List<Chat>? Chats { get; set; }
        public virtual List<Participant>? Participants { get; set; }
        public virtual List<Period>? Periods { get; set; }


        public string FullName()
        {
            return this.Name + " " + this.Surname;
        }
    }
}
