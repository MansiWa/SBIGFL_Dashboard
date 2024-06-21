using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintSoftWeb.Models;
using System.Text;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using System.Data;

namespace PrintSoftWeb.Controllers
{
    public class RoleMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RoleMasterController> _logger;
        private readonly IStringLocalizer<RoleMasterController> _localizer;

        public RoleMasterController(ILogger<RoleMasterController> logger, IStringLocalizer<RoleMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;

        }
        public IActionResult Index(string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }

                var roleDataList = new List<RoleMasterModel>(); ;
                string server = HttpContext.Session.GetString("Server_Value");
                string comid = HttpContext.Session.GetString("com_id");
                //Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                Guid? UserId = new Guid(comid);
                HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/RoleMaster/GetAll?UserId={UserId}&status={status}&server={server}").Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<RoleMasterModel>() };
                    var jsonresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    roleDataList = jsonresponse.data;
                    if (roleDataList != null)
                    {
                        return View(roleDataList);
                    }
                    else
                    {
                        var roleData = new List<RoleMasterModel>();
                        return View(roleData);
                    }
                }

                return View(roleDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult EditIndex()
        {
            try
            {
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string server = HttpContext.Session.GetString("Server_Value");
                string url = $"{_httpClient.BaseAddress}/RoleMaster/GetMenu?UserId={UserId}&server={server}";
                List<AccessPrivilegeModel> roleDataList = new List<AccessPrivilegeModel>();
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<AccessPrivilegeModel>() };
                    var responsemodel = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    roleDataList = responsemodel.data;
                    if (roleDataList != null)
                    {
                        return View(roleDataList);
                    }
                    else
                    {
                        var stateDataListse = new List<AccessPrivilegeModel>();
                        return View(stateDataListse);
                    }
                }
                return View(roleDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(RoleMasterModel model)
        {
            try
            {
                //Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                string comid = HttpContext.Session.GetString("com_id");
                Guid? UserId = new Guid(comid);
                model.UserId = UserId;
                model.r_com_id = UserId;
                model.r_updateddate = DateTime.Now;
                model.r_createddate = DateTime.Now;
                model.Server_Value = serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/RoleMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Ok(outcomeDetail);
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid? r_id, string r_rolename, string r_description, string r_module, string r_isactive)
        {
            Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
            string server = HttpContext.Session.GetString("Server_Value");
            //RoleMasterModel model = new RoleMasterModel();
            try
            {
                List<AccessPrivilegeModel> roleDataList = new List<AccessPrivilegeModel>();
                List<AccessPrivilegeModel> model = new List<AccessPrivilegeModel>();
                string url = $"{_httpClient.BaseAddress}/RoleMaster/GetRoleById?r_id={r_id}&UserId={UserId}&server={server}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = new { data = new List<AccessPrivilegeModel>() };// JsonConvert.DeserializeObject<Role>(data);
                    var res = JsonConvert.DeserializeAnonymousType(data, rootObject);
                    model = res.data;

                    string urlre = $"{_httpClient.BaseAddress}/RoleMaster/GetMenu?UserId={UserId}&server={server}";
                    HttpResponseMessage responsere = _httpClient.GetAsync(urlre).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic datare = responsere.Content.ReadAsStringAsync().Result;
                        var dataObject = new { data = new List<AccessPrivilegeModel>() };
                        var responsemodel = JsonConvert.DeserializeAnonymousType(datare, dataObject);
                        roleDataList = responsemodel.data;
                        if (roleDataList != null)
                        {
                            foreach (var roleData in roleDataList)
                            {
                                var matchingRole = model.FirstOrDefault(r => string.Equals(r.a_menuid, roleData.a_menuid, StringComparison.OrdinalIgnoreCase));
                                if (matchingRole != null)
                                {
                                    roleData.a_addaccess = matchingRole.a_addaccess;
                                    roleData.a_editaccess = matchingRole.a_editaccess;
                                    roleData.a_viewaccess = matchingRole.a_viewaccess;
                                    roleData.a_deleteaccess = matchingRole.a_deleteaccess;
                                    roleData.a_workflow = matchingRole.a_workflow;

                                }
                            }
                            ViewData["r_id"] = r_id;
                            ViewData["r_rolename"] = r_rolename;
                            ViewData["r_description"] = r_description;
                            ViewData["r_module"] = r_module;
                            ViewData["IsReadOnly"] = true;
                            ViewData["r_isactive"] = r_isactive;
                            return View("EditIndex", roleDataList);
                        }
                        else
                        {
                            var stateDataListse = new List<AccessPrivilegeModel>();
                            return View("EditIndex", stateDataListse);
                        }
                    }
                    return View("EditIndex", roleDataList);
                }
                return View("EditIndex", roleDataList);
                //return View("EditIndex");
                //return Ok(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        public IActionResult Delete(Guid? r_id)
        {
            try
            {
                RoleMasterModel model = new RoleMasterModel();
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.r_id = r_id;
                string serverValue = HttpContext.Session.GetString("Server_Value");
                model.Server_Value = serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/RoleMaster/DeleteRole", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return RedirectToAction("Index");
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Excel(Guid? UserId, string status)
        {
            try
            {
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/RoleMaster/GetExcel?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Role Master");
                            var currentRow = 1;

                            worksheet.Cell(currentRow, 1).Value = "Role Name";
                            worksheet.Cell(currentRow, 2).Value = "Description";
                            worksheet.Cell(currentRow, 3).Value = "Module";
                            worksheet.Cell(currentRow, 4).Value = "Menu Name";
                            worksheet.Cell(currentRow, 5).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/RoleMaster?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["r_rolename"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["r_description"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["r_module"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["m_menuname"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["r_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Role Master.xlsx");
                            }
                        }

                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/RoleMaster?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                RoleMasterModel model = new RoleMasterModel();
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/RoleMaster/GetPdf?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        string htmlContent = content.ToString();
                        if (htmlContent.Contains("<td"))
                        {
                            MemoryStream ms = new MemoryStream();
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                            pdfDoc.Open();
                            TextReader sr = new StringReader(htmlContent);
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            pdfDoc.Close();
                            byte[] pdfBytes = ms.ToArray();
                            string base64Pdf = Convert.ToBase64String(pdfBytes);
                            ms1 = ms;
                            using (var stream = new MemoryStream())
                            {
                                return File(pdfBytes, "application/pdf", "RoleMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "Failed to retrieve data from the API.";
                            return Redirect($"/RoleMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/RoleMaster?status={status}");
                    }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return new FileStreamResult(ms1, "application/pdf");
        }
    }
}
