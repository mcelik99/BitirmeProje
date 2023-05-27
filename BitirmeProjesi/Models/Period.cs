using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public  class Period
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime FinishAt { get; set; }
        public DateTime CreateAt { get; set; }

        [ForeignKey("CreateUser")]
        public int CreateUserId { get; set; }

        public User? CreateUser { get; set; } = null!;
        public virtual List<Participant>? Participants { get; set; }
    }
}
