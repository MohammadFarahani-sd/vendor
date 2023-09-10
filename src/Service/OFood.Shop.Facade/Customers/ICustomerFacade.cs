using OFood.Shop.Application.Contract.Customers.Command.CommandRequest;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Facade.Customers
{
    public interface ICustomerFacade
    {
        /// <summary>
        /// getting customer by phone number contains some address
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<CustomerResponse?> GetCustomerAsync(string phoneNumber);

        /// <summary>
        /// create new customer 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Guid> CreateCustomerAsync(CreateCustomerRequest request);

        /// <summary>
        /// create new customer address for exist customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Guid> AddAddressToExistCustomerAsync(AddAddressToExistCustomerRequest request);
    }
}