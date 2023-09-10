using System.ComponentModel.DataAnnotations;
using Framework.Core.Spatial;

namespace OFood.Shop.Api.Controllers.V1.Customers.Models.Request;

public class CreateCustomerAddressRequest
{
    [Required]
    public int AreaId { get; set; }

    [Required]
    public int CityId { get; set; }

   
    [MaxLength(256)]
    public string? ExtraInfo { get; set; }

    public GeoLocation? Location { get; set; }
}