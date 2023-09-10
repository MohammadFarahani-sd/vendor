using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using OFood.Shop.Domain.SeedWork;

namespace OFood.Shop.Domain.AggregatesModel.AreaCityAggregate;

[Table("AreaCities")]
public class AreaCity : IAggregateRoot
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
}