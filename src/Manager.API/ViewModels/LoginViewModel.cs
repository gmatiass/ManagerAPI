using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login should not be empty.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password should not be empty.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
