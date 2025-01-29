using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using DataAccessLayer;
using Logger;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using static Core.Module.commonClass;

namespace BusinessLogic
{
    public class BLogin
    {

        //Test Pournima
        private readonly IConfigManager _configuration;
        DbLogin dbLogin; DbLogger dbLogger;
        CommonIUD commonIUD; CommonList objList;

        public BLogin(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogin = new DbLogin(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }

        public async Task<CommonList> GetLogin(Users obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.Password = SecurityEncy.PwdEncryptDecrypt(SecurityEncy.Decrypt(obj.Password),"E");
                var t1 = Task.Run(() => dbLogin.Get_UserLogin(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["IS_USER_LOCKED"].ToString() == "True")
                    {
                        objList.Message = "UserAccountLocked";
                    }
                    else
                    {
                        objList.Message = "";
                    }
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Data = dt;
                    objList.Count = dt.Rows.Count;
                    objList.AdditionalParameter = "";
                }
                
                return objList;
            }
            catch(Exception ex)
            {
                dbLogger.PostErrorLog("BLogin", ex.Message.ToString(), "GetLogin", 10001, obj.Username, true);
                return objList;
            }
        }

        public async Task<CommonList> GetResetPassword(UserResetPwd obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.Password = SecurityEncy.PwdEncryptDecrypt(SecurityEncy.Decrypt(obj.Password), "E");
                obj.NewPassword = SecurityEncy.PwdEncryptDecrypt(SecurityEncy.Decrypt(obj.NewPassword), "E");
                var t1 = Task.Run(() => dbLogin.Get_ResetPassword(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.Data = dt;
                    objList.Message = "Success";
                }
                else
                {
                    objList.Message = "Incorrect Password";
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GetResetPassword", 10001, obj.Username, true);
                return objList;
            }
        }

        public async Task<CommonList> CheckUserLogin(Users obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbLogin.Check_UserLogin(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["IS_PWD_CHANGE"].ToString() == "False")
                    {
                        objList.FinalMode = DBReturnGridRecord.RecordFound;
                        objList.Data = dt;
                        objList.Count = dt.Rows.Count;
                        objList.Message = "FirstTimeUserLogin";
                        objList.AdditionalParameter = "";
                        return objList;
                    }
                    else if(dt.Rows[0]["IS_PWD_EXPIRED"].ToString() == "True")
                    {
                        objList.FinalMode = DBReturnGridRecord.RecordFound;
                        objList.Data = dt;
                        objList.Count = dt.Rows.Count;
                        objList.Message = "Password Expired";
                        objList.AdditionalParameter = "";
                        return objList;
                    }
                }
                else
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    objList.Message = "";
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "CheckUserLogin", 10001, obj.Username, true);
                return objList;
            }
        }

        /*       public async Task<CommonList> UpdateAccountLockedStatus(Users obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbLogin.Update_Account_Locked_Status(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                
                objList.Message = "UserAccountLocked";
                objList.AdditionalParameter = "";
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "UpdateAccountLockedStatus", 10001, obj.Username, true);
                return objList;
            }
        }*/

        public async Task<CommonList> GetUserActivityLog(UserActivity obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbLogin.Get_UserActivityLog(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Data = dt;
                    objList.AdditionalParameter = "";
                    return objList;
                }
                else
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    return objList;
                }
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GetUserActivityLog", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString( obj.UpdatedBy)) : Convert.ToString( obj.CreatedBy), true);
                return objList;
            }
        }

        //public async Task<CommonList> GetUserActivityLog(UserActivity obj)
        //{
        //    objList = new CommonList();
        //    DataTable dt;
        //    try
        //    {
        //        var t1 = Task.Run(() => dbLogin.Get_UserActivityLog(obj).Tables[0]);
        //        await Task.WhenAll(t1);
        //        dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            objList.FinalMode = DBReturnGridRecord.RecordFound;
        //            objList.Data = dt;
        //        }
        //        else
        //        {
        //            objList.FinalMode = DBReturnGridRecord.RecordNotFound;
        //        }
        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        dbLogger.PostErrorLog("BLogin", ex.Message.ToString(), "GetUserActivityLog", 10001, "Admin", true);
        //        return objList;
        //    }

        //}

        public async Task<CommonList> GenerateUserOTP(UserOTP obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var Recid = SecurityEncy.GenerateOTP();
                obj.Userotp = int.Parse(Recid);
                obj.Type = "user";
                if (Recid != null && Recid != "")
                {
                    var t1 = Task.Run(() => dbLogin.PostGenerateUserOTP(obj).Tables[0]);
                    await Task.WhenAll(t1);
                    dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string USER_EMAIL_ID = dt.Rows[0]["USER_EMAIL_ID"].ToString();
                        string OTP = dt.Rows[0]["OTP"].ToString();
                        Mailler mailler = new Mailler(this._configuration);
                        var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/BusinessPartnerOtpAuth.txt");
                        string MaillerBody = System.IO.File.ReadAllText(fPath);
                        MaillerBody = MaillerBody.Replace("@Otp@", OTP);
                        mailler.SendMail(USER_EMAIL_ID, "", 10001, "User verification OTP", MaillerBody);
                        dbLogger.PostEmailLog("BOnboard", MaillerBody, "PostOnboard", 10001, USER_EMAIL_ID, "User OTP", true);
                        objList.Message = "Success";
                        objList.FinalMode = DBReturnGridRecord.RecordFound;
                        objList.Data = dt;
                    }
                    else
                    {
                        objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BLogin", ex.Message.ToString(), "GenerateUserOTP", 10001, "Admin", true);
                return objList;
            }

        }

        public async Task<CommonList> VerifyUserOTP(UserOTP obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.Type = "user";
                var t1 = Task.Run(() => dbLogin.Verify_User_OTP(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.Message = "Success";
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Data = dt;
                    /*
                    int DB_OTP = int.Parse(dt.Rows[0]["OTP"].ToString());
                    DateTime DB_OTP_CREATED_DATE = DateTime.Parse(dt.Rows[0]["OTP_CREATED_DATE"].ToString());
                    if (obj.Userotp == DB_OTP)
                    {
                        TimeSpan timediff = DateTime.UtcNow - DB_OTP_CREATED_DATE;
                        if (timediff.TotalMinutes > 2)
                        {
                            objList.Message = "OTPExpired";
                        }
                        else
                        {
                            objList.Message = "Success";
                            objList.FinalMode = DBReturnGridRecord.RecordFound;
                            objList.Data = dt;
                        }
                    }
                    else
                    {
                        objList.Message = "OTPDoesNotMatch";
                    }
                    */
                }
                else
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BLogin", ex.Message.ToString(), "VerifyUserOTP", 10001, "Admin", true);
                return objList;
            }

        }

        public async Task<CommonList> UpdateUserPassword(UserOTP obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.NewPassword = SecurityEncy.PwdEncryptDecrypt(SecurityEncy.Decrypt(obj.NewPassword), "E");
                var t1 = Task.Run(() => dbLogin.Update_User_Password(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.Data = dt;
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Message = "Success";
                }
                else
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    objList.Message = "SetPassword";
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BLogin", ex.Message.ToString(), "UpdateUserPassword", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetUserInfo(UserInfo obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbLogin.GetdbGetUserInfo(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Data = dt;
                    objList.Count = dt.Rows.Count;
                    objList.Message = "";
                    objList.AdditionalParameter = "";
                    return objList;
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BLogin", ex.Message.ToString(), "GetUserInfo", 10001, "Admin", true);
                return objList;
            }
        }
    }
}

    

