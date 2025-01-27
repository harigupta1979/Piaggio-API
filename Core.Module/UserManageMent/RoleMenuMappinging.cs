using System;

namespace Core.Module.UserManageMent
{
   public class RoleMenuMappinging : commonClass
    {
        public Nullable<Int32> AccessModuleId { get; set; }
        public Nullable<Int32> RoleMappingId { get; set; }
        public Int32 RoleId { get; set; }
        public string SubMenuId { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsView { get; set; }
    }
}
