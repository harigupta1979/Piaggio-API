using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Module.Master
{
   public class CampaignUploadClass:commonClass
    {
        public Nullable<Int64> CampaignUploadId { get; set; }
        public string CampaignName { get; set; }
        public string TempletName { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        
    }
    public class CampaignImagesClass : commonClass
    {
        public Nullable<Int64> CampaignUploadId { get; set; }
        public string CampaignUploadNo { get; set; }
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
    }
    public class CampaignUploadSearch : commonSearchClass
    {
        public string CampaignName { get; set; }
       
       
    }
}
