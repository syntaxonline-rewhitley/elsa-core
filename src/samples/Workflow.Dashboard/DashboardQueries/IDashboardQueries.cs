using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workflow.Dashboard.DashboardQueries
{
    public interface IDashboardQueries
    {
        Task<bool> ResetDatabaseAsync();
    }
}
