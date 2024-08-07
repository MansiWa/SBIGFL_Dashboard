using System.ComponentModel;

namespace PrintSoftWeb.Models
{
    public class RoleMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? r_id { get; set; }
        [DisplayName("Role Name")]
        public string? r_rolename { get; set; }
        [DisplayName("Description")]
        public string? r_description { get; set; }
        [DisplayName("Module")]
        public string? r_module { get; set; }
        [DisplayName("Menu Name")]
        public string? m_menuname { get; set; }

        [DisplayName("Status")]
        public string? r_isactive { get; set; }
        public DateTime? r_updateddate { get; set; }
        public DateTime? r_createddate { get; set; }
        public List<AccessPrivilegeModel>? Privilage { get; set; }
        public string? a_id { get; set; }
        public DateTime? a_updateddate { get; set; }
        public DateTime? a_createddate { get; set; }
	}
   
}
