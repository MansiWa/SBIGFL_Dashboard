using Dapper;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data.SqlClient;
using Tokens;
using common.Token;
using Context;
using Master.Entity;
using Master.Repository.Interface;

namespace Master.Repository
{
    public class LoginDetailsRepository:ILoginDetailsRepository
    {
    
        private readonly DapperContext _context;
        public LoginDetailsRepository(DapperContext context)
        {
            _context = context;
        }



        public DynamicParameters SetLogin(LoginDetails user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);

            parameters.Add("@Id", user.Id, DbType.Guid);
            parameters.Add("@com_id", user.com_id, DbType.String);
            parameters.Add("@UserId", user.UserId, DbType.String);
            parameters.Add("@Contact_no", user.Contact_no, DbType.String);
           
            parameters.Add("@com_password", user.com_password, DbType.String);
            //parameters.Add("@NewPassword", user.NewPassword, DbType.String);
            //parameters.Add("@IMEI_No", user.IMEI_No, DbType.String);
            //parameters.Add("@DeviceId", user.DeviceId, DbType.String);
            //parameters.Add("@PlayerId", user.PlayerId, DbType.String);

            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }

        public DynamicParameters Setserver(LoginDetails user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);

            parameters.Add("@Id", user.Id, DbType.Guid);
            parameters.Add("@com_id", user.com_id, DbType.String);
            parameters.Add("@Contact_no", user.Contact_no, DbType.String);
            parameters.Add("@ip_address", user.ip_address, DbType.String);
            parameters.Add("@browser_name", user.browser_name, DbType.String);
            parameters.Add("@browser_version", user.browser_version, DbType.String);
            parameters.Add("@Server_Value", user.BaseModel.Server_Value, DbType.String);
            parameters.Add("@is_signIn", user.is_signIn, DbType.String);
            parameters.Add("@CreatedDate", user.CreatedDate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }



        public async Task<IActionResult> ValidateServer(LoginDetails model)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameter = Setserver(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_ServerDetails", parameter, commandType: CommandType.StoredProcedure);

                    // Retrieve the outcome parameters
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var Model = queryResult.ReadSingleOrDefault<object>();


                    if (outcomeId == 1)
                    {
                        var result = new Result
                        {

                            Outcome = outcome,
                            Data = Model
                        };
                        // Login successful
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        var result = new Result
                        {

                            Outcome = outcome,

                        };
                        // Login failed
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

        public async Task<IActionResult> ValidateEmail(LoginDetails model)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameter = SetLogin(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_LoginDetails", parameter, commandType: CommandType.StoredProcedure);

                    // Retrieve the outcome parameters
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var Model = queryResult.ReadSingleOrDefault<Object>();
                    //var Model = queryResult.Read<Login>().ToList();


                    if (outcomeId == 1)
                    {
                        var result = new Result
                        {

                            Outcome = outcome,
                            Data = Model
                        };
                        // Login successful
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        var result = new Result
                        {

                            Outcome = outcome,

                        };
                        // Login failed
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



        public async Task<IActionResult> LogOut(TokenInfo model)
        {

            using (var connection = _context.CreateConnection())
            {
                TokenRepo tk = new TokenRepo(_context);
                var parameter = tk.SetToken(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_LoginDetails", parameter, commandType: CommandType.StoredProcedure);

                    // Retrieve the outcome parameters
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;

                    if (outcomeId == 1)
                    {
                        // Login successful
                        return new ObjectResult(outcome)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        // Login failed
                        return new ObjectResult(outcome)
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


    }
}

