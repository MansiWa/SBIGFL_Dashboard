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
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (status == null)
                {
                    status = "1";
                }

                var roleDataList = new List<RoleMasterModel>(); ;
                HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/RoleMaster/GetAll?UserId={userid}&status={status}").Result;
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
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                string url = $"{_httpClient.BaseAddress}/RoleMaster/GetMenu?UserId={userid}";
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
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Guid? UserId = new Guid(userid);
                model.UserId = UserId;
                model.r_updateddate = DateTime.Now;
                model.r_createddate = DateTime.Now;
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
            string? userid = Request.Cookies["com_id"];
            if (userid == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                List<AccessPrivilegeModel> roleDataList = new List<AccessPrivilegeModel>();
                List<AccessPrivilegeModel> model = new List<AccessPrivilegeModel>();
                string url = $"{_httpClient.BaseAddress}/RoleMaster/GetRoleById?r_id={r_id}&UserId={userid}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = new { data = new List<AccessPrivilegeModel>() };// JsonConvert.DeserializeObject<Role>(data);
                    var res = JsonConvert.DeserializeAnonymousType(data, rootObject);
                    model = res.data;

                    string urlre = $"{_httpClient.BaseAddress}/RoleMaster/GetMenu?UserId={userid}";
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
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.UserId = new Guid(userid);
                model.r_id = r_id;
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
    }
}
