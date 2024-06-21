
namespace PrintSoftWeb.Models
{
	public class CompanyDetailsModel
	{
		public Guid? com_id { get; set; }
		public Guid? UserId { get; set; }
		public string? com_company_name { get; set; }
		public string? com_company_name2 { get; set; }
		public string? com_owner_name { get; set; }
		public string? com_address { get; set; }
		public string? com_contact { get; set; }
		public string? com_gst_no { get; set; }
		public string? com_email { get; set; }
		public string? com_website { get; set; }
		public IFormFile? com_company_logo { get; set; }
		public string? com_bank_name { get; set; }
		public string? com_branch { get; set; }
		public string? com_acc_no { get; set; }
		public string? com_ifsc { get; set; }
		public IFormFile? com_company_logo2 { get; set; }
		public string? com_note { get; set; }
		public string? com_otpno { get; set; }
		public string? CountryId { get; set; }
		public string? StateId { get; set; }
		public string? CityId { get; set; }
		public string? Currency_format { get; set; }
		public string? Server_Value { get; set; }
        public DateTime? com_updateddate { get; set; }
        public DateTime? com_createddate { get; set; }
        public string? type { get; set; }
        public string? ImageBase64 { get; set; }
        public string? ImageBase642 { get; set; }
    }

}
