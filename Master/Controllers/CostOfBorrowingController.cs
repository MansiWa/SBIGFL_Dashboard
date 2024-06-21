using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostOfBorrowingController : ControllerBase
    {
        private readonly ICostOfBorrowingRepository _FileUploadRepo;
        public CostOfBorrowingController(ICostOfBorrowingRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId)
        {
            try
            {
                CostOfBorrowing user = new CostOfBorrowing();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.CostOfBorrowing(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(CostOfBorrowing user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                if (user.cob_id == null)
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
        public async Task<IActionResult> Delete(CostOfBorrowing user)
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
