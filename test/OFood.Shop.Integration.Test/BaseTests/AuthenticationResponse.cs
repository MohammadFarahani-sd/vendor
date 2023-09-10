using System.ComponentModel.DataAnnotations;

namespace OFood.Shop.Integration.Test.BaseTests
{
    public class AuthenticateResponse
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}