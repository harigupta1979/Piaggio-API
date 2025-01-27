using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Core.Module.Upload
{
    public class UploadEmployeeData
    {
        public long Id { get; set; }
       // [Required(ErrorMessage = "Please select file to upload")]
       // public HttpPostedFileBase EmployeeData { get; set; }
        [Required(ErrorMessage = "Please select Trigger date")]
        public string TriggerDate { get; set; }
        public string hdTriggerDate { get; set; }
    }
    public class DownloadUpoadInfo
    {
        public long Valid { get; set; }
        public long Invalid { get; set; }
        public long Id { get; set; }
        public string Status { get; set; }
    }
    public class UploadFileData 
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? TriggerDate { get; set; }
        public string Status { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
    }
    public class UploadFileDataList 
    {
        public string Upload_Date { get; set; }
        public int Total_Member_Upload { get; set; }
        public int Valid_Member { get; set; }
        public int Invalid_Member { get; set; }
        public int Total_Data_Updated { get; set; }
        public int pending_Data { get; set; }
        public string Report_Date { get; set; }
        public int SrNo { get; set; }
    }
    public class DBMessage
    {
        public string Id { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public DBMessage()
        {
            this.StatusCode = -1;
            this.Message = "Please Contact Support";
        }
    }
    public class uploaddownload: commonClass
    {
        public int ParentId { get; set; }
        public string ChildId { get; set; }
        public int BusinessPartnerId { get; set; }
    }
}
