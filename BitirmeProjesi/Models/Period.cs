using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public int CreateUserId { get; set; }

        public User CreateUser { get; set; } = null!;
        public virtual List<PeriodStudent>? PeriodStudents { get; set; }
    }
}
