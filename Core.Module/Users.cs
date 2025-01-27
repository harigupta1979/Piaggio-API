using System.ComponentModel.DataAnnotations;

namespace Core.Module
{
    public class Users
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public int AccountLockedStatus { get; set; }
    }
    public class LogInLog
    {
        public int USER_ID { get; set; }
        public int DEALER_ID { get; set; }
    }

    public class UserResetPwd
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string NewPassword { get; set; }
        public int PasswordExpireDays { get; set; }
    }
   
    public class UserOTP
    {
        public string Username { get; set; }
        public int Userotp { get; set; }
        public string NewPassword { get; set; }
        public int PasswordExpireDays { get; set; }
        public string Type { get; set; }
        public int Otpid { get; set; }

    }
    public class UserInfo
    {
        public int UserId { get; set; }
       
    }
}
