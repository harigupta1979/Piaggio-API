using AppConfig;
using Core.Module;
using DataAccessLayer.DataAccess;
using System.Data;
using System;
using System.Data.SqlClient;
using Logger;
namespace DataAccessLayer
{
    public class DbLogin
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public DbLogin(IConfigManager configuration)
        {
            this._configuration = configuration;
        }

        public DataSet Get_UserLogin(Users _obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@USERNAME", _obj.Username));
                DB.Parameters.Add(new SqlParameter("@PASSWORD", _obj.Password));
                return DB.ExecuteDataSet("USP_USER_AUTHENTICATION");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Get_UserLogin", 10001, "Admin", true);
                return null;
            }
        }

        public DataSet Get_UserLogin_TimeDtl(LogInLog _obj)
        {

            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@USER_ID", _obj.USER_ID));
                DB.Parameters.Add(new SqlParameter("@DEALER_ID", _obj.DEALER_ID));
                return DB.ExecuteDataSet("USP_LOGIN_TIME_DETAILS");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Get_UserLogin_TimeDtl", 10001, "Admin", true);
                return null;
            }
        }

        public DataTable Get_ResetPassword(UserResetPwd _obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@USERNAME", _obj.Username));
                DB.Parameters.Add(new SqlParameter("@EXISTINGPASSWORD", _obj.Password));
                DB.Parameters.Add(new SqlParameter("@NEWPASSWORD", _obj.NewPassword));
                DB.Parameters.Add(new SqlParameter("@PASSWORDEXPIREDAYS", _obj.PasswordExpireDays));
                return DB.ExecuteDataTable("USP_USER_RESETPASSWORD");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Get_ResetPassword", 10001, "Admin", true);
                return null;
            }
        }

        public DataSet Check_UserLogin(Users obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@USERNAME", obj.Username));
                return DB.ExecuteDataSet("USP_CHECK_USERLOGIN");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Check_UserLogin", 10001, "Admin", true);
                return null;
            }
        }

        /*  public DataSet Update_Account_Locked_Status(Users obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            DB.Parameters.Add(new SqlParameter("@USERNAME", obj.Username));
            DB.Parameters.Add(new SqlParameter("@STATUS", 1));
            DB.Parameters.Add(new SqlParameter("@Action", "UPDATE"));
            return DB.ExecuteDataSet("USP_USER_ACCLOCKOUT");
        }*/

        public DataSet Get_UserActivityLog(UserActivity obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@ActivityId", obj.ActivityId));
                DB.Parameters.Add(new SqlParameter("@UserId", obj.UserId));
                DB.Parameters.Add(new SqlParameter("@LoginBrowser", obj.LoginBrowser));
                DB.Parameters.Add(new SqlParameter("@LoginBrowserVersion", obj.LoginBrowserVersion));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteDataSet("usp_Insert_Update_User_Activity_Log");
            }
            catch (Exception ex)
            {

                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Get_UserActivityLog", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return null;
            }
        }

        public DataSet PostGenerateUserOTP(UserOTP obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Action", "insert"));
                DB.Parameters.Add(new SqlParameter("@UserName", obj.Username));
                DB.Parameters.Add(new SqlParameter("@OTP", obj.Userotp));
                DB.Parameters.Add(new SqlParameter("@Type", obj.Type));
                return DB.ExecuteDataSet("usp_Insert_USER_OTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "PostGenerateUserOTP", 10001, "Admin", true);
                return null;
            }
        }

        public DataSet Verify_User_OTP(UserOTP obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Action", "select"));
                DB.Parameters.Add(new SqlParameter("@UserName", obj.Username));
                DB.Parameters.Add(new SqlParameter("@OTP", obj.Userotp));
                DB.Parameters.Add(new SqlParameter("@Type", obj.Type));
                DB.Parameters.Add(new SqlParameter("@Otpid", obj.Otpid));
                return DB.ExecuteDataSet("usp_Insert_USER_OTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Verify_User_OTP", 10001, "Admin", true);
                return null;
            }
        }

        public DataSet Update_User_Password(UserOTP obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@UserName", obj.Username));
                DB.Parameters.Add(new SqlParameter("@NEWPASSWORD", obj.NewPassword));
                DB.Parameters.Add(new SqlParameter("@PASSWORDEXPIREDAYS", obj.PasswordExpireDays));
                return DB.ExecuteDataSet("USP_USER_SETPASSWORD");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "Update_User_Password", 10001, "Admin", true);
                return null;
            }
        }

        public DataTable GetdbGetUserInfo(UserInfo obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@UserId", obj.UserId));
                return DB.ExecuteDataTable("dbo.usp_GetUserInfo");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbLogin", ex.Message.ToString(), "GetdbGetUserInfo", 10001, "Admin", true);
                return null;
            }
        }
    }
}
