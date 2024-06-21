using PrintSoftWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Localization;

namespace PrintSoftWeb.Controllers
{
    public class CompanyRegistrationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompanyRegistrationController> _logger;
        private readonly IStringLocalizer<CompanyRegistrationController> _localizer;
        private readonly string baseaddresuser;
        public CompanyRegistrationController(ILogger<CompanyRegistrationController> logger, IStringLocalizer<CompanyRegistrationController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            baseaddresuser = new Uri(configuration.GetSection("Server:User").Value).ToString();
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm] CompanyRegistrationModel model)
        {
            try
            {
                model.com_updateddate = DateTime.Now;
                model.com_createddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/RegisterCompany/InsertCompany", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
                    dynamic comid = rootObject.data;
                    string id = comid.OutcomeId;
                    string name = comid.OutcomeDetail;
                    var result = new { id, name, outcomeDetail };
                    TempData["successMessage"] = outcomeDetail; 
                    
                    return Ok(result);                                  
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
