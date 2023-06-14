using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public  class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        public string MessageBody { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public  Student? Student { get; set; }
        public  User? User { get; set; }
        public Chat? Chat { get; set; }
    }
}
