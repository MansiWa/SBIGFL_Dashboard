using Context;
using Dapper;
using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class ReportRepository: IReportRepository
    {
        private readonly DapperContext _context;
        public ReportRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Get(Report model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_Report", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.Read<Object>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model
                        //UserId = userIdValue
                    };

                    if (outcomeId == 1)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> Report(Report model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_Report", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.ReadSingleOrDefault<Object>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model
                        //UserId = userIdValue
                    };

                    if (outcomeId == 1)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public DynamicParameters SetParameter(Report user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@r_id", user.r_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.String);
            parameters.Add("@r_name", user.r_name, DbType.String);
            parameters.Add("@r_remark", user.r_remark, DbType.String);
            parameters.Add("@r_type", user.r_type, DbType.String);
            parameters.Add("@r_date", user.r_date, DbType.DateTime);
            parameters.Add("@r_file", user.r_filecopy, DbType.Binary, null);
            if (user.DataTable != null && user.DataTable.Rows.Count > 0)
            {
                parameters.Add("@protfolioanalysys", user.DataTable.AsTableValuedParameter("[dbo].[Tbl_Portfolio_Analysys]"));
            }
            parameters.Add("@r_createddate", user.r_createddate, DbType.DateTime);
            parameters.Add("@r_updateddate", user.r_updateddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
