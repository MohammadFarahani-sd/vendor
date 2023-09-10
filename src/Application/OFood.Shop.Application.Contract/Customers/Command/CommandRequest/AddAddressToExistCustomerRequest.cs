namespace OFood.Shop.Application.Contract.Customers.Command.CommandRequest;

[Serializable]
public class AddAddressToExistCustomerRequest
{
    public AddAddressToExistCustomerRequest(Guid customerId, AddCustomerAddressRequest address)
    {
        CustomerId = customerId;
        Address = address;
    }

    public Guid CustomerId { get; set; }

    public AddCustomerAddressRequest Address { get; set; }
}