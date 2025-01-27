using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Module.UserManagement
{
    public class UserClass : commonClass
    {
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "*")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContactNo { get; set; }
        //[Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        //[Required(ErrorMessage = "*")]
        public string Password { get; set; }
        public string DecptPassword { get; set; }
        public Nullable<DateTime> DOB { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<Int32> PinCode { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        //[Required(ErrorMessage = "*")]
        public Nullable<Int32> CountryId { get; set; }
        [Required(ErrorMessage = "*")]
        //public Nullable<Int32> BusinessPartnerId { get; set; }
        //[Required(ErrorMessage = "*")]
        public Nullable<Int32> StateId { get; set; }
        //[Required(ErrorMessage = "*")]
        public Nullable<Int32> CityId { get; set; }
        public Nullable<Int32> UserId { get; set; }
        public Nullable<Int32> DesignationId { get; set; }
        public Nullable<Int32> RoleId { get; set; }
        //public string ShowroomId { get; set; }
        public Nullable<Int32> DealerId { get; set; }
        public string UserType { get; set; }
        
        public Nullable<Int32> ReportingPersonId { get;set;}
        public Nullable<Int32> PwdExpireDays { get; set; }
    }

    public class UserSearch : commonSearchClass
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        //public string BusinessPartnerName { get; set; }
        //public Nullable<Int64> BusinessPartnerId { get; set; }
      
    }
    public class UserProfileClass 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public string UserName { get; set; }
        public Nullable<DateTime> DOB { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public Nullable<Int32> CountryId { get; set; }
        public Nullable<Int32> BusinessPartnerId { get; set; }
        public Nullable<Int32> StateId { get; set; }
        public Nullable<Int32> CityId { get; set; }
        public Nullable<Int32> UserId { get; set; }
        public Nullable<Int32> DesignationId { get; set; }
        public bool IsUpdate { get; set; }
    }




}
