using Microsoft.Build.Framework;

namespace BitirmeProjesi.Areas.Student.Models.Dto
{
    public class ParticipantDto
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public List<int> Teachers { get; set; }
    }
}
