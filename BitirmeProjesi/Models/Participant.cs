using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        public int PeriodId { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; } = null!;

        // 0=> Yeni Kayıt,1=>Kabul Edildi ,2=>Reddedildi
        public byte? AdvisorStatus { get; set; }

        public DateTime CreateAt { get; set; }

        [ForeignKey("PeriodId")]
        public Period Period { get; set; } = null!;

        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;

        public virtual List<ParticipantTeacher>? ParticipantTeachers { get; set; }
    }
}
