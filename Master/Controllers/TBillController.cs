using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TBillController : ControllerBase
    {
        private readonly ITBillRepository _FileUploadRepo;
        public TBillController(ITBillRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId)
        {
            try
            {
                TBill user = new TBill();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.TBill(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetTopAll")]
        public async Task<IActionResult> GetTopAll(Guid UserId)
        {
            try
            {
                TBill user = new TBill();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetTopAll";

                var createduser = await _FileUploadRepo.TBill(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(TBill user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if(user.tbr_id== null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _FileUploadRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(TBill user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                    user.BaseModel.OperationType = "Delete";

                var createduser = await _FileUploadRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
