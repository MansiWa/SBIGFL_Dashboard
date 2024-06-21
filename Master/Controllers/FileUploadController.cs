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
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadRepository _FileUploadRepo;
        public FileUploadController(IFileUploadRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? f_date)
        {
            try
            {
                FileUpload user = new FileUpload();
                user.UserId = UserId;
                user.f_date = f_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.FileUpload(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Update")]
        public async Task<IActionResult> Update(Guid UserId, string? f_date)
        {
            try
            {
                FileUpload user = new FileUpload();
                user.UserId = UserId;
                user.f_date = f_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Update";

                var createduser = await _FileUploadRepo.FileUpload(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(Guid UserId, string? f_date)
        {
            try
            {
                FileUpload user = new FileUpload();
                user.UserId = UserId;
                user.f_date = f_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Delete";

                var createduser = await _FileUploadRepo.FileUpload(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Insert")]
        public async Task<IActionResult> Insert(string? f_date,string f_docid, string? f_count,string? f_filename)
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
                user.f_date=f_date;
                user.f_docid = f_docid;
                user.f_count = f_count;
                user.f_filename = f_filename;
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
