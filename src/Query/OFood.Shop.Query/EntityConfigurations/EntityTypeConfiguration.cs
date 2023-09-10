using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OFood.Shop.Query.Models;

namespace OFood.Shop.Query.EntityConfigurations
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // clustered index
            builder.Property<Guid>(x => x.Id);


            builder.Property(x => x.CreatedAt)
                  .HasDefaultValueSql("getdate()");

            builder.Property<bool>("IsDeleted")
                .HasDefaultValue(false);

            builder.Property<DateTimeOffset?>("DeletedAt")
                .IsRequired(false);

            builder.HasQueryFilter(p => EF.Property<bool>(p, "IsDeleted") == false);

            builder.Ignore(x => x.DomainEvents);        

            ConfigureDerived(builder);

        }

        public abstract void ConfigureDerived(EntityTypeBuilder<TEntity> builder);

        // ReSharper disable once StaticMemberInGenericType
        private static readonly JsonSerializer Serializer = new JsonSerializer
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
        };

        internal static string Serialize<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                Serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        internal static T Deserialize<T>(string json)
        {
            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize<T>(jsonReader)!;
            }
        }
    }
}