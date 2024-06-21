using System.ComponentModel;

namespace PrintSoftWeb.Models
{
    public class DesignationMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? de_id { get; set; }
        public string? de_department_id { get; set; }
        [DisplayName("Department Name")]
        public string? de_department_name { get; set; }
        [DisplayName("Code")]
        public string? de_designation_code { get; set; }
        [DisplayName("Designation Name")]
        public string? de_designation_name { get; set; }
        [DisplayName("Status")]
        public string? de_isactive { get; set; }
        public DateTime? de_createddate { get; set; }
        public DateTime? de_updateddate { get; set; }
    }
}
