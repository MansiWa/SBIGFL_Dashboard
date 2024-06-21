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
        string Server_Value="";
		private readonly IRoleMasterRepository _roleRepo;
        public RoleMasterController(IRoleMasterRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetRole(Guid? UserId, string status, string? server)
        {
            try
            {
                RoleMaster user = new RoleMaster();
                user.UserId = UserId;
                user.r_com_id = UserId;
                user.r_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
				user.BaseModel.Server_Value = server;
				var createduser = await _roleRepo.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetMenu")]
        public async Task<IActionResult> Getmenu(Guid? UserId, string? server)
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
				user.BaseModel.Server_Value = server;
				var createduser = await _roleRepo.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetCountry(Guid UserId, Guid? r_id, string? server)
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
				user.BaseModel.Server_Value = server;
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
				user.BaseModel.Server_Value = user.Server_Value;
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
			user.BaseModel.Server_Value = user.Server_Value;
			user.BaseModel.OperationType = "Delete";
            var productDetails = await _roleRepo.Get(user);
            return productDetails;
        }
        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status)
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
                // Fetch data from the repository
                dynamic createduser = await _roleRepo.Role(user);
                dynamic data1 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value).Data;
                DataTable data = new DataTable();
                if (data1 is List<object> dataList)
                {
                    if (dataList.Count > 0)
                    {
                        // Assuming the objects in the list have the same structure, use the first object to create columns
                        var firstItem = dataList[0] as IDictionary<string, object>;
                        if (firstItem != null)
                        {
                            foreach (var kvp in firstItem)
                            {
                                data.Columns.Add(kvp.Key);
                            }
                        }
                        // Populate the DataTable with data from the list
                        foreach (var item in dataList)
                        {
                            var values = item as IDictionary<string, object>;
                            if (values != null)
                            {
                                var row = data.NewRow();
                                foreach (var kvp in values)
                                {
                                    row[kvp.Key] = kvp.Value;
                                }
                                data.Rows.Add(row);
                            }
                        }
                    }
                }
                ExportRepository ep = new ExportRepository();
                // Return the Base64 string as the response
                var result = new Result
                {
                    Data = ep.DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception gracefully (e.g., log it and return an error response)
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(Guid? UserId, string status)
        {
            try
            {
                RoleMaster user = new RoleMaster();
                user.UserId = UserId;
                user.r_isactive = status;
                // Fetch data from the repository
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _roleRepo.Role(user);
                dynamic data12 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createdUser).Value).Data;
                DataTable data = new DataTable();

                if (data12 is List<object> dataList)
                {
                    if (dataList.Count > 0)
                    {
                        // Assuming the objects in the list have the same structure, use the first object to create columns
                        var firstItem = dataList[0] as IDictionary<string, object>;
                        if (firstItem != null)
                        {
                            foreach (var kvp in firstItem)
                            {
                                data.Columns.Add(kvp.Key);
                            }
                        }
                        // Populate the DataTable with data from the list
                        foreach (var item in dataList)
                        {
                            var values = item as IDictionary<string, object>;
                            if (values != null)
                            {
                                var row = data.NewRow();
                                foreach (var kvp in values)
                                {
                                    row[kvp.Key] = kvp.Value;
                                }
                                data.Rows.Add(row);
                            }
                        }
                    }
                }
                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
                htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                htmlContent += "        <h3 style='font-weight: bold;'>Role Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Role Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Description</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Module</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Menu Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Is Active</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? r_rolename = row["r_rolename"].ToString();
                    string? r_description = row["r_description"].ToString();
                    string? r_module = row["r_module"].ToString();
                    string? a_menuid = row["m_menuname"].ToString();
                    string? r_isactive = row["r_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + r_rolename + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + r_description + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + r_module + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + a_menuid + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + r_isactive + "</td>";
                    htmlContent += "</tr>";
                }
                htmlContent += "        </tbody>";
                htmlContent += "    </table>";
                htmlContent += "</div>";
                string date = DateTime.Now.ToString("dd-MM-yyyy--HH-mm");
                return Ok(htmlContent);
            }
            catch (Exception ex)
            {
                // Handle the exception gracefully (e.g., log it and return an error response)
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
