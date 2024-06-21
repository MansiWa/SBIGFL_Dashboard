using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public readonly IDashboardRepository _dashboardRepo;

        public DashboardController(IDashboardRepository dashRepo)
        {
            _dashboardRepo = dashRepo;
        }
        [HttpGet("GetValue")]
        public async Task<IActionResult> GetValue(string UserId, string? date)
        {
            try
            {
                Dashboard user = new Dashboard();

                user.date = date;
                user.UserId = UserId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetValue";

                var createduser = await _dashboardRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //currently not in use
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string UserId, string Server_Value)
        {
            try
            {
                Dashboard user = new Dashboard();

                user.UserId = UserId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetDetails";

                var createduser = await _dashboardRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //currently not in use

        [HttpGet("GetSub")]
        public async Task<IActionResult> GetSub(string UserId, string Server_Value, string com_id)
        {
            try
            {
                Dashboard user = new Dashboard();

                user.UserId = UserId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetSub";

                var createduser = await _dashboardRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetGrowthFIU")]
        public async Task<IActionResult> GetGrowthFIU(string? UserId)
        {
            try
            {
                Dashboard user = new Dashboard();
                user.UserId = UserId;
                //user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetGrowthFIU";

                var createduser = await _dashboardRepo.GetAll(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("GetFIU")]
        public async Task<IActionResult> GetFIU(Dashboard user)
        {
            try
            {
                // Convert the array of products to a comma-separated string
                string productsString = string.Join(",", user.Product);
                string npaString = string.Join(",", user.client_npa);
                user.ProductString= productsString;
                user.npaString= npaString;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetFIU";

                var createduser = await _dashboardRepo.GetAll(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("GetTurnover")]
        public async Task<IActionResult> GetTurnover(Dashboard user)
        {
            try
            {
                // Convert the array of products to a comma-separated string
                string productsString = string.Join(",", user.Product);
                string npaString = string.Join(",", user.client_npa);
                user.ProductString = productsString;
                user.npaString = npaString;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetTurnover";

                var createduser = await _dashboardRepo.GetAll(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
