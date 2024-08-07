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

				string? userid = Request.Cookies["com_id"];
				if (userid == null)
				{
					return RedirectToAction("Index", "Login");
				}
				Guid? UserId = new Guid(userid);
                string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_RoleMaster&sValue=r_rolename&id=r_id&IsActiveColumn=r_isactive&sCoulmnName=r_com_id&sColumnValue={userid}";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
                ViewBag.um_rolename = rootObject;

                var userDataList12 = new List<UserMasterModel>(); ;


                var userDataList = new List<UserMasterModel>(); ;
              
                var UserMasterlList = new List<UserMasterModel>();
                string usermasterurl = $"{_httpClient.BaseAddress}/UserMaster/GetAll?UserId={UserId}&status={status}";
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
				string? userid = Request.Cookies["com_id"];
				if (userid == null)
				{
					return RedirectToAction("Index", "Login");
				}
				model.UserId = new Guid(userid);
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

        [HttpGet]
        public IActionResult Edit(Guid? um_id)
        {
			string? userid = Request.Cookies["com_id"];
			if (userid == null)
			{
				return RedirectToAction("Index", "Login");
			}
			UserMasterModel model = new UserMasterModel();
            Guid? UserId = new Guid(userid);
            try
            {
                string url = $"{_httpClient.BaseAddress}/UserMaster/Get?um_id={um_id}&UserId={UserId}";
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
				string? userid = Request.Cookies["com_id"];
				if (userid == null)
				{
					return RedirectToAction("Index", "Login");
				}
				UserMasterModel model = new UserMasterModel();
                Guid? UserId = new Guid(userid);
                model.um_id = um_id;
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

    }
}
