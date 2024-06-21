using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditRatingController : ControllerBase
    {
        private readonly ICreditRatingRepository _FileUploadRepo;
        public CreditRatingController(ICreditRatingRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId)
        {
            try
            {
                CreditRating user = new CreditRating();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.CreditRating(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(CreditRating user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.cr_id == null)
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
        [HttpPost("DeleteCr")]
        public async Task<IActionResult> DeleteCr(CreditRating user)
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
        [HttpGet("GetAllPRR")]
        public async Task<IActionResult> GetAllprr(Guid UserId)
        {
            try
            {
                CreditRating user = new CreditRating();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllPRR";

                var createduser = await _FileUploadRepo.CreditRating(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("InsertPRR")]
        public async Task<IActionResult> InsertPRR(CreditRating user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.pr_id == null)
                {
                    user.BaseModel.OperationType = "InsertPRR";
                }
                else
                {
                    user.BaseModel.OperationType = "UpdatePRR";
                }
                var createduser = await _FileUploadRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("DeletePRR")]
        public async Task<IActionResult> DeletePRR(CreditRating user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "DeletePRR";

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
