using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsLiabilitiesController : ControllerBase
    {
        private readonly IAssetsLiabilitiesRepository _FileUploadRepo;
        public AssetsLiabilitiesController(IAssetsLiabilitiesRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? al_date)
        {
            try
            {
                AssetsLiabilities user = new AssetsLiabilities();
                user.UserId = UserId;
                user.al_date = al_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.AssetsLiabilities(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Insert")]
        public async Task<IActionResult> Insert(string? al_date, string al_docid, string? al_count, string? al_filename, string UserId, string batchid)
        {
            try
            {
                AssetsLiabilities user = new AssetsLiabilities();
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.al_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {

                    //user.BaseModel.OperationType = "Update";
                }
                user.al_updateddate = DateTime.Now;
                user.UserId = new Guid(UserId);
                user.al_date = al_date;
                user.al_docid = al_docid;
                user.al_count = al_count;
                user.al_filename = al_filename;
                user.batchid = batchid;
                var createduser = await _FileUploadRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAssests")]
        public async Task<IActionResult> GetAssests(Guid UserId, string? date)
        {
            try
            {
                AssetsLiabilities user = new AssetsLiabilities();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAssests";

                var createduser = await _FileUploadRepo.AssetsLiabilities(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Deletetreasury([FromBody] AssetsLiabilities user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "DeleteDoc";
            var productDetails = await _FileUploadRepo.Get(user);
            return productDetails;
        }
    }
}
