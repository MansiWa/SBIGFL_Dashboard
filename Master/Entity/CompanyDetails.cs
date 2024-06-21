using Microsoft.SqlServer.Server;
using System.Diagnostics.Metrics;
using Tokens;

namespace Master.API.Entity
{
	public class CompanyDetails
	{
		public Guid? com_id { get; set; }
		public Guid? UserId { get; set; }
		public BaseModel? BaseModel { get; set; }
		public string? com_company_name { get; set; }
		public string? com_company_name2 { get; set; }
		public string? com_owner_name { get; set; }
		public string? com_address { get; set; }
		public string? com_contact { get; set; }
		public string? com_gst_no { get; set; }
		public string? com_staff_no { get; set; }
		public string? com_email { get; set; }
		public string? com_website { get; set; }
		public IFormFile? com_company_logo { get; set; }
		public byte[]? com_company_logoc { get; set; }
		public string? com_bank_name { get; set; }
		public string? com_branch { get; set; }
		public string? com_acc_no { get; set; }
		public string? com_ifsc { get; set; }
		public IFormFile? com_company_logo2 { get; set; }
		public byte[]? com_company_logo2c { get; set; }
		public string? com_note { get; set; }
		public string? com_otpno { get; set; }
		public string? CountryId { get; set; }
		public string? StateId { get; set; }
		public string? CityId { get; set; }
		public string? Currency_format { get; set; }
		public string? Server { get; set; }
        public string? Server_Key { get; set; }
        public string? Server_Value { get; set; }
        public string? com_code { get; set; }
        public DateTime? com_updateddate { get; set; }
        public DateTime? com_createddate { get; set; }
        public DateTime? ser_updateddate { get; set; }
        public DateTime? ser_createddate { get; set; }
        public string? sub_id { get; set; }
        public string? py_id { get; set; }
    }

}
