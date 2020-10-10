using MassTransit;
using System;

namespace Workflow.Events.Intake
{
 
    public class FirstCallCreated : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId => Id;
        public string IntakeId { get; set; }
    }

}
