using Tokens;

namespace Master.Entity
{
    public class DesignationMaster
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? de_id { get; set; }
        public string? de_department_id { get; set; }
        public string? de_department_name { get; set; }
        public string? de_designation_code { get; set; }
        public string? de_designation_name { get; set; }
        public string? de_isactive { get; set; }
        public DateTime? de_createddate { get; set; }
        public DateTime? de_updateddate { get; set; }
    }
}
