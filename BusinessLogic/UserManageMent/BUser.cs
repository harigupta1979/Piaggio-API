using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.UserManagement;
using DataAccessLayer;
using DataAccessLayer.UserManagement;
using Logger;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLogic.UserManagement
{
    public class BUser
    {
        private readonly IConfigManager _configuration;
        dbUser dbUser; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objList;

        public BUser(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbUser = new dbUser(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostUser(UserClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
              
                if (obj.UserId != null && obj.UserId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.UserId = 0; }
                var t1 = Task.Run(() => dbUser.PostdbUser(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "User Name Already Exists !";
                }
                else if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.UserId != null && obj.UserId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update")
                    {
                        commonIUD.Message = "Data Updated Successfully!";
                    }
                    else
                    {
                        commonIUD.Message = "Data Inserted Successfully!";
                        obj.UserId = 0;

                        Mailler mailler = new Mailler(this._configuration);
                        var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderLogin.txt");
                        string MaillerBody = System.IO.File.ReadAllText(fPath);
                        MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", obj.FirstName + " " + obj.LastName);
                        MaillerBody = MaillerBody.Replace("@UserName@", obj.UserName);
                        MaillerBody = MaillerBody.Replace("@Password@", obj.DecptPassword);
                        MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "resetpassword?u=" + obj.UserName);
                        mailler.SendMail(obj.EmailAddress, "", 10001, "Reset Password", MaillerBody);
                    }
                    commonIUD.AdditionalParameter = "";

                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                }
                return commonIUD;
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BUser", ex.Message.ToString(), "PostUser", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GetUser(UserSearch obj)
        {
            objList = new CommonList();
            DataTable dt;
            dbCommon = new dbCommon(this._configuration);
            try
            {
                QueryBuilder queryBuilder = new QueryBuilder();

                var t1 = Task.Run(() => queryBuilder.BuildQuerySearch(obj));
                await Task.WhenAll(t1);
                string WhereCond = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                var t2 = Task.Run(() => dbCommon.DynamicQuery("user", WhereCond,obj.LoginUserId));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        objList.FinalMode = DBReturnGridRecord.RecordFound;
                        objList.Data = dt;
                        objList.Count = dt.Rows.Count;
                        objList.Message = "";
                        objList.AdditionalParameter = "";
                        return objList;
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BUser", ex.Message.ToString(), "GetUser", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetUserProfileDetails(UserProfileClass obj)
        {
            objList = new CommonList();
            DataTable dt;
            dbCommon = new dbCommon(this._configuration);
            try
            {
                var t1 = Task.Run(() => dbUser.Get_UserProfileDetails(obj).Tables[0]);
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
                else
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BUser", ex.Message.ToString(), "GetUserProfileDetails", 10001, "Admin", true);
                return objList;
            }
        }

    }
}
