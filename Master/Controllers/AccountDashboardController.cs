using Common;
using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web.WebPages;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDashboardController : ControllerBase
    {
        private readonly IAccountDashboardRepository _AccountDashboardRepo;
        public AccountDashboardController(IAccountDashboardRepository AccountDashboardRepo)
        {
            _AccountDashboardRepo = AccountDashboardRepo;
        }
        [HttpGet("GetAllFinhighlights")]
        public async Task<IActionResult> GetAllFinhighlights(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetFinhighlights";

                var createduser = await _AccountDashboardRepo.AccountDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAllYeild")]
        public async Task<IActionResult> GetAllYeild(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetYeild";

                var createduser = await _AccountDashboardRepo.AccountDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetHighlights")]
        public async Task<IActionResult> GetHighlights(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetHighlights";

                var createduser = await _AccountDashboardRepo.AccountDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAllMourevised")]
        public async Task<IActionResult> GetAllMourevised(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetMourevised";

                var createduser = await _AccountDashboardRepo.AccountDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAllEffratio")]
        public async Task<IActionResult> GetAllEffratio(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetEffratio";

                var createduser = await _AccountDashboardRepo.AccountDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
