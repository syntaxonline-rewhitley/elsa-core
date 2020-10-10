using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
namespace Workflow.Dashboard.DashboardQueries
{
    public class DashboardQueries : IDashboardQueries
    {
        private readonly string _connectionString;
        public DashboardQueries(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<bool> ResetDatabaseAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var sqlStatement = @"BEGIN TRY
                                  BEGIN TRAN
                                  DELETE FROM BlockingActivities
                                  DELETE FROM ActivityInstances
                                  DELETE FROM WorkflowInstances
                                  DELETE FROM ConnectionDefinitions where WorkflowDefinitionVersionId in (select Id from WorkflowDefinitionVersions where IsLatest = 0)
                                  DELETE FROM ActivityDefinitions where WorkflowDefinitionVersionId in (select Id from WorkflowDefinitionVersions where IsLatest = 0)
                                  DELETE FROM WorkflowDefinitionVersions where IsLatest = 0
                                  UPDATE WorkflowDefinitionVersions set [Version] = 1 where IsLatest = 1
                                  COMMIT TRAN
                                  SELECT 'Reset successful' as msg
                                END TRY
                                BEGIN CATCH
                                  SELECT ERROR_MESSAGE() as msg
                                  ROLLBACK TRAN
                                END CATCH";
            var results = await connection.QueryAsync<dynamic>($"{sqlStatement}");
            return true;
        }
    }
}
