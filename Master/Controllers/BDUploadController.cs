using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BDUploadController : ControllerBase
    {
        private readonly IFileUploadRepository _FileUploadRepo;
        public BDUploadController(IFileUploadRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? bd_date)
        {
            try
            {
                FileUpload user = new FileUpload();
                user.UserId = UserId;
                user.f_date = bd_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.BDUpload(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Update")]
        public async Task<IActionResult> Update(Guid UserId, string? bd_date)
        {
            try
            {
                FileUpload user = new FileUpload();
                user.UserId = UserId;
                user.f_date = bd_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Update";

                var createduser = await _FileUploadRepo.BDUpload(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(FileUpload user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Delete";

                var createduser = await _FileUploadRepo.BDUpload(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Insert")]
        public async Task<IActionResult> Insert(string? f_date, string f_docid, string? f_count, string? f_filename)
        {
            try
            {
                FileUpload user = new FileUpload();
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.f_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {

                    //user.BaseModel.OperationType = "Update";
                }
                user.f_date = f_date;
                user.f_docid = f_docid;
                user.f_count = f_count;
                user.f_filename = f_filename;
                var createduser = await _FileUploadRepo.GetBD(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
