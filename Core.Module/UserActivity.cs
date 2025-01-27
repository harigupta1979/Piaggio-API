using System;

namespace Core.Module
{
   public class UserActivity : commonClass
    {
        public Nullable<Int32> ActivityId { get; set; }
        public Nullable<Int32> UserId { get; set; }
        public string LoginBrowser { get; set; }
        public string LoginBrowserVersion { get; set; }
    }
}
