using System.ComponentModel.DataAnnotations;

namespace OFood.Shop.Api.Controllers.V1.Authenticate.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Otp is required")]
        public string Otp { get; set; } = null!;
    }
    public class ApproachModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; } = null!;
    }
}
