using Common;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DesignationMasterController : ControllerBase
    {
        private readonly IDesignationMasterRepository _designationRepo;
        public DesignationMasterController(IDesignationMasterRepository designationRepo)
        {
            _designationRepo = designationRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string? DepartmentId, string status)
        {
            try
            {
                DesignationMaster user = new DesignationMaster();
                user.UserId = UserId;
                user.de_department_id = DepartmentId;
                user.de_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _designationRepo.DesignationMaster(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid? UserId, Guid de_id)
        {
            DesignationMaster user = new DesignationMaster();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = UserId;
            user.de_id = de_id;
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _designationRepo.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] DesignationMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.de_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _designationRepo.DesignationMaster(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] DesignationMaster user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _designationRepo.DesignationMaster(user);
            return productDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status, string DepartmentId)
        {
            try
            {
                DesignationMaster user = new DesignationMaster();
                user.UserId = UserId;
                user.de_isactive = status;
                user.de_department_id = DepartmentId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _designationRepo.DesignationMaster(user);
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
                ExportRepository exportRepository = new ExportRepository();
                var result = new Result
                {
                    Data = exportRepository.DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(Guid? UserId, string status, string DepartmentId)
        {
            try
            {
                DesignationMaster user = new DesignationMaster();
                user.UserId = UserId;
                user.de_isactive = status;
                user.de_department_id = DepartmentId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _designationRepo.DesignationMaster(user);
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
                htmlContent += "        <h3 style='font-weight: bold;'>Designation Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Department Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Designation Code</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Designation Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Is Active</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? de_department_name = row["de_department_name"].ToString();
                    string? de_designation_code = row["de_designation_code"].ToString();
                    string? de_designation_name = row["de_designation_name"].ToString();
                    string? isActive = row["de_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + de_department_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + de_designation_code + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + de_designation_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + isActive + "</td>";
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
