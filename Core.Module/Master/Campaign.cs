
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Module.Master
{
    public class CampaignClass : commonClass
    {
        public Nullable<Int32> CampaignId { get; set; }
        [Required(ErrorMessage = "Campaign Name is required")]
        public string CampaignName { get; set; }
        public string CampaignDesc { get; set; }
    }
    public class CampaignSearch : commonSearchClass
    {
        public string CampaignName { get; set; }
    }
}
