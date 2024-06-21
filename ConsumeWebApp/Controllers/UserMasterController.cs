using Common;
using PrintSoftWeb.Models;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace PrintSoftWeb.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserMasterController> _logger;
        private readonly IStringLocalizer<UserMasterController> _localizer;
        public UserMasterController(ILogger<UserMasterController> logger, IStringLocalizer<UserMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:User").Value);
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
                
                string comid = HttpContext.Session.GetString("com_id");
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                Guid? UserId = new Guid(comid);
                string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_RoleMaster&sValue=r_rolename&id=r_id&IsActiveColumn=r_isactive&Server_Value={serverValue}&sCoulmnName=r_com_id&sColumnValue={comid}";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
                ViewBag.um_rolename = rootObject;

                var userDataList12 = new List<UserMasterModel>(); ;



                //            string n = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId={comid}&sTableName=tbl_Staff&sValue=st_staff_name&id=st_id&IsActiveColumn=st_isactive&Server_Value={serverValue}&sCoulmnName=st_com_id&sColumnValue={comid}";
                //            HttpResponseMessage nView = _httpClient.GetAsync(n).Result;
                //            dynamic namedata = nView.Content.ReadAsStringAsync().Result;
                //            var namerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(namedata);
                //            var idrootobject= JsonConvert.DeserializeObject<List<FillDropdown>>(namedata);
                //ViewBag.um_staffname = namerootObject;

                string n = $"{_httpClient.BaseAddress}/UserMaster/GetStaff?UserId={UserId}&Server_Value={serverValue}";
                HttpResponseMessage nView = _httpClient.GetAsync(n).Result;
                dynamic namedata = nView.Content.ReadAsStringAsync().Result;
                var namerootObject = JsonConvert.DeserializeObject<List<UserMasterModel>>(namedata);
                var idrootobject = JsonConvert.DeserializeObject<List<UserMasterModel>>(namedata);
                ViewBag.um_staffname = namerootObject;

                var userDataList = new List<UserMasterModel>(); ;
              
                var UserMasterlList = new List<UserMasterModel>();
                string usermasterurl = $"{_httpClient.BaseAddress}/UserMaster/GetAll?UserId={UserId}&status={status}&Server_Value={serverValue}";
                HttpResponseMessage response = _httpClient.GetAsync(usermasterurl).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<UserMasterModel>() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    UserMasterlList = response2.data;

                    if (UserMasterlList != null)
                    {
                        return View(UserMasterlList);
                    }
                    else
                    {
                        var UserMasterlDataList = new List<UserMasterModel>();
                        return View(UserMasterlDataList);
                    }
                }
                return View(UserMasterlList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(UserMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string comid = HttpContext.Session.GetString("com_id");
                model.UserId = UserId;
                model.um_com_id = comid;
                string serverValue = HttpContext.Session.GetString("Server_Value");
                model.Server_Value = serverValue;
                model.um_createddate = DateTime.Now;
                model.um_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/UserMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
                    return Ok(outcomeDetail);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Username(UserMasterModel model)
        {
            try
            {
                string comid = HttpContext.Session.GetString("com_id");
                Guid? UserId = new Guid(comid);
                model.um_com_id = comid;
                string serverValue = HttpContext.Session.GetString("Server_Value");
                model.Server_Value = serverValue;
                model.um_createddate = DateTime.Now;
                model.um_updateddate = DateTime.Now;
                string? staffid = model.um_staffid;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = $"{_httpClient.BaseAddress}/Staff/Get?st_id={staffid}&UserId={UserId}&Server_Value={serverValue}&st_com_id={new Guid(comid)}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    //string stContact = json.data.st_contact;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    string stContact = rootObject.data.st_contact;
                    //ViewBag.um_user_name= stContact;
                    return Ok(stContact);
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid? um_id)
        {
            string comid = HttpContext.Session.GetString("com_id");
            UserMasterModel model = new UserMasterModel();
            Guid? UserId = new Guid(comid);
            model.um_com_id = comid;
            try
            {
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                string url = $"{_httpClient.BaseAddress}/UserMaster/Get?um_id={um_id}&UserId={UserId}&Server_Value={serverValue}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = JsonConvert.DeserializeObject<RootUserMaster>(data);
                    model = rootObject.data;
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        public IActionResult Delete(Guid? um_id)
        {
            try
            {
                string comid = HttpContext.Session.GetString("com_id");
                UserMasterModel model = new UserMasterModel();
                Guid? UserId = new Guid(comid);
                model.um_id = um_id;
                string serverValue = HttpContext.Session.GetString("Server_Value");
                model.Server_Value = serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/UserMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
                    return RedirectToAction("Index");
                }
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
                var userDataList = new List<UserMasterModel>();
                string comid = HttpContext.Session.GetString("com_id");
                UserMasterModel model = new UserMasterModel();
                 UserId = new Guid(comid);
                string serverValue = HttpContext.Session.GetString("Server_Value");
                model.Server_Value = serverValue;
                string url = $"{_httpClient.BaseAddress}/UserMaster/GetExcel?UserId={UserId}&status={status}&Server_Value={serverValue}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    // Extract the "data" property
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    // Create a memory stream from the byte array
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        // Load the data into an Excel package (using the EPPlus library)
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("User Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Sr.No";
                            worksheet.Cell(currentRow, 2).Value = "Staff Name";
                            worksheet.Cell(currentRow, 3).Value = "User Name";
                            //worksheet.Cell(currentRow, 4).Value = "Role";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["Message"] = "No data found to export!";
                                return Redirect($"/UserMaster?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = index .ToString();
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["um_staffname"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["um_user_name"].ToString();
                                    //worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["um_rolename"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["um_isactive"].ToString();
                                }

                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "User Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/UserMaster?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/UserMaster?status={status}");
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                UserMasterModel model = new UserMasterModel();
                string comid = HttpContext.Session.GetString("com_id");
                UserId = new Guid(comid);
                string serverValue = HttpContext.Session.GetString("Server_Value");
                model.Server_Value = serverValue;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/UserMaster/GetPdf?UserId={UserId}&status={status}&Server_Value={serverValue}";
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
                                return File(pdfBytes, "application/pdf", "UserMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/UserMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/UserMaster?status={status}");
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
