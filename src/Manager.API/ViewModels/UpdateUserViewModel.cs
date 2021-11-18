using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.API.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "Id required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Id out of range.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name should not be empty.")]
        [MinLength(3, ErrorMessage = "Name should have at least 3 characters.")]
        [MaxLength(80, ErrorMessage = "Name must have a maximum of 80 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email should not be empty.")]
        [MinLength(8, ErrorMessage = "Email should have at least 8 characters.")]
        [MaxLength(180, ErrorMessage = "Email must have a maximum of 180 characters.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                        ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password should not be empty.")]
        [MinLength(6, ErrorMessage = "Password should have at least 6 characters.")]
        [MaxLength(30, ErrorMessage = "Password must have a maximum of 30 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
