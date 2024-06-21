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

    public class DebtController : ControllerBase
    {
        private readonly IDebtRepository _DebtRepo;
        private readonly IDebtDashboardRepository _DebtRepo2;
        public DebtController(IDebtRepository DebtRepo, IDebtDashboardRepository debtRepo2)
        {
            _DebtRepo = DebtRepo;
            _DebtRepo2 = debtRepo2;

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _DebtRepo.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Insert")]
        public async Task<IActionResult> Insert(string? date, string? d_count, string? d_filename, string? d_filetype)
        {
            try
            {
                Debt user = new Debt();
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Insert";
                user.date = date;
                user.d_count = d_count;
                user.d_filename = d_filename;
                user.d_filetype = d_filetype;
                var createduser = await _DebtRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllVal")]
        public async Task<IActionResult> GetAllVal(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllNPA";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllWOAList")]
        public async Task<IActionResult> GetAllWOAList(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllWOAList";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllNPAList")]
        public async Task<IActionResult> GetAllNPAList(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllNPAList";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllSME0")]
        public async Task<IActionResult> GetAllSME0(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllSME0";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllSME1")]
        public async Task<IActionResult> GetAllSME1(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllSME1";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllSME2")]
        public async Task<IActionResult> GetAllSME2(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllSME2";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllWeeklySMA")]
        public async Task<IActionResult> GetAllWeeklySMA(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllWeeklySMA";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllBranchwise")]
        public async Task<IActionResult> GetAllBranchwise(Guid UserId, string? date)
        {
            try
            {
                Debt user = new Debt();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllBranchwise";

                var createduser = await _DebtRepo2.Debt(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
