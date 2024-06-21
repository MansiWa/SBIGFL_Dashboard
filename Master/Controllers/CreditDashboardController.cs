using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditDashboardController : ControllerBase
    {
        private readonly ICreditDashRepository _DashboardRepo;
        public CreditDashboardController(ICreditDashRepository DashboardRepo)
        {
            _DashboardRepo = DashboardRepo;
        }
        [HttpGet("GetAnnexII")]
        public async Task<IActionResult> GetAnnexII(string UserId, string? date)
        {
            try
            {
                Dashboard user = new Dashboard();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAnnexII";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
