using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using Workflow.Events.FirstCall;
using Workflow.Dashboard.DashboardQueries;

namespace Workflow.Dashboard.Consumers
{
    public class ResetWorkflowDatabaseServiceConsumer : IConsumer<ResetWorkflowDatabase>
    {
        private readonly ILogger<ResetWorkflowDatabaseServiceConsumer> _logger;
        private readonly IDashboardQueries _dashboardQueries;

        public ResetWorkflowDatabaseServiceConsumer(
            ILogger<ResetWorkflowDatabaseServiceConsumer> logger,
            IDashboardQueries dashboardQueries
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dashboardQueries = dashboardQueries ?? throw new ArgumentNullException(nameof(dashboardQueries));
        }
        public async Task Consume(ConsumeContext<ResetWorkflowDatabase> context)
        {
            _logger.LogInformation("Resetting workflow database");
            await _dashboardQueries.ResetDatabaseAsync();
        }
    }
}
