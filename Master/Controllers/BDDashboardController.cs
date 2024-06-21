using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BDDashboardController : ControllerBase
    {
        private readonly IAccountDashboardRepository _AccountDashboardRepo;
        public BDDashboardController(IAccountDashboardRepository AccountDashboardRepo)
        {
            _AccountDashboardRepo = AccountDashboardRepo;
        }
        [HttpGet("GetSummary")]
        public async Task<IActionResult> GetSummary(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetSummary";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetNBO")]
        public async Task<IActionResult> GetNBO(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetNBO";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetFSA")]
        public async Task<IActionResult> GetFSA(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetFSA";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetWithCredit")]
        public async Task<IActionResult> GetWithCredit(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetWithCredit";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetSanctioned")]
        public async Task<IActionResult> GetSanctioned(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetSanctioned";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetDisbursalNew")]
        public async Task<IActionResult> GetDisbursalNew(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetDisbursalNew";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet("GetBDSummary")]
        public async Task<IActionResult> GetBDSummary(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetBDSummary";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("GetBDCredit")]
        public async Task<IActionResult> GetBDCredit(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetBDCredit";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetBDSanctioned")]
        public async Task<IActionResult> GetBDSanctioned(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetBDSanctioned";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetBDQueryResolve")]
        public async Task<IActionResult> GetBDQueryResolve(Guid UserId, string? date)
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
                user.BaseModel.OperationType = "GetBDQueryResolve";

                var createduser = await _AccountDashboardRepo.BDDashboard(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
