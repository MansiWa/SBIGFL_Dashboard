using Context;
using Dapper;
using Master.API.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{ 
    public class CurrencyRateMasterRepository : ICurrencyRateMasterRepository
    {
        private readonly DapperContext _context;
        public CurrencyRateMasterRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CurrencyRate(CurrencyRateMaster model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_CurrencyRateMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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
        public async Task<IActionResult> Get(CurrencyRateMaster model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_CurrencyRateMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(CurrencyRateMaster user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@cr_id", user.cr_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@cr_currencyid", user.cr_currencyid, DbType.String);
            parameters.Add("@cr_currencyrate", user.cr_currencyrate, DbType.String);
            parameters.Add("@cr_fromdate", user.cr_fromdate, DbType.Date);
            parameters.Add("@cr_todate", user.cr_todate, DbType.Date);
            parameters.Add("@cr_currency_name", user.cr_currency_name, DbType.String);
            parameters.Add("@cr_isactive", user.cr_isactive, DbType.String);
            parameters.Add("@cr_createddate", user.cr_createddate, DbType.DateTime);
            parameters.Add("@cr_updateddate", user.cr_updateddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
