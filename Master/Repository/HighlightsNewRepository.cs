using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class HighlightsNewRepository: IHighlightsNewRepository
    {
        private readonly DapperContext _context;

        public HighlightsNewRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Highlights(Highlights model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_HighlightsNew", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(Highlights model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_HighlightsNew", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(Highlights user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@f_id", user.f_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@f_fileid", user.f_fileid, DbType.String);
            parameters.Add("@f_docid", user.f_docid, DbType.String);
            parameters.Add("@f_date", user.f_date, DbType.String);
            parameters.Add("@f_filename", user.f_filename, DbType.String);
            parameters.Add("@f_count", user.f_count, DbType.String);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
