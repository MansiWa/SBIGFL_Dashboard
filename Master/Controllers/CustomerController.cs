using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IAccountDashboardRepository _AccountDashboardRepo;
        public CustomerController(IAccountDashboardRepository AccountDashboardRepo)
        {
            _AccountDashboardRepo = AccountDashboardRepo;
        }
        [HttpGet("GetFIU_Turnover")]
        public async Task<IActionResult> GetFIU_Turnover(Guid UserId, string? date)
        {
            try
            {
                AccountDashboard user = new AccountDashboard();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetFIU_Turnover";

                var createduser = await _AccountDashboardRepo.CustomerDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetFIU_Outstanding")]
        public async Task<IActionResult> GetFIU_Outstanding(Guid UserId, string? date)
        {
            try
            {
                AccountDashboard user = new AccountDashboard();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetFIU_Outstanding";

                var createduser = await _AccountDashboardRepo.CustomerDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetBranchwise_FIU")]
        public async Task<IActionResult> GetBranchwise_FIU(Guid UserId, string? date)
        {
            try
            {
                AccountDashboard user = new AccountDashboard();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetBranchwise_FIU";

                var createduser = await _AccountDashboardRepo.CustomerDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
