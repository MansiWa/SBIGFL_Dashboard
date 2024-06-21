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
        string Server_Value = "";
        public readonly IUserMasterRepository _userMasterRepo;
        public UserMasterController(IUserMasterRepository userMasterRepo)
        {
            _userMasterRepo = userMasterRepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string Server_Value,string Status)
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
                user.um_com_id = comp;
                user.um_isactive = Status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _userMasterRepo.UserMaster(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetStaff")]
        public async Task<IActionResult> GetStaff(Guid?UserId,string Server_Value)
        {
            UserMaster user= new UserMaster();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = UserId;
            user.um_com_id = UserId.ToString();
            user.BaseModel.Server_Value = Server_Value;
            user.BaseModel.OperationType = "GetStaff";
            try
            {
                var parameter = await _userMasterRepo.StaffGet(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid? UserId, Guid um_id,string Server_Value)
        {
            UserMaster user = new UserMaster();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = UserId;
            user.um_id = um_id;
            user.um_com_id = UserId.ToString();
            user.BaseModel.Server_Value = Server_Value;
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
                user.BaseModel.Server_Value = user.Server_Value;
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
            user.BaseModel.Server_Value = user.Server_Value;
            user.BaseModel.OperationType = "Delete";
            var usertDetails = await _userMasterRepo.UserMaster(user);
            return usertDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status, string Server_Value)
        {
            try
            {
                UserMaster user = new UserMaster();
                user.UserId = UserId;
                user.um_com_id = UserId.ToString();
                user.um_isactive = status;
                
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                    user.BaseModel.Server_Value = Server_Value;
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _userMasterRepo.UserMaster(user);
                dynamic data1 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value).Data;
                DataTable data = new DataTable();
                if (data1 is List<object> dataList)
                {
                    if (dataList.Count > 0)
                    {
                        var firstItem = dataList[0] as IDictionary<string, object>;
                        if (firstItem != null)
                        {
                            foreach (var kvp in firstItem)
                            {
                                data.Columns.Add(kvp.Key);
                            }
                        }
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
                ExportRepository export = new ExportRepository();
                var result = new Result
                {
                    Data = export.DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(Guid? UserId, string status, string Server_Value)
        {
            try
            {
                UserMaster user = new UserMaster();
                user.UserId = UserId;
                user.um_com_id = UserId.ToString();
                user.um_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                    user.BaseModel.Server_Value = Server_Value;
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _userMasterRepo.UserMaster(user);
                dynamic data12 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createdUser).Value).Data;
                DataTable data = new DataTable();
                if (data12 is List<object> dataList)
                {
                    if (dataList.Count > 0)
                    {
                        var firstItem = dataList[0] as IDictionary<string, object>;
                        if (firstItem != null)
                        {
                            foreach (var kvp in firstItem)
                            {
                                data.Columns.Add(kvp.Key);
                            }
                        }
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
                htmlContent += "        <h3 style='font-weight: bold;'>User Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Sr.No</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Staff Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>User Name</th>";
                //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Role</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                int a=0;
                foreach (DataRow row in data.Rows)
                {
                    a++;
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + a + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["um_staffname"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["um_user_name"].ToString() + "</td>";
                    //htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["um_rolename"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["um_isactive"].ToString() + "</td>";
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
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
