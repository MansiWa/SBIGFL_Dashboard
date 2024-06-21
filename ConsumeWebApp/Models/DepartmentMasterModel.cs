using System.ComponentModel;

namespace PrintSoftWeb.Models
{
    public class DepartmentMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? d_id { get; set; }
        [DisplayName("Name")]
        public string? d_department_name { get; set; }
        [DisplayName("Code")]
        public string? d_department_code { get; set; }
        [DisplayName("Status")]
        public string? d_isactive { get; set; }
        public DateTime? d_createddate { get; set; }
        public DateTime? d_updateddate { get; set; }
    }
    public class RootObjectDM
    {
        public DepartmentMasterModel? data { get; set; }
    }
}
