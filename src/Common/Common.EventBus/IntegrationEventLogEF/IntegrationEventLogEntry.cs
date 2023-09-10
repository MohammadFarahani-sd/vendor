using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Common.BuildingBlocks.EventBus;
using Common.BuildingBlocks.EventBus.Events;
using Newtonsoft.Json;

namespace Common.BuildingBlocks.IntegrationEventLogEF
{
    [Table("IntegrationEventLog")]
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry()
        {
            
        }
        public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }

        [Column("EventId")]
        public Guid EventId { get; private set; }

        [Column("EventTypeName", TypeName = "varchar(200)")]
        public string EventTypeName { get; private set; }

        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();

        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }

        [Column("State")]
        public EventStateEnum State { get; set; }

        [Column("TimesSent")]
        public int TimesSent { get; set; }

        [Column("CreationTime")]
        public DateTimeOffset CreationTime { get; private set; }

        [Column("Content", TypeName = "varchar(4000)")]
        public string Content { get; private set; }

        [Column("TransactionId", TypeName = "varchar(200)")]
        public string TransactionId { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
            return this;
        }
    }
}
