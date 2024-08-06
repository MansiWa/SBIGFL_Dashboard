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

        [HttpGet("GetAnnexIII")]
        public async Task<IActionResult> GetAnnexIII(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexIII";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAnnexIV")]
        public async Task<IActionResult> GetAnnexIV(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexIV";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAnnexVI")]
        public async Task<IActionResult> GetAnnexVI(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexVI";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAnnexVII")]
        public async Task<IActionResult> GetAnnexVII(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexVII";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAnnexIX")]
        public async Task<IActionResult> GetAnnexIX(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexIX";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAnnexX")]
        public async Task<IActionResult> GetAnnexX(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexX";

                var createduser = await _DashboardRepo.Credit(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAnnexXI")]
        public async Task<IActionResult> GetAnnexXI(string UserId, string? date)
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
                user.BaseModel.OperationType = "GetAnnexXI";

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
