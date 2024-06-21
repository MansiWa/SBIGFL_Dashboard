using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class CostOfBorrowingRepository: ICostOfBorrowingRepository
    {
        private readonly DapperContext _context;

        public CostOfBorrowingRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CostOfBorrowing(CostOfBorrowing model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_CostOfBorrowing", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(CostOfBorrowing model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_CostOfBorrowing", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(CostOfBorrowing user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@cob_id", user.cob_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@cob_period", user.cob_period, DbType.String);
            parameters.Add("@cob_finantialyear", user.cob_finantialyear, DbType.String);
            parameters.Add("@cob_includingncd", user.cob_includingncd, DbType.String);
            parameters.Add("@cob_excludingncd", user.cob_excludingncd, DbType.String);
            parameters.Add("@cob_createddate", user.cob_createddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
