using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadBorroRFRController : ControllerBase
    {
        private readonly IRFRDocRepository _FileUploadRepo;
        public UploadBorroRFRController(IRFRDocRepository FileUploadRepo)
        {
            _FileUploadRepo = FileUploadRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? rb_date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.rb_date = rb_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Insert")]
        public async Task<IActionResult> Insert(string? rb_date, string rb_docid, string? rb_count, string? rb_filename, string UserId, string rb_doctype, string batchid)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.rb_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {

                    //user.BaseModel.OperationType = "Update";
                }
                user.rb_updateddate = DateTime.Now;
                user.UserId = new Guid(UserId);
                user.rb_date = rb_date;
                user.rb_docid = rb_docid;
                user.rb_count = rb_count;
                user.rb_filename = rb_filename;
                user.rb_doctype = rb_doctype;
                user.batchid = batchid;
                var createduser = await _FileUploadRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRFR")]
        public async Task<IActionResult> GetRFR(Guid UserId)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetRFR";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetBorrow")]
        public async Task<IActionResult> GetBorrow(Guid UserId,string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetBorrow";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetBorrowValue")]
        public async Task<IActionResult> GetBorrowValue(Guid UserId,string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetBorrowValue";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetTIER_II_BOND")]
        public async Task<IActionResult> GetTIER_II_BOND(Guid UserId, string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetTIER_II_BOND";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetCommercial_Paper")]
        public async Task<IActionResult> GetCommercial_Paper(Guid UserId, string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetCommercial_Paper";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetBank_Line_CC")]
        public async Task<IActionResult> GetBank_Line_CC(Guid UserId, string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetBank_Line_CC";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetBank_Line_WCL")]
        public async Task<IActionResult> GetBank_Line_WCL(Guid UserId, string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetBank_Line_WCL";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetFOREX")]
        public async Task<IActionResult> GetFOREX(Guid UserId, string? date)
        {
            try
            {
                BorroAndRFR user = new BorroAndRFR();
                user.UserId = UserId;
                user.date = date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetFOREX";

                var createduser = await _FileUploadRepo.Report(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Deletetreasury([FromBody] BorroAndRFR user)
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