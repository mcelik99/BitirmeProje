using System.ComponentModel.DataAnnotations;

namespace BitirmeProjesi.Models.Dto
{
    public class StudentRegisterDto
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string StudentNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string PasswordConfirm { get; set; }

    }
}
