using System.Data;
using Tokens;

namespace Master.Entity
{
    public class Report
    {
        public string? UserId { get; set; }
        public BaseModel? BaseModel { get; set; }
        public string? r_id { get; set; }
        public string? r_type { get; set; }
        public string? r_name { get; set; }
        public string? r_remark { get; set; }
        public IFormFile? r_file { get; set; }
        public byte[]? r_filecopy { get; set; }
        public DataTable? DataTable { get; set; }

        public DateTime? r_date { get; set; }
        public DateTime? r_updateddate { get; set; }
        public DateTime? r_createddate { get; set; }
    }
}
