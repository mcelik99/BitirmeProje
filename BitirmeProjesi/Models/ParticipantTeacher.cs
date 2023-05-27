using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public  class ParticipantTeacher
    {
        [Key]
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public int ParticipantId { get; set; }

        public byte? Status { get; set; }
        public byte? Direction { get; set; }

        [ForeignKey("TeacherId")]
        public User Teacher { get; set; } = null!;
       
    }
}
