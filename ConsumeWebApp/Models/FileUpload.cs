using Tokens;
using Twilio.Rest.Video.V1.Room.Participant;

namespace PrintSoftWeb.Models
{
    public class FileUpload
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? f_id { get; set; }
        public string? d_id { get; set; }
        public string? f_fileid { get; set; } 
        public string? f_docid { get; set; }
        public string? f_date { get; set; }
        public string? f_filename { get; set; }
        public string? f_count { get; set; }
        public string? f_updatedby { get; set; }
        public DateTime? f_updateddate { get; set; }
        public string? f_isactive { get; set; }
        public string? f_doctype { get;set; }
        public string? d_tablename { get;set; }
    }
}
