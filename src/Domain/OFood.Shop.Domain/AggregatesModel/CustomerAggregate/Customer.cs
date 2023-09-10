using System.ComponentModel.DataAnnotations.Schema;
using Framework.Core.Spatial;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using OFood.Shop.Domain.Exceptions;
using OFood.Shop.Domain.SeedWork;

namespace OFood.Shop.Domain.AggregatesModel.CustomerAggregate;

[Table("Customers")]
public class Customer : Entity, IAggregateRoot
{
    private readonly List<CustomerAddress> _customerAddresses;
    public string PhoneNumber { get; private set; }

    protected Customer()
    {
        _customerAddresses = new List<CustomerAddress>();
    }

    public Customer(Guid id,string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("invalid phone number");

        Id = id;

        this.PhoneNumber = phoneNumber.Trim();
    }


    [BackingField(nameof(_customerAddresses))]
    public virtual IReadOnlyCollection<CustomerAddress> CustomerAddresses => _customerAddresses;

    public void AddCustomerAddress(int areaId, int cityId, string? extraInfo, GeoLocation location)
    {
        var address = new CustomerAddress(Id, areaId, cityId, extraInfo, location);

        _customerAddresses.Add(address);
    }
}