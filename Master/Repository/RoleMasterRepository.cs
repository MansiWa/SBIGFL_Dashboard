using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
namespace Master.Repository
{
	public class RoleMasterRepository : IRoleMasterRepository
	{
		private readonly DapperContext _context;
		public RoleMasterRepository(DapperContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Role(RoleMaster model)
		{

            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
				{
					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();
					var queryResult = await connection.QueryMultipleAsync("proc_RoleMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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
		public async Task<IActionResult> Get(RoleMaster model)
		{

            using (var connection = _context.CreateConnection())

            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
				{
					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();
					var queryResult = await connection.QueryMultipleAsync("proc_RoleMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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

		public DynamicParameters SetParameter(RoleMaster user)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OperationType", user.BaseModel?.OperationType, DbType.String);
			parameters.Add("@userId", user.UserId, DbType.Guid);
			parameters.Add("@r_id", user.r_id, DbType.Guid);
			parameters.Add("@r_rolename", user.r_rolename, DbType.String);
			parameters.Add("@r_description", user.r_description, DbType.String);
			parameters.Add("@r_module", user.r_module, DbType.String);
			parameters.Add("@r_isactive", user.r_isactive, DbType.String);
			parameters.Add("@r_createddate", user.r_createddate, DbType.DateTime);
			parameters.Add("@r_updateddate", user.r_updateddate, DbType.DateTime);
			if (user.DataTable != null && user.DataTable.Rows.Count > 0)
			{
				parameters.Add("@RolePrivilege", user.DataTable.AsTableValuedParameter("[dbo].[Tbl_RolePrivilege]"));
			}
			parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
			return parameters;
		}
	}
}
