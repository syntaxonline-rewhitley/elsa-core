using MassTransit;
using System;

namespace Workflow.Events.FirstCall
{
    public class Testing:CorrelatedBy<Guid>
    {
        public Guid Id { get; set; }

        public Guid CorrelationId => Id;
    }

    public class MorgueEntryDeleted : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public int MorgueEntryId { get; set; }
        public Guid TrackingId { get; set; }
    }

    public class MorgueEntryCreated : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public int MorgueEntryId { get; set; }
        public Guid TrackingId { get; set; }
    }


    public class MorgueEntryUpdated : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public int MorgueEntryId { get; set; }
        public Guid TrackingId { get; set; }
    }

    public class ChangeMorgueEntryStatus : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => Id;
        public int MorgueEntryId { get; set; }
        public string Status { get; set; }
    }

    public class MorgueEntryStatusChanged : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public Guid TrackingId { get; set; }
        public int FirstCallId { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ResetFirstCallDatabase : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public Guid TrackingId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ResetWorkflowDatabase : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public Guid TrackingId { get; set; }
        public DateTime Timestamp { get; set; }
    }
    public class DatabaseResetCompleted : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => TrackingId;
        public Guid TrackingId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
