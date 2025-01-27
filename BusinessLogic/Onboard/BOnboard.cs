using AppConfig;
using BusinessLogic.Dealer;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.Onboard;
using DataAccessLayer;
using DataAccessLayer.Onboard;
using Logger;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Reporting.NETCore;
using System.Net;
using Core.Module.Dealer;
using System.Collections.Generic;

namespace BusinessLogic.Onboard
{
    public class BOnboard
    {

        private readonly IConfigManager _configuration;
        dbOnboard dbOnboard; DbLogger dbLogger; CommonListDataSet objListDataSet;
        CommonIUD commonIUD; CommonList objList; dbCommon dbCommon;

        public BOnboard(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbOnboard = new dbOnboard(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostOnboard(OnboardClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                if (obj.OnBoardId != null && obj.OnBoardId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.OnBoardId = 0; }
                var t1 = Task.Run(() => dbOnboard.PostdbOnboard(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
                else if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.OnBoardId != null && obj.OnBoardId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "insert" && Recid != null && Recid != 0)
                    {
                        try
                        {
                            //Mailler mailler = new Mailler(this._configuration);
                            //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVender.txt");
                            //string MaillerBody = System.IO.File.ReadAllText(fPath);
                            //MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", obj.OnBoardName);
                            //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "business?" + SecurityEncy.Encrypt("Onboard=" + Recid + "&Stage=Client"));
                            //mailler.SendMail(obj.EmailId, "", 10001, "Dealer onboarding-ICICI Lombard", MaillerBody);
                            Mailler mailler = new Mailler(this._configuration);
                            var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVender.html");
                            string MaillerBody = System.IO.File.ReadAllText(fPath);
                            MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", obj.OnBoardName);
                            MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "business?" + SecurityEncy.Encrypt("Onboard=" + Recid + "&Stage=Client"));
                            mailler.SendMail(obj.EmailId, "", 10001, "ICICI: Marketing Firm Onboarding - NO REPLY", MaillerBody);
                            dbLogger.PostEmailLog("BOnboard", MaillerBody, "PostOnboard", 10001, obj.EmailId,"Onboard", true);
                        }
                        catch (Exception ex)
                        {
                            dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "OnboardingMail", 10001, "Admin", true);
                            return commonIUD;
                        }
                    }
                    if (obj.Action == "update" && Recid != null && Recid != 0 && obj.OnBoardAction == "Approve")
                    {
                        try
                        {
                            obj.ApprovedBy = obj.CreatedBy;
                            //var t2 = Task.Run(() => dbOnboard.PostdbOnboardApproval(obj));
                            //await Task.WhenAll(t2);
                            //dynamic status = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                            //Mailler mailler = new Mailler(this._configuration);
                            //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderLogin.txt");
                            //string MaillerBody = System.IO.File.ReadAllText(fPath);
                            //MaillerBody = MaillerBody.Replace("@Name@", obj.OnBoardName);
                            //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl")+ "Auth/Login");
                            //MaillerBody = MaillerBody.Replace("@Password@", "12345");
                            //MaillerBody = MaillerBody.Replace("@UserName@", obj.EmailId);
                            //mailler.SendMail(obj.EmailId, "", 10001, "Dealer onboarding-ICICI Lombard Access Details", MaillerBody);

                            //COMMENT BY ANISH 
                            if (!Convert.ToBoolean(obj.ISAPPROVED))
                            {
                                // BDealer bDealer = new BDealer(this._configuration);
                                var t4 = Task.Run(() => SendDealerAgreement(Recid, "OnBoardId"));
                                await Task.WhenAll(t4);
                                dynamic argrrmentStatus = t4.Status == TaskStatus.RanToCompletion ? t4.Result : null;
                            }
                        }

                        catch (Exception ex)
                        {
                            dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "ApproveMail", 10001, "Admin", true);
                            return commonIUD;
                        }
                        //Mailler mailler = new Mailler(this._configuration);
                        //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderLogin.html");
                        //string MaillerBody = System.IO.File.ReadAllText(fPath);
                        //MaillerBody = MaillerBody.Replace("@Name@", obj.OnBoardName);
                        //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "Auth/Login");
                        //MaillerBody = MaillerBody.Replace("@Password@", "12345");
                        //MaillerBody = MaillerBody.Replace("@UserName@", obj.EmailId);
                        //mailler.SendMail(obj.EmailId, "", 10001, "ICICI: Marketing Firm Access Details", MaillerBody);
                    }
                    if (obj.Action == "update" && Recid != null && Recid != 0 && obj.OnBoardAction == "ReferBack")
                    {
                        try
                        {
                            obj.ApprovedBy = obj.CreatedBy;
                            //var t3 = Task.Run(() => dbOnboard.PostdbOnboardApproval(obj));
                            //await Task.WhenAll(t3);
                            //dynamic status = t3.Status == TaskStatus.RanToCompletion ? t3.Result : null;

                            //Mailler mailler = new Mailler(this._configuration);
                            //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderReject.txt");
                            //string MaillerBody = System.IO.File.ReadAllText(fPath);
                            //MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", obj.OnBoardName);
                            //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "business?" + SecurityEncy.Encrypt("Onboard=" + Recid + "&Stage=Client"));
                            //mailler.SendMail(obj.EmailId, "", 10001, "Dealer onboarding-ICICI Lombard", MaillerBody);
                            Mailler mailler = new Mailler(this._configuration);
                            var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderReject.html");
                            string MaillerBody = System.IO.File.ReadAllText(fPath);
                            MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", obj.OnBoardName);
                            MaillerBody = MaillerBody.Replace("@ApprovalRemarks@", obj.ApprovalRemarks);
                            MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "business?" + SecurityEncy.Encrypt("Onboard=" + Recid + "&Stage=Client"));
                            mailler.SendMail(obj.EmailId, "", 10001, "ICICI: Marketing Firm Refer Back Onboarding - NO REPLY", MaillerBody);
                            dbLogger.PostEmailLog("BOnboard", MaillerBody, "PostOnboard", 10001, obj.EmailId, "Onboard", true);
                        }
                        catch (Exception ex)
                        {
                            dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "ReferBackMail", 10001, "Admin", true);
                            return commonIUD;
                        }
                    }
                    if (obj.Action == "update" && obj.OnBoardAction == "Approve")
                    { commonIUD.Message = "Approve Successfully!"; }
                    else if (obj.Action == "update" && obj.OnBoardAction == "ReferBack")
                    { commonIUD.Message = "ReferBack Successfully!"; }
                    else if (obj.Action == "update" && obj.Action == "Client")
                    { commonIUD.Message = "Submit Successfully!"; }
                    else if (obj.Action == "insert")
                    { commonIUD.Message = "Data Inserted Successfully!"; obj.OnBoardId = 0; }
                    else
                    { commonIUD.Message = "Data Updated Successfully!"; }
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.OnBoardId != null && obj.OnBoardId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
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
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "PostOnboard", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GenerateOtp(OnboardOtp obj)
        {
            objList = new CommonList();
            commonIUD = new CommonIUD(); DataTable dt;
            var Recid = (dynamic)null; dbCommon = new dbCommon(this._configuration);
            try
            {
                QueryBuilder queryBuilder = new QueryBuilder();
                var t1 = Task.Run(() => queryBuilder.BuildQuerySearch(obj));
                await Task.WhenAll(t1);
                string WhereCond = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                var t2 = Task.Run(() => dbCommon.DynamicQuery("onboard", WhereCond));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Recid = SecurityEncy.GenerateOTP();
                        string emailid = Convert.ToString(dt.Rows[0]["EmailId"]);
                      //  commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                      //  commonIUD.AdditionalParameter = Recid;
                        if (Recid != null && Recid != "")
                        {
                            obj.Username = emailid;
                            obj.Userotp = int.Parse(Recid);
                            obj.Type = "client";
                            var t3 = Task.Run(() => dbOnboard.PostGenerateClientOTP(obj).Tables[0]);
                            await Task.WhenAll(t3);
                            dt = t3.Status == TaskStatus.RanToCompletion ? t3.Result : null;
                            try
                            {
                                //Mailler mailler = new Mailler(this._configuration);
                                //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/BusinessPartnerOtpAuth.txt");
                                //string MaillerBody = System.IO.File.ReadAllText(fPath);
                                //MaillerBody = MaillerBody.Replace("@Otp@", Recid);
                                //mailler.SendMail(emailid, "", 10001, "Dealer onboarding-ICICI Lombard - Otp Verification", MaillerBody);
                                Mailler mailler = new Mailler(this._configuration);
                                var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/BusinessPartnerOtpAuth.html");
                                string MaillerBody = System.IO.File.ReadAllText(fPath);
                                MaillerBody = MaillerBody.Replace("@Otp@", Recid);
                                mailler.SendMail(emailid, "", 10001, "ICICI:Otp Verification - NO REPLY", MaillerBody);
                                dbLogger.PostEmailLog("BOnboard", MaillerBody, "GenerateOtp", 10001, emailid, "Onboard OTP", true);
                                objList.Message = "Success";
                                objList.FinalMode = DBReturnGridRecord.RecordFound;
                                objList.Data = dt;
                            }
                            catch (Exception ex)
                            {
                                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "OTPmail", 10001, "Admin", true);
                                return objList;
                            }
                         
                        }
                        return objList;
                    }
                }
                else
                {
                    // commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    return objList;
                }
                return objList;
            }
            catch (Exception ex)
            {
                objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                // commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    //commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "GenerateOtp", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonList> VerifyOTP(OnboardOtp obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.Type = "client";
                var t1 = Task.Run(() => dbOnboard.Verify_Client_OTP(obj).Tables[0]);
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.Message = "Success";
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Data = dt;
                }
                else
                {
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "VerifyOTP", 10001, "Admin", true);
                return objList;
            }

        }

        public async Task<CommonList> GetOnboardApprovalList(OnboardSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("onboardapproval", WhereCond));
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
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "GetOnboardApprovalList", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetOnbaord(OnboardSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("onboard", WhereCond));
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
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "GetOnbaord", 10001, "Admin", true);
                return objList;
            }
        }

        //public async Task<CommonIUD> PostOnboardApproval(OnboardClass obj)
        //{
        //    commonIUD = new CommonIUD();
        //    var Recid = (dynamic)null;
        //    try
        //    {
        //        if (obj.OnBoardId != null && obj.OnBoardId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.OnBoardId = 0; }
        //        var t1 = Task.Run(() => dbOnboard.PostdbOnboardApproval(obj));
        //        await Task.WhenAll(t1);
        //        Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
        //        if (Recid != null && Recid != 0)
        //        {
        //            commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
        //            if (obj.OnBoardId != null && obj.OnBoardId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
        //            commonIUD.Recid = Recid;
        //            if (obj.Action == "insert" && Recid != null && Recid != 0)
        //            {
        //                Mailler mailler = new Mailler(this._configuration);
        //                mailler.SendMail(obj.EmailId, "", 10001, "Dealer Dealer Test", "Dealer Dealer Body");
        //            }
        //            if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.OnBoardId = 0; }
        //            commonIUD.AdditionalParameter = "";
        //            return commonIUD;
        //        }
        //        else
        //        {
        //            commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
        //            return commonIUD;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
        //        if (ex.Message.Contains("UNIQUE KEY"))
        //        {
        //            commonIUD.Message = "Cannot insert duplicate record.";
        //        }
        //        dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "PostDealer", 10001, "Admin", true);
        //        return commonIUD;
        //    }
        //}

        public async Task<CommonListDataSet> PostBulkOnboard(string FileName, Int64 CreatedBy, string filePath, string fileExt, DataTable dtdata)
        {
            // DBMessage message = new DBMessage();
            objListDataSet = new CommonListDataSet();
            DataSet dt = new DataSet();
            try
            {
                string Colums = string.Empty;
                int i = dbOnboard.PostdbBulkOnboard(dtdata);
                if (i == 1)
                {
                    var t2 = Task.Run(() => dbOnboard.PostdbOnboardUpload());
                    await Task.WhenAll(t2);
                    dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                    if (dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Tables[0].Rows)
                            {
                                OnboardClass onboardClass = new OnboardClass();
                                commonIUD = new CommonIUD();
                                onboardClass.OnBoardName = Convert.ToString(dr["Marketing Firm Name"]);
                                onboardClass.MobileNo = Convert.ToString(dr["Mobile No"]);
                                onboardClass.EmailId = Convert.ToString(dr["Email Id"]);
                                var t1 = Task.Run(() => PostOnboard(onboardClass));
                                await Task.WhenAll(t1);
                                commonIUD = t1.Status == TaskStatus.RanToCompletion ? t1.Result : commonIUD;
                            }

                        }

                        objListDataSet.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        objListDataSet.Data = dt;
                        objListDataSet.Count = dt.Tables.Count;
                        objListDataSet.Message = "";
                        objListDataSet.AdditionalParameter = "";
                        return objListDataSet;
                    }

                }
                return objListDataSet;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "PostBulkOnboard", 10001, "Admin", true);
                return objListDataSet;
            }
        }

        public async Task<CommonListDataSet> PostOnboadDataMigration(string FileName, Int64 CreatedBy, string filePath, string fileExt, DataTable dtdata)
        {
            // DBMessage message = new DBMessage();
            objListDataSet = new CommonListDataSet();
            DataSet dt = new DataSet();
            var Recid = (dynamic)null;
            try
            {
                string Colums = string.Empty;
                int i = dbOnboard.PostdbBulkOnboadDataMigration(dtdata);
                if (i == 1)
                {
                    var t2 = Task.Run(() => dbOnboard.PostdbOnboadDataMigration());
                    await Task.WhenAll(t2);
                    dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                    if (dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Tables[0].Rows)
                            {
                                OnboardClass onboardClass = new OnboardClass();
                                commonIUD = new CommonIUD();
                                var t1 = Task.Run(() => dbOnboard.PostdbOnboardDataMigration(dr, onboardClass));
                                await Task.WhenAll(t1);
                                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : Recid;
                                if (Recid != null && Recid != 0)
                                {
                                    try
                                    {
                                        //BDealer bDealer = new BDealer(this._configuration);
                                        var t3 = Task.Run(() => SendDealerAgreement(Recid, "DealerId", Convert.ToString(dr["Agreement Start Date"]), Convert.ToString(dr["Agreement End Date"])));
                                        await Task.WhenAll(t3);
                                        dynamic argrrmentStatus = t3.Status == TaskStatus.RanToCompletion ? t3.Result : null;
                                    }
                                    catch (Exception ex)
                                    {
                                        dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "PostOnboadDataMigrationmail", 10001, "Admin", true);
                                        return null;
                                    }
                                }
                            }

                        }

                        objListDataSet.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        objListDataSet.Data = dt;
                        objListDataSet.Count = dt.Tables.Count;
                        objListDataSet.Message = "";
                        objListDataSet.AdditionalParameter = "";
                        return objListDataSet;
                    }

                }
                return objListDataSet;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "PostOnboadDataMigration", 10001, "Admin", true);
                return objListDataSet;
            }
        }

        public async Task<CommonIUD> SendDealerAgreement(dynamic Recid, string condition, string AgreementStartDate = "", string AgreementEndDate = "")
        {
            commonIUD = new CommonIUD();
            DealerClass objDealer = new DealerClass();
            try
            {
                string WhereCond = " And " + condition + "=" + Convert.ToString(Recid);
                DataTable dataTable;
                dbCommon = new dbCommon(this._configuration);
                var t3 = Task.Run(() => dbCommon.DynamicQuery("dealer", WhereCond));
                await Task.WhenAll(t3);
                dataTable = t3.Status == TaskStatus.RanToCompletion ? t3.Result : null;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    objDealer.DealerId = Convert.ToInt32(dataTable.Rows[0]["DealerId"]);
                    objDealer.DealerCode = Convert.ToString(dataTable.Rows[0]["DealerCode"]);
                    objDealer.DealerName = Convert.ToString(dataTable.Rows[0]["DealerName"]);
                    objDealer.EmailId = Convert.ToString(dataTable.Rows[0]["EmailId"]);
                    objDealer.CreatedBy = Convert.ToInt32(dataTable.Rows[0]["CreatedBy"]);
                    objDealer.IpAddress = Convert.ToString(dataTable.Rows[0]["IpAddress"]);
                }
                DateTime? startDate = new DateTime();
                DateTime? toDate = new DateTime();
                try
                {
                    startDate = Convert.ToDateTime(AgreementStartDate);
                    //startDate = DateTime.ParseExact(AgreementStartDate, "dd/MM/yyyy", null);
                    toDate = Convert.ToDateTime(AgreementEndDate);
                    //toDate= DateTime.ParseExact(AgreementEndDate, "dd/MM/yyyy", null);
                }
                catch
                {
                    startDate = DateTime.Now;
                    toDate = DateTime.Now.AddDays(1095);
                }

                //Insert Agreement
                DealerAgreement agg = new DealerAgreement
                {
                    Action = "send",
                    AgreementId = null,
                    AgreementRefId = objDealer.DealerId,
                    AgreementStartDate = startDate,
                    AgreementEndDate = toDate,
                    SentBy = objDealer.CreatedBy,
                    CreatedBy = objDealer.CreatedBy
                };
                var t1 = Task.Run(() => dbOnboard.PostdbDealerAgreement(agg));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid != null && Recid != 0)
                {
                    try
                    {
                        commonIUD.Recid = Recid;
                        commonIUD.Message = "Agreement Sent Successfully!";
                        commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        agg.AgreementId = 0;
                        commonIUD.AdditionalParameter = "";
                        var t2 = Task.Run(() => dbCommon.PdfScheduler(objDealer.DealerId.ToString(), Recid.ToString(), "ACP", agg.CreatedBy.ToString()));
                        await Task.WhenAll(t2);
                        Recid = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                        
                        //Generate Agreement
                        //string ReportURL = Convert.ToString(this._configuration.AppKey("ReportUrl"));
                        //string ReportUserName = Convert.ToString(this._configuration.AppKey("ReportUserName"));
                        //string ReportPassword = Convert.ToString(this._configuration.AppKey("ReportPassword"));
                        //string ReportHost = Convert.ToString(this._configuration.AppKey("ReportHost"));
                        //string ReportFolderPath = Convert.ToString(this._configuration.AppKey("ReportFolder"));
                        //byte[] pdfByte = null;
                        //ServerReport report = new ServerReport();
                        //report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ReportUserName, ReportPassword, ReportHost);
                        //report.ReportServerUrl = new Uri(ReportURL);
                        //report.ReportPath = ReportFolderPath + "DealerAgreementEmail";
                        //report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(objDealer.DealerId)) });
                        //pdfByte = report.Render("PDF");

                        //var folderName = Path.Combine("wwwroot", "AgreementFilesContainer");
                        //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        //var fileName = objDealer.DealerCode + "_Agreement.pdf";
                        //var outputFile = Path.Combine(pathToSave, fileName);
                        //System.IO.File.WriteAllBytes(outputFile, pdfByte);


                        ////Terms and Conditions
                        //pdfByte = null;
                        //commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        //report.ReportPath = ReportFolderPath + "TnC";
                        //report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(objDealer.DealerId)) });
                        //pdfByte = report.Render("PDF");
                        //var TnCPath = Path.Combine(pathToSave, objDealer.DealerCode + "_TermsAndConditionsPromotionPartners.pdf");
                        //System.IO.File.WriteAllBytes(TnCPath, pdfByte);

                        ////Privacy Policy
                        //pdfByte = null;
                        //commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        //report.ReportPath = ReportFolderPath + "PrivacyPolicy";
                        //report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(objDealer.DealerId)) });
                        //pdfByte = report.Render("PDF");
                        //var PrivacyPolicyPath = Path.Combine(pathToSave, objDealer.DealerCode + "_PrivacyPolicy.pdf");
                        //System.IO.File.WriteAllBytes(PrivacyPolicyPath, pdfByte);

                        //List<string> attachments = new List<string>();
                        //attachments.Add(outputFile);
                        //attachments.Add(PrivacyPolicyPath);
                        //attachments.Add(TnCPath);


                        //commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        //Mailler mailler = new Mailler(this._configuration);
                        //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/DealerAgreementMail.html");
                        //string MaillerBody = System.IO.File.ReadAllText(fPath);
                        //MaillerBody = MaillerBody.Replace("@DealerCode@", objDealer.DealerCode);
                        //MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", objDealer.DealerName);
                        //MaillerBody = MaillerBody.Replace("@UserId@", objDealer.EmailId);
                        //MaillerBody = MaillerBody.Replace("@Pass@", "12345");

                        //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "agreement?" + SecurityEncy.Encrypt("agreementId=" + Recid));
                        //if (mailler.SendMail(objDealer.EmailId, attachments, 10001, "ICICI: Dealer Agreement - NO REPLY", MaillerBody))
                        //{
                        //    commonIUD.Recid = Recid;
                        //    commonIUD.Message = "Agreement Sent Successfully!";
                        //    agg.AgreementId = 0;
                        //    commonIUD.AdditionalParameter = "";
                        //}
                        //if (System.IO.File.Exists(outputFile))
                        //{
                        //    System.IO.File.Delete(outputFile);
                        //}
                        //if (System.IO.File.Exists(PrivacyPolicyPath))
                        //{
                        //    System.IO.File.Delete(PrivacyPolicyPath);
                        //}
                        //if (System.IO.File.Exists(TnCPath))
                        //{
                        //    System.IO.File.Delete(TnCPath);
                        //}
                    }
                    catch (Exception ex)
                    {
                        dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "SendDealerAgreement", 10001, "Admin", true);
                        return null;
                    }
                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                }
                return commonIUD;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BOnboard", ex.Message.ToString(), "SendDealerAgreement", 10001, objDealer.CreatedBy == null ? (objDealer.UpdatedBy == null ? null : Convert.ToString(objDealer.UpdatedBy)) : Convert.ToString(objDealer.CreatedBy), true);
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                commonIUD.Message = "Mail Sent Failed!";
                return commonIUD;
            }
        }
    }

}
