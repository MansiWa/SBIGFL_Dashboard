using Tokens;

namespace Master.Entity
{
    public class UserMaster 
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? um_id { get; set; }
        public string? um_user_name { get; set; }
        public string? um_password { get; set; }    
        public string? um_isactive { get; set; }
        public string? um_roleid { get; set; }
        public string? um_rolename { get; set; }
        public DateTime? um_createddate { get; set; }
        public DateTime? um_updateddate { get; set; }
    }
}
