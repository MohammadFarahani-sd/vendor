namespace OFood.Shop.Application.Contract.Customers.Command.CommandRequest;

[Serializable]
public class CreateCustomerRequest
{
    public CreateCustomerRequest(Guid id,string phoneNumber)
    {
        Id = id;
        PhoneNumber = phoneNumber;
    }
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
}