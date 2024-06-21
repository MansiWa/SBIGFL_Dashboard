using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighlightsNewController : ControllerBase
    {
        private readonly IHighlightsRepository _HighlightsRepo;
        public HighlightsNewController(IHighlightsRepository HighlightsRepo)
        {
            _HighlightsRepo = HighlightsRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? f_date)
        {
            try
            {
                Highlights user = new Highlights();
                user.UserId = UserId;
                user.f_date = f_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _HighlightsRepo.Highlights(user);
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
                Highlights user = new Highlights();
                user.UserId = UserId;
                user.f_date = f_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Update";

                var createduser = await _HighlightsRepo.Highlights(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Highlights user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Delete";

                var createduser = await _HighlightsRepo.Highlights(user);
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
                Highlights user = new Highlights();
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.h_id == null)
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
                var createduser = await _HighlightsRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
