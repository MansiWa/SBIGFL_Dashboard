using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;


namespace Master.Repository.Interface
{
	public interface ICompanyDetailsRepository
	{
		public Task<IActionResult> CompanyDetails(CompanyDetails user);
		
	}
}
