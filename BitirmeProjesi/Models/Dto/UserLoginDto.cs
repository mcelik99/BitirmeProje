﻿using System.ComponentModel.DataAnnotations;

namespace BitirmeProjesi.Models.Dto
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
