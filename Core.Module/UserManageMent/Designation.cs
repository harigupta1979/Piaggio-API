using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Module.UserManagement
{
    public class DesignationClass : commonClass
    {
        [Required(ErrorMessage = "*")]
        public string DesignationName { get; set; }
        public string DesignationDesc { get; set; }
        public Nullable<Int32> DesignationId { get; set; }
    }
    public class DesignationSearch : commonSearchClass
    {
        public string DesignationName { get; set; }
    }
}
