using System.ComponentModel;
using Tokens;

namespace PrintSoftWeb.Models
{
    public class UserMasterModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? um_id { get; set; }
        [DisplayName("User Name")]
        public string? um_user_name { get; set; }
        [DisplayName("Password")]
        public string? um_password { get; set; }
        [DisplayName("Roleid")]
        public string? um_roleid { get; set; }
        [DisplayName("Role Name")]
        public string? um_rolename { get; set; }

        [DisplayName("Status")]
        public string? um_isactive { get; set; }
        public DateTime? um_createddate { get; set; }
        public DateTime? um_updateddate { get; set; }
	}
    public class RootUserMaster
    {
        public UserMasterModel? data { get; set; }
    }
}
