using System;
namespace Caching.Abstractions
{
    public class PersonLocation
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }
    }
}
