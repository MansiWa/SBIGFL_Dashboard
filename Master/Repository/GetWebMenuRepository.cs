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
	public class GetWebMenuRepository : IGetWebMenuRepository
	{
		private readonly DapperContext _context;
		public GetWebMenuRepository(DapperContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> GetWebMenuR(GetWebMenu model)
		{

            using (var connection = _context.CreateConnection())

            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
				{

					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();

					var queryResult = await connection.QueryMultipleAsync("proc_GetMenu", SetParameter(model), commandType: CommandType.StoredProcedure);
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
						// Login successful
						return new ObjectResult(result)
						{
							StatusCode = 200
						};
					}
					else
					{
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

		public DynamicParameters SetParameter(GetWebMenu user)
		{

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Operation_type", user.BaseModel.OperationType, DbType.String);
			parameters.Add("@RoleId", user.RoleId, DbType.Guid);

			parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


			return parameters;

		}
	}
}
