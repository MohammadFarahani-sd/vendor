using System.ComponentModel.DataAnnotations;

namespace OFood.Shop.Api.Controllers.V1.Customers.Models.Request;

public class CreateCustomerRequest
{
    [Required] 
    [MinLength(6)] 
    public string PhoneNumber { get; set; } = null!;


    [Required] 
    public CreateCustomerAddressRequest Address { get; set; } = null!;
}