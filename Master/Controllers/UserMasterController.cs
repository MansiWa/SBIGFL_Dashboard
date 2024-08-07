using common;
using Common;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
using Master.Entity;
using Master.Repository.Interface;

namespace Master.Controllers 
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class UserMasterController : Controller
    {
        public readonly IUserMasterRepository _userMasterRepo;
        public UserMasterController(IUserMasterRepository userMasterRepo)
        {
            _userMasterRepo = userMasterRepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId,string Status)
        {
            try
            {
                if(Status== null)
                {
                    Status = "1";
                }
                UserMaster user = new UserMaster();
                string comp = UserId.ToString();
                user.UserId = UserId;
                user.um_isactive = Status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _userMasterRepo.UserMaster(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid? UserId, Guid um_id)
        {
            UserMaster user = new UserMaster();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = UserId;
            user.um_id = um_id;
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _userMasterRepo.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] UserMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.um_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.um_updateddate = DateTime.Now;
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _userMasterRepo.UserMaster(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] UserMaster user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var usertDetails = await _userMasterRepo.UserMaster(user);
            return usertDetails;
        }
   }
}
