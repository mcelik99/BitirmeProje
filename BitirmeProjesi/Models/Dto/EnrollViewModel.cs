using Microsoft.AspNetCore.Mvc.Rendering;

namespace BitirmeProjesi.Models.Dto
{
    public class EnrollViewModel
    {
        public string Subject { get; set; }
        public List<int> SelectedUserIds { get; set; }
        public List<SelectListItem> Users { get; set; }
        public int SelectedPeriodId { get; set; }
        public List<SelectListItem> Periods { get; set; }
    }
}
