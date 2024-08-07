using Newtonsoft.Json;

namespace PrintSoftWeb.Models
{
	public class WebMenu
	{
		public List<Home> GetWebMenu(string? userid)
		{
			var homedatalist = new List<Home>();
			HttpClient _httpClient = new HttpClient();
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			_httpClient = new HttpClient(handler);
			HttpResponseMessage response = _httpClient.GetAsync($"https://localhost:44355/api/RoleMaster/GetWebMenu?UserId={new Guid(userid)}").Result;
			if (response.IsSuccessStatusCode)
			{
				dynamic data = response.Content.ReadAsStringAsync().Result;
				var dataObject = new { data = new List<Home>() };
				var responsemodel = JsonConvert.DeserializeAnonymousType(data, dataObject);
				homedatalist = responsemodel.data;
				return homedatalist;
			}
			return homedatalist;
		}
	}
}
