using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Module
{
    public class CustomerMaster : commonClass 
    {
        public Nullable<Int64> CustomerId { get; set; }
        public Nullable<Int64> BusinessPartnerId { get; set; }
        public Nullable<Int64> TitleId { get; set; }
        public Nullable<Int64> CustomerTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public Nullable<Int64> MobileNo { get; set; }
        public Nullable<Int64> AltMobileNo { get; set; }
        public Nullable<DateTime> Dob { get; set; }
        public string Address { get; set; }
        public Nullable<Int64> OccupationId { get; set; }
        public Nullable<Int64> LanguageId { get; set; }
        public Nullable<Int64> CountryId { get; set; }
        public Nullable<Int64> StateId { get; set; }
        public Nullable<Int64> CityId { get; set; }
        public Nullable<Int64> PinCode { get; set; }
        public string PanNo { get; set; }
        public string TanNo { get; set; }
        public string AadharNo { get; set; }
        public string GstNo { get; set; }
    }
    public class CustomerSearch : commonSearchClass
    {
        public Nullable<Int64> CustomerId { get; set; }
        public Nullable<Int64> BusinessPartnerId { get; set; }
    }
}
