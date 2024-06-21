using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Net.Http.Headers;

namespace PrintSoftWeb.Controllers
{
    public class UploadReportController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UploadReportController> _logger;
        private readonly IStringLocalizer<UploadReportController> _localizer;
        private readonly string baseaddresuser;
        public UploadReportController(ILogger<UploadReportController> logger, IStringLocalizer<UploadReportController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            baseaddresuser = new Uri(configuration.GetSection("Server:User").Value).ToString();
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(DateTime? r_date)
        {
            try
            {
                if (r_date == null)
                {
                    r_date = DateTime.Now;
                }
                var FileUploadDataList = new List<FileUpload>(); ;
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<Report> FileUploadlist = new List<Report>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadReport/GetAll?user_id=" + userid + "&f_date=" + r_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<Report>() };
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
                        var FileUploadDataList2 = new List<Report>();
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
        public IActionResult Insert([FromForm] Report model)
        {
            try
            {
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.UserId = userid;
                model.r_date = DateTime.Now;
                model.r_updateddate = DateTime.Now;
                model.r_createddate = DateTime.Now;

                var content = new MultipartFormDataContent();
                content.Add(new StringContent(model.UserId?.ToString() ?? ""), "UserId");
                content.Add(new StringContent(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")), "r_updateddate");
                content.Add(new StringContent(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")), "r_createddate");
                content.Add(new StringContent(model.r_name ?? ""), "r_name");
                content.Add(new StringContent(model.r_type ?? ""), "r_type");
                content.Add(new StringContent(model.r_remark ?? ""), "r_remark");
                
                content.Add(new StringContent(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")), "r_date");

                StreamContent? combo_filesContent = null;

                if (model.r_file != null)
                {
                    combo_filesContent = new StreamContent(model.r_file.OpenReadStream());
                    combo_filesContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"r_file\"",
                        FileName = $"\"{model.r_file.FileName}\""
                    };
                    combo_filesContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Add(combo_filesContent);
                }
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/UploadReport/Insert", content).Result;
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

    }
}
