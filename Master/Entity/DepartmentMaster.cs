using Tokens;

namespace Master.API.Entity
{
    public class DepartmentMaster
    { 
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? d_id { get; set; }
        public string? d_department_name { get; set; }
        public string? d_department_code { get; set; }
        public string? d_isactive { get; set; }
        public DateTime? d_createddate { get; set; }
        public DateTime? d_updateddate { get; set; }
    }
}
