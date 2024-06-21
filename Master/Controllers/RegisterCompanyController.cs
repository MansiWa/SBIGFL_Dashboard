using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class RegisterCompanyController : ControllerBase
	{
		private readonly ICompanyDetailsRepository _CRepo;
        public RegisterCompanyController(ICompanyDetailsRepository CRepo)
        {
            _CRepo = CRepo;
        }
        [HttpPost]
		public async Task<IActionResult> InsertCompany(IFormFile? com_company_logo, IFormFile? com_company_logo2)
		{
			CompanyDetails user = new CompanyDetails();

			if (user.BaseModel == null)
			{
				user.BaseModel = new BaseModel();
			}
			user.BaseModel.OperationType = "InsertCompanyDetails";
			try
			{
				string? com_id = HttpContext.Request.Form["com_id"];
				string? com_company_name = HttpContext.Request.Form["com_company_name"];
				string? com_company_name2 = HttpContext.Request.Form["com_company_name2"];
				string? com_owner_name = HttpContext.Request.Form["com_owner_name"];
				string? com_address = HttpContext.Request.Form["com_address"];
				string? com_contact = HttpContext.Request.Form["com_contact"];
				string? com_gst_no = HttpContext.Request.Form["com_gst_no"];
				string? com_email = HttpContext.Request.Form["com_email"];
				string? com_website = HttpContext.Request.Form["com_website"];
				string? com_bank_name = HttpContext.Request.Form["com_bank_name"];
				string? com_branch = HttpContext.Request.Form["com_branch"];
				string? com_acc_no = HttpContext.Request.Form["com_acc_no"];
				string? com_ifsc = HttpContext.Request.Form["com_ifsc"];
				string? com_note = HttpContext.Request.Form["com_note"];
				string? com_otpno = HttpContext.Request.Form["com_otpno"];
				string? Country = HttpContext.Request.Form["Country"];
				string? State = HttpContext.Request.Form["State"];
				string? City = HttpContext.Request.Form["City"];
				string? Currency_format = HttpContext.Request.Form["Currency_format"];
				//string? Server = HttpContext.Request.Form["Server"];




				if (com_company_logo != null)
				{
					if (com_company_logo.Length > 0)
					{
						string[] AllowedFileExtensions = new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".PNG", ".JPG", ".GIF", ".JPEG", ".BMP" };

						if (!AllowedFileExtensions.Contains(com_company_logo.FileName.Substring(com_company_logo.FileName.LastIndexOf('.'))))
						{
							ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
						}
						else
						{
							string[] Filecontent = com_company_logo.FileName.Split('.');
							string ImageName = Filecontent[0];
							string ImageExt = Filecontent[1];
							using (var ms = new MemoryStream())
							{
								MemoryStream memoryStream = new MemoryStream();
                                com_company_logo.OpenReadStream().CopyTo(ms);
								var fileBytes = ms.ToArray();
								user.com_company_logoc = fileBytes;
							}

						}

					}
				}

				if (com_company_logo2 != null)
				{
					if (com_company_logo2.Length > 0)
					{
						string[] AllowedFileExtensions = new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".PNG", ".JPG", ".GIF", ".JPEG", ".BMP" };

						if (!AllowedFileExtensions.Contains(com_company_logo2.FileName.Substring(com_company_logo2.FileName.LastIndexOf('.'))))
						{
							ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
						}
						else
						{
							string[] Filecontent = com_company_logo2.FileName.Split('.');
							string ImageName = Filecontent[0];
							string ImageExt = Filecontent[1];
							using (var ms = new MemoryStream())
							{
								MemoryStream memoryStream = new MemoryStream();
                                com_company_logo2.OpenReadStream().CopyTo(ms);
								var fileBytes = ms.ToArray();
								user.com_company_logo2c = fileBytes;
							}

						}

					}
				}



				user.com_company_name = com_company_name;
				user.com_company_name2 = com_company_name2;
				user.com_owner_name = com_owner_name;
				user.com_address = com_address;
				user.com_contact = com_contact;
				user.com_gst_no = com_gst_no;
				user.com_email = com_email;
				user.com_website = com_website;
				user.com_bank_name = com_bank_name;
				user.com_branch = com_branch;
				user.com_acc_no = com_acc_no;
				user.com_ifsc = com_ifsc;
				user.com_note = com_note;
				user.com_otpno = com_otpno;
				user.CountryId = Country;
				user.StateId = State;
				user.CityId = City;
				user.Currency_format = Currency_format;
				//user.Server = Server;




				var parameter = await _CRepo.CompanyDetails(user);
				return parameter;
			}
			catch (Exception)
			{

				throw;
			}
		}
        [HttpPost("InsertCompany")]
        public async Task<IActionResult> InsertCompany([FromBody] CompanyDetails com)
        {
            try
            {
                if (com.BaseModel == null)
                {
                    com.BaseModel = new BaseModel();
                }
                com.BaseModel.OperationType = "InsertCompany";
                var parameter = await _CRepo.CompanyDetails(com);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }

        }

    
    }
}
