using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace OFood.Shop.Query.Models.Customers;

[Table("Customers")]
public class Customer : Entity
{
    [Column("PhoneNumber")] 
    public string PhoneNumber { get; set; } = null!;
    
    [ForeignKey("CustomerId")]
    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }= new List<CustomerAddress>();


}