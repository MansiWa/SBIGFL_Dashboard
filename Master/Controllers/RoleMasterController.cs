using Master.Repository.Interface;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using System.Data;
using Common;
using Microsoft.AspNetCore.Hosting.Server;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMasterController : ControllerBase
    {
		private readonly IRoleMasterRepository _roleRepo;
        public RoleMasterController(IRoleMasterRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetRole(Guid? UserId, string status)
        {
            try
            {
                RoleMaster user = new RoleMaster();
                user.UserId = UserId;
                user.r_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
				var createduser = await _roleRepo.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetMenu")]
        public async Task<IActionResult> Getmenu(Guid? UserId)
        {
            try
            {
                RoleMaster user = new RoleMaster();
                user.UserId = UserId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetMenu";
				var createduser = await _roleRepo.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetWebMenu")]
        public async Task<IActionResult> Getwebmenu(Guid? UserId)
        {
            try
            {
                RoleMaster user = new RoleMaster();
                user.UserId = UserId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetWebMenu";
                var createduser = await _roleRepo.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetCountry(Guid UserId, Guid? r_id)
        {
            try
            {
                RoleMaster user = new RoleMaster();
                user.UserId = UserId;
                user.r_id = r_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Get";
				var createduser = await _roleRepo.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertRole([FromBody] RoleMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
				if (user.r_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
				}
                else
                {
                    user.BaseModel.OperationType = "Update";
				}
				user.r_createddate = DateTime.Now;
                user.r_updateddate = DateTime.Now;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("a_menuid", typeof(Guid));
                dataTable.Columns.Add("a_addaccess", typeof(string));             
                dataTable.Columns.Add("a_editaccess", typeof(string));
                dataTable.Columns.Add("a_deleteaccess", typeof(string));
                dataTable.Columns.Add("a_viewaccess", typeof(string));
                dataTable.Columns.Add("a_workflow", typeof(string));
                if (user.r_id != null && user.Privilage == null)
                {
                    user.BaseModel.OperationType = "UpdateStatus";
                }
                else
                {
                    foreach (var privilage in user.Privilage)
                    {
                        dataTable.Rows.Add(
                            privilage.a_menuid,
                            privilage.a_addaccess,
                            privilage.a_editaccess,
                            privilage.a_deleteaccess,
                            privilage.a_viewaccess,
                            privilage.a_workflow
                        );
                    }
                    user.Privilage = null;
                    user.DataTable = dataTable;
                }
                var createduser = await _roleRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleMaster user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
			user.BaseModel.OperationType = "Delete";
            var productDetails = await _roleRepo.Get(user);
            return productDetails;
        }

    }
}
