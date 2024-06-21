using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class TBillRepository: ITBillRepository
    {
        private readonly DapperContext _context;

        public TBillRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> TBill(TBill model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_TBill", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(TBill model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_TBill", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(TBill user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@tbr_id", user.tbr_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@tbr_date", user.tbr_date, DbType.DateTime);
            parameters.Add("@tbr_30_days", user.tbr_30_days, DbType.String);
            parameters.Add("@tbr_60_days", user.tbr_60_days, DbType.String);
            parameters.Add("@tbr_90_days", user.tbr_90_days, DbType.String);
            parameters.Add("@tbr_182_days", user.tbr_182_days, DbType.String);
            parameters.Add("@tbr_364_days", user.tbr_364_days, DbType.String);
            parameters.Add("@tbr_createddate", user.tbr_createddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
