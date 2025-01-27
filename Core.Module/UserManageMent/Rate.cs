using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Module.UserManageMent
{
   public class RateClass : commonClass
    {
        public Nullable<Int32> RateId { get; set; }
        public Nullable<Int32> CountryId { get; set; }
        public Nullable<Int32> ZoneId { get; set; }
        public Nullable<Int32> StateId { get; set; }
        public Nullable<Int32> CityId { get; set; }
        public Nullable<decimal> VideoRate { get; set; }
        public Nullable<decimal> ImageRate { get; set; }
        public string ImageRateType { get; set; }
        public Nullable<Int32> VideoId { get; set; }
    }
    public class RateSearch : commonSearchClass
    {

        public Nullable<Int32> StateId { get; set; }
        public Nullable<Int32> CityId { get; set; }
    }
}
