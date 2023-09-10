using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using OFood.Shop.Query.Models.Customers;

namespace OFood.Shop.Query.Models.AreaCities;

[Table("AreaCities")]
public class AreaCity 
{
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsDeleted { get; protected set; } = false;
    public DateTimeOffset? ModifiedAt { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public string? Slug { get; set; }
    
    [ForeignKey(nameof(ParentId))]
    public virtual ICollection<AreaCity> SubAreaCities { get; set; }

    public CustomerAddress CustomerAddress { get; set; }
}