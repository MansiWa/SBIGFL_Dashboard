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
	public class CompanyDetailsRepository : ICompanyDetailsRepository
	{
		private readonly DapperContext _context;
		public CompanyDetailsRepository(DapperContext context)
		{
			_context = context;
		}
      
        public async Task<IActionResult> CompanyDetails(CompanyDetails user)
		{
			using (var connection = _context.CreateConnection())
			{
                //Guid? userIdValue = (Guid)user.UserId;
                try
				{
					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();
					var queryResult = await connection.QueryMultipleAsync("proc_CompanyRegistration", SetParameter(user), commandType: CommandType.StoredProcedure);
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

		public DynamicParameters SetParameter(CompanyDetails user)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
			parameters.Add("@com_id", user.com_id, DbType.Guid);
			parameters.Add("@UserId", user.UserId, DbType.Guid);
			parameters.Add("@com_company_name", user.com_company_name, DbType.String);
			parameters.Add("@com_company_name2", user.com_company_name2, DbType.String);
			parameters.Add("@com_owner_name", user.com_owner_name, DbType.String);
			parameters.Add("@com_address", user.com_address, DbType.String);
			parameters.Add("@com_contact", user.com_contact, DbType.String);
			parameters.Add("@com_gst_no", user.com_gst_no, DbType.String);
			parameters.Add("@com_email", user.com_email, DbType.String);
			parameters.Add("@com_website", user.com_website, DbType.String);
			parameters.Add("@com_company_logo", user.com_company_logoc, DbType.Binary, null);
			parameters.Add("@com_bank_name", user.com_bank_name, DbType.String);
			parameters.Add("@com_branch", user.com_branch, DbType.String);
			parameters.Add("@com_acc_no", user.com_acc_no, DbType.String);
			parameters.Add("@com_ifsc", user.com_ifsc, DbType.String);
			parameters.Add("@com_company_logo2", user.com_company_logo2c, DbType.Binary, null);
			parameters.Add("@com_note", user.com_note, DbType.String);
			parameters.Add("@com_otpno", user.com_otpno, DbType.String);
			parameters.Add("@com_staff_no", user.com_staff_no, DbType.String);

			parameters.Add("@Country", user.CountryId, DbType.String);
			parameters.Add("@State", user.StateId, DbType.String);
			parameters.Add("@City", user.CityId, DbType.String);
			parameters.Add("@Currency_format", user.Currency_format, DbType.String);
			parameters.Add("@Server", user.Server, DbType.String);
            parameters.Add("@com_updateddate", user.com_updateddate, DbType.Date);
            parameters.Add("@com_createddate", user.com_createddate, DbType.Date);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
			return parameters;
		}

    }
}
