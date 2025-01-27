using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Module.UserManagement
{
    public class RoleClass : commonClass
    {
        [Required(ErrorMessage = "*")]
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }

        public Nullable<Int64> RoleId { get; set; }
        
        public string SubMenuId { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsView { get; set; }

    }
    public class RoleSearch : commonSearchClass
    {
        public string RoleName { get; set; }
       
    }
    public class RoleMenu
    {
        public Nullable<Boolean> IsSuperAdmin { get; set; }      
        public Nullable<Int64> RoleId { get; set; }
        public string Mode { get; set; }
    }
}
