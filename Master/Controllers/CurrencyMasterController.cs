using Common;
using Master.API.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyMasterController : ControllerBase
    {
        public readonly ICurrencyMasterRepository _currencyRepo;
        public CurrencyMasterController(ICurrencyMasterRepository currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string status)
        {
            try
            {
                CurrencyMaster user = new CurrencyMaster();
                user.UserId = UserId;
                user.cm_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _currencyRepo.Currency(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid? UserId, Guid cm_id)
        {
            CurrencyMaster user = new CurrencyMaster();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = UserId;
            user.cm_id = cm_id;
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _currencyRepo.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CurrencyMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.cm_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _currencyRepo.Currency(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] CurrencyMaster user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _currencyRepo.Currency(user);
            return productDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status)
        {
            try
            {
                CurrencyMaster user = new CurrencyMaster();
                user.UserId = UserId;
                user.cm_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _currencyRepo.Currency(user);
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
        public async Task<IActionResult> GetPdf(Guid? UserId, string status)
        {
            try
            {
                CurrencyMaster user = new CurrencyMaster();
                user.UserId = UserId;
                user.cm_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _currencyRepo.Currency(user);
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
                htmlContent += "        <h3 style='font-weight: bold;'>Currency Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Sr.No</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Currency Code</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Currency Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Currency Symbol</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Currency Format</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Is Active</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                int a = 0;
                foreach (DataRow row in data.Rows)
                {
                    a++;
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + a+ "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["cm_currencycode"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["cm_currencyname"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["cm_currencysymbol"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["cm_currency_format"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["cm_isactive"].ToString() + "</td>";
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
