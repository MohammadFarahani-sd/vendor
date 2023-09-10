using System.ComponentModel.DataAnnotations;

namespace OFood.Shop.Api.Controllers.V1.Authenticate.Models;
public class CustomerRegisterModel
{
    [Required(ErrorMessage = "Phone number is required")]
    public string Username { get; set; } = null!;
}
