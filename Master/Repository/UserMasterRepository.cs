using Context;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
using Master.Entity;
using Master.Repository.Interface;

namespace Master.Repository
{
    public class UserMasterRepository:IUserMasterRepository
    {
        private readonly DapperContext _context;
        public UserMasterRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> UserMaster(UserMaster model)
        {

            using (var connection = _context.CreateConnection())

            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_UserMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.Read<Object>().ToList();
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

        public async Task<IActionResult> StaffGet(UserMaster model)
        {

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_UserMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.Read<Object>().ToList();
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
                        return new ObjectResult(Model)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ObjectResult(Model)
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

        public async Task<IActionResult> Get(UserMaster model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_UserMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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
        public DynamicParameters SetParameter(UserMaster user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@um_id", user.um_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@um_user_name", user.um_user_name, DbType.String);
            parameters.Add("@um_password", user.um_password, DbType.String);
            parameters.Add("@um_staffname", user.um_staffname, DbType.String);
            parameters.Add("@um_staffid", user.um_staffid, DbType.String); 
            parameters.Add("@um_roleid", user.um_roleid, DbType.String); 
            parameters.Add("@um_com_id", user.um_com_id, DbType.String); 
            parameters.Add("@um_rolename", user.um_rolename, DbType.String); 
            parameters.Add("@um_isactive", user.um_isactive, DbType.String);
            parameters.Add("@um_createddate", user.um_createddate, DbType.DateTime);
            parameters.Add("@um_updateddate", user.um_updateddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }
    }
}
