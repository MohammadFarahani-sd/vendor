using Framework.Core.Spatial;

namespace OFood.Shop.Application.Contract.Customers.Command.CommandRequest;

[Serializable]
public class CreateCustomerAddressRequest
{
    public CreateCustomerAddressRequest(int areaId,int cityId,  string? extraInfo, GeoLocation? location)
    {
        CityId = cityId;
        AreaId = areaId;
        ExtraInfo = extraInfo;
        Location = location;
    }

    public int AreaId { get; set; }
    public int CityId { get; set; }
    public string? ExtraInfo { get; set; }

    public GeoLocation? Location { get; set; }
}