using System;

namespace Common.BuildingBlocks.EventBus.Events
{
    public record IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [System.Text.Json.Serialization.JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [Newtonsoft.Json.JsonProperty]
        public Guid Id { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
