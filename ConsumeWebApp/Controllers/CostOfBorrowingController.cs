using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OfficeOpenXml;
using PrintSoftWeb.Models;
using System.Data.OleDb;
using System.Data;
using System.Text;

namespace PrintSoftWeb.Controllers
{
	public class CostOfBorrowingController : Controller
	{
        private readonly HttpClient _httpClient;
        private readonly ILogger<CostOfBorrowingController> _logger;
        private readonly IStringLocalizer<CostOfBorrowingController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public CostOfBorrowingController(ILogger<CostOfBorrowingController> logger, IStringLocalizer<CostOfBorrowingController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
            Environment = _environment;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                var FileUploadDataList = new List<CostOfBorrowingModel>();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CostOfBorrowing/GetAll?user_id=" + new Guid(userid)).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CostOfBorrowingModel>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    if (FileUploadDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        return View(FileUploadDataList);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var FileUploadDataList2 = new List<CostOfBorrowingModel>();
                        return View(FileUploadDataList2);
                    }
                }
                return View(FileUploadDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(CostOfBorrowingModel model)
        {
            try
            {
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.UserId = new Guid(userid);
                model.cob_createddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CostOfBorrowing", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
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
        public IActionResult Delete(Guid cob_id)
        {
            try
            {
                CostOfBorrowingModel model = new CostOfBorrowingModel();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.UserId = new Guid(userid);
                model.cob_id = cob_id;
                //model.cr_createddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CostOfBorrowing/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
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
