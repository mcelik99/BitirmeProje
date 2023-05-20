using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public  class PeriodStudent
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Period")]
        public int PeriodId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public string Subject { get; set; } = null!;
        public byte? AdvisorStatus { get; set; }
        public byte? TeacherStatus { get; set; }
        public DateTime CreateAt { get; set; }

        public Period Period { get; set; } = null!;
        public Student Student { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
