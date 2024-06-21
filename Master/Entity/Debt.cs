using Tokens;

namespace Master.Entity
{
    public class Debt
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? date { get; set; }
        public string? d_count { get; set; }
        public string? d_filename { get; set; }
        public string? d_filetype { get; set; }
        public string? d_updatedby { get; set; }
        public string? Product { get; set; }
        public string? NoOfAccount { get; set; }
        public string? Amount { get; set; }
    }
}
