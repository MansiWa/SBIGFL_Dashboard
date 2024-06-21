using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;

namespace PrintSoftWeb.Controllers
{
	public class AssetsDashController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<AssetsDashController> _logger;
		private readonly IStringLocalizer<AssetsDashController> _localizer;
		private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
		private IConfiguration Configuration;
		public AssetsDashController(ILogger<AssetsDashController> logger, IStringLocalizer<AssetsDashController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
		public IActionResult Index(string? date)
		{
			try
			{
				var FileUploadDataList = new List<AssetsLiabilities>();
				string? userid = Request.Cookies["com_id"];
				if (userid == null)
				{
					return RedirectToAction("Index", "Login");
				}
				HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/AssetsLiabilities/GetAssests?user_id=" + new Guid(userid) + "&al_date=" + date).Result;
				if (response.IsSuccessStatusCode)
				{
					dynamic data = response.Content.ReadAsStringAsync().Result;
					var dataObject = new { data = new List<AssetsLiabilities>() };
					var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
					FileUploadDataList = responses.data;
					return View(FileUploadDataList);

				}
				return View(FileUploadDataList);
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return RedirectToAction("ErrorIndex", "Home");
			}
		}
	}
}
