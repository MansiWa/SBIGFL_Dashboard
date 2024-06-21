using System.Data;
using Tokens;

namespace Master.Entity
{
    public class RoleMaster
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? r_id { get; set; }
        public string? r_rolename { get; set; }
        public string? r_description { get; set; }
        public string? r_module { get; set; }
        public string? r_isactive { get; set; }
        public DateTime? r_updateddate { get; set; }
        public DateTime? r_createddate { get; set; }
        public List<AccessPrivilege>? Privilage { get; set; }
        public DataTable? DataTable { get; set; }
        public Guid? a_id { get; set; }
        public DateTime? a_updateddate { get; set; }
        public DateTime? a_createddate { get; set; }
        public Guid? r_com_id { get; set; }
		public string? Server_Value { get; set; }
	}
}
