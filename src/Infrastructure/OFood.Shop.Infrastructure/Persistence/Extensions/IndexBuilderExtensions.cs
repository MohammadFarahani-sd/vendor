using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OFood.Shop.Infrastructure.Persistence.Extensions
{
    public static class IndexBuilderExtensions
    {
        public static IndexBuilder HasIsDeletedFilter(this IndexBuilder indexBuilder)
        {
            return indexBuilder.HasFilter($"IsDeleted=0");
        }
    }
}