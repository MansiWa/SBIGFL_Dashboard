using System.ComponentModel;

namespace PrintSoftWeb.Models
{
    public class CompanyRegistrationModel
    {
        public Guid? com_id { get; set; }

        [DisplayName("Company Name")]
        public string? com_company_name { get; set; }
        [DisplayName("Email")]
        public string? com_email { get; set; }
        [DisplayName("Contact No")]
        public string? com_contact { get; set; }
        public string? CountryId { get; set; }
        public string? com_staff_no { get; set; }
        public DateTime? com_updateddate { get; set; }
        public DateTime? com_createddate { get; set; }
    }
}
