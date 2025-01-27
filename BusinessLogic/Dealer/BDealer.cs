using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.Dealer;
using DataAccessLayer;
using DataAccessLayer.Dealer;
using DocumentFormat.OpenXml.Packaging;
using Logger;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic.Dealer
{
    public class BDealer
    {
        private readonly IConfigManager _configuration;
        dbDealer dbDealer; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objList;

        public BDealer(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbDealer = new dbDealer(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostDealer(DealerClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                if (obj.DealerId != null && obj.DealerId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.DealerId = 0; }
                var t1 = Task.Run(() => dbDealer.PostdbDealer(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
                else if (Recid != null && Recid != 0)
                {
                    if (obj.Action == "insert")
                    {
                        //------------------
                        //Mailler mailler = new Mailler(this._configuration);
                        //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderLogin.txt");
                        //string MaillerBody = System.IO.File.ReadAllText(fPath);
                        //MaillerBody = MaillerBody.Replace("@BusinessPartnerName@", obj.DealerName);
                        //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "Auth/Login");
                        //MaillerBody = MaillerBody.Replace("@Password@", "12345");
                        //MaillerBody = MaillerBody.Replace("@UserName@", obj.EmailId.Trim());
                        //mailler.SendMail(obj.EmailId, "", 10001, "Dealer Access Details", MaillerBody);
                        Mailler mailler = new Mailler(this._configuration);
                        var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderLogin.html");
                        string MaillerBody = System.IO.File.ReadAllText(fPath);
                        MaillerBody = MaillerBody.Replace("@Name@", obj.DealerName);
                        if (string.IsNullOrEmpty(obj.DealerCode))
                        {
                            MaillerBody = MaillerBody.Replace("@DealerCode@", "");
                        }
                        else
                        {
                            MaillerBody = MaillerBody.Replace("@DealerCode@", "<p>Your Dealer Code: " + obj.DealerCode + "</p>");
                        }
                        MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "Auth/Login");
                        MaillerBody = MaillerBody.Replace("@Password@", "12345");
                        MaillerBody = MaillerBody.Replace("@UserName@", obj.EmailId.Trim());
                        mailler.SendMail(obj.EmailId.Trim(), "", 10001, "ICICI: Dealer Access Details", MaillerBody);
                        //-------------------
                    }

                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.DealerId != null && obj.DealerId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.DealerId = 0; }
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "PostDealer", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GetDealer(DealerSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("dealer", WhereCond));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "GetDealer", 10001, Convert.ToString(obj.SearchBy), true);
                return objList;
            }
        }

        public async Task<CommonIUD> PostDealerImage(DealerImage obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                obj.DocumentId = 0; obj.Action = "insert"; obj.IsActive = true;
                var folderName = Path.Combine("Resources", "DealerImage");
                var fPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                var finalFileName = obj.ImgFileName + '.' + obj.FileExtension;
                obj.ImgFileName = finalFileName;
                obj.ImgPath = Path.Combine(folderName, finalFileName);
                File.WriteAllBytes(obj.ImgPath, Convert.FromBase64String(obj.ImgBase64.Replace("data:image/png;base64,", String.Empty)));
                var t1 = Task.Run(() => dbDealer.PostdbDealerImage(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.DocumentId != null && obj.DocumentId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.DocumentId = 0; }
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "PostDealerImage", 10001, "Admin", true);
                return commonIUD;
            }

        }

        public async Task<CommonList> GetDealerDocument(DealerDocSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("dealerdocument", WhereCond));
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "GetDealerDocument", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> DealerDocumentDelete(DealerDocSearch obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.Action = "delete";
                var t1 = Task.Run(() => dbDealer.DealerDocumentDelete(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "DealerDocumentDelete", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonList> VerifyOTP(DealerAgreement obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                obj.Type = "agreement";
                var t1 = Task.Run(() => dbDealer.Verify_Agreement_OTP(obj).Tables[0]);
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "VerifyOTP", 10001, "Admin", true);
                return objList;
            }

        }
        public async Task<CommonList> GenerateOtp(DealerAgreement obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("dealeragreement", WhereCond));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Recid = SecurityEncy.GenerateOTP();
                        string emailid = Convert.ToString(dt.Rows[0]["EmailId"]);
                        commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                        commonIUD.AdditionalParameter = Recid;
                        if (Recid != null && Recid != "")
                        {
                            obj.Username = emailid;
                            obj.Userotp = int.Parse(Recid);
                            obj.Type = "agreement";
                            var t3 = Task.Run(() => dbDealer.PostGenerateAgreementOTP(obj).Tables[0]);
                            await Task.WhenAll(t3);
                            dt = t3.Status == TaskStatus.RanToCompletion ? t3.Result : null;
                            try { 
                            Mailler mailler = new Mailler(this._configuration);
                            var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/BusinessPartnerOtpAuth.html");
                            string MaillerBody = System.IO.File.ReadAllText(fPath);
                            MaillerBody = MaillerBody.Replace("@Otp@", Recid);
                            mailler.SendMail(emailid, "", 10001, "ICICI:Otp Verification - NO REPLY", MaillerBody);
                            dbLogger.PostEmailLog("BDealer", MaillerBody, "GenerateOtp", 10001, emailid, "Agreement OTP", true);
                                objList.Message = "Success";
                                objList.FinalMode = DBReturnGridRecord.RecordFound;
                                objList.Data = dt;
                            }
                            catch (Exception ex)
                            {
                                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "OTPmail", 10001, "Admin", true);
                                return objList;
                            }

                        }
                        return objList;
                    }
                }
                else
                {
                    //  commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                    objList.FinalMode = DBReturnGridRecord.RecordNotFound;
                    return objList;
                }
                return objList;
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    objList.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "GenerateOtp", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonIUD> AcceptDealerAgreement(DealerAgreement obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            dbCommon = new dbCommon(this._configuration);
            DataTable dt;
            try
            {


                obj.Action = "accept";
                var t3 = Task.Run(() => dbDealer.PostdbDealerAgreement(obj));
                await Task.WhenAll(t3);
                Recid = t3.Status == TaskStatus.RanToCompletion ? t3.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
                else if (Recid != null && Recid != 0)
                {
                    obj.IpAddress = "";
                    QueryBuilder queryBuilder = new QueryBuilder();
                    var t1 = Task.Run(() => queryBuilder.BuildQuerySearch(obj));
                    await Task.WhenAll(t1);
                    string WhereCond = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                    var t2 = Task.Run(() => dbCommon.DynamicQuery("dealeragreement", WhereCond));
                    await Task.WhenAll(t2);
                    dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                    var t4 = Task.Run(() => dbCommon.PdfScheduler(dt.Rows[0]["DealerId"].ToString(), obj.AgreementId.ToString(), "ACV", ""));
                    await Task.WhenAll(t4);
                    Recid = t4.Status == TaskStatus.RanToCompletion ? t4.Result : null;
                    //string dealerId = Convert.ToString(dt.Rows[0]["DealerId"]);
                    //string dealerCode = Convert.ToString(dt.Rows[0]["DealerCode"]);
                    //string ReportURL = Convert.ToString(this._configuration.AppKey("ReportUrl"));
                    //string ReportUserName = Convert.ToString(this._configuration.AppKey("ReportUserName"));
                    //string ReportPassword = Convert.ToString(this._configuration.AppKey("ReportPassword"));
                    //string ReportHost = Convert.ToString(this._configuration.AppKey("ReportHost"));
                    //string ReportFolderPath = Convert.ToString(this._configuration.AppKey("ReportFolder"));
                    //var folderName = Path.Combine("wwwroot", "AgreementFilesContainer");
                    //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    //ServerReport report = new ServerReport();
                    //report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ReportUserName, ReportPassword, ReportHost);
                    //report.ReportServerUrl = new Uri(ReportURL);


                    ////Agreement
                    //byte[] pdfByte = null;
                    //report.ReportPath = ReportFolderPath + "DealerAgreementEmail";
                    //report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(dealerId)) });
                    //pdfByte = report.Render("PDF");
                    //var AgreementPath = Path.Combine(pathToSave, dealerCode + "_Agreement.pdf");
                    //System.IO.File.WriteAllBytes(AgreementPath, pdfByte);

                    ////Terms and Conditions
                    //pdfByte = null;
                    //report.ReportPath = ReportFolderPath + "TnC";
                    //report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(dealerId)) });
                    //pdfByte = report.Render("PDF");
                    //var TnCPath = Path.Combine(pathToSave, dealerCode + "_TermsAndConditionsPromotionPartners.pdf");
                    //System.IO.File.WriteAllBytes(TnCPath, pdfByte);

                    ////Privacy Policy
                    //pdfByte = null;
                    //report.ReportPath = ReportFolderPath + "PrivacyPolicy";
                    //report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(dealerId)) });
                    //pdfByte = report.Render("PDF");
                    //var PrivacyPolicyPath = Path.Combine(pathToSave, dealerCode + "_PrivacyPolicy.pdf");
                    //System.IO.File.WriteAllBytes(PrivacyPolicyPath, pdfByte);

                    //List<string> attachments = new List<string>();
                    //attachments.Add(AgreementPath);
                    //attachments.Add(PrivacyPolicyPath);
                    //attachments.Add(TnCPath);
                    //Mailler mailler = new Mailler(this._configuration);
                    //var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/WelcomeVenderLogin.html");
                    //string MaillerBody = System.IO.File.ReadAllText(fPath);
                    //if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DealerCode"])))
                    //{
                    //    MaillerBody = MaillerBody.Replace("@DealerCode@", "");
                    //}
                    //else
                    //{
                    //    MaillerBody = MaillerBody.Replace("@DealerCode@", "<p>Your Dealer Code: " + Convert.ToString(dt.Rows[0]["DealerCode"]) + "</p>");
                    //}
                    //MaillerBody = MaillerBody.Replace("@Name@", dt.Rows[0]["DealerName"].ToString());
                    //MaillerBody = MaillerBody.Replace("@UrlLink@", this._configuration.AppKey("businessUrl") + "auth/login");
                    //MaillerBody = MaillerBody.Replace("@Password@", "12345");
                    //MaillerBody = MaillerBody.Replace("@UserName@", dt.Rows[0]["EmailId"].ToString());
                    //mailler.SendMail(dt.Rows[0]["EmailId"].ToString(), attachments, 10001, "ICICI: Marketing Firm Access Details", MaillerBody);

                    if (obj.AgreementId != null && obj.AgreementId != 0)
                    {
                        commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE;
                    }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "accept")
                    {
                        commonIUD.Message = "Agreement Accepted Successfully!";
                    }
                    commonIUD.AdditionalParameter = "";
                    //if (System.IO.File.Exists(AgreementPath))
                    //{
                    //    System.IO.File.Delete(AgreementPath);
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
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "AcceptDealerAgreement", 10001, "Admin", true);
                return commonIUD;
            }
        }
        public async Task<CommonIUD> GetAgreementFile(DealerAgreement obj)
        {
            dbCommon = new dbCommon(this._configuration);
            commonIUD = new CommonIUD();
            DataTable dt;
            try
            {
                QueryBuilder queryBuilder = new QueryBuilder();
                var t1 = Task.Run(() => queryBuilder.BuildQuerySearch(obj));
                await Task.WhenAll(t1);
                string WhereCond = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                var t2 = Task.Run(() => dbCommon.DynamicQuery("dealeragreement", WhereCond));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                if (dt != null && dt.Rows.Count != 0)
                {
                    if (Convert.ToString(dt.Rows[0]["AgreementStatus"]) == "A")
                    {
                        commonIUD.FinalMode = DBReturnGridRecord.RecordFound;
                        commonIUD.AdditionalParameter = "Agreement url already accepted";
                        commonIUD.Message = "RecordFound";
                        commonIUD.Recid = 1;
                    }
                    else
                    {
                        string dealerId = Convert.ToString(dt.Rows[0]["DealerId"]);
                        string ReportURL = Convert.ToString(this._configuration.AppKey("ReportUrl"));
                        string ReportUserName = Convert.ToString(this._configuration.AppKey("ReportUserName"));
                        string ReportPassword = Convert.ToString(this._configuration.AppKey("ReportPassword"));
                        string ReportHost = Convert.ToString(this._configuration.AppKey("ReportHost"));
                        string ReportFolderPath = Convert.ToString(this._configuration.AppKey("ReportFolder"));
                        byte[] pdfByte = null;
                        ServerReport report = new ServerReport();
                        report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ReportUserName, ReportPassword, ReportHost);
                        report.ReportServerUrl = new Uri(ReportURL);
                        report.ReportPath = ReportFolderPath + "DealerAgreementEmail";
                        report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(dealerId)) });
                        pdfByte = report.Render("PDF");
                        commonIUD.FinalMode = DBReturnGridRecord.RecordFound;
                        commonIUD.AdditionalParameter = Convert.ToBase64String(pdfByte, 0, pdfByte.Length);
                        commonIUD.Message = "RecordFound";
                        commonIUD.Recid = 0;
                    }
                }
                else
                {
                    commonIUD.FinalMode = DBReturnGridRecord.RecordNotFound;
                    commonIUD.Message = "RecordNotFound";
                    commonIUD.Recid = 0;
                }
                return commonIUD;
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                commonIUD.Message = "ERROR";
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "GetAgreementFile", 10001, "Admin", true);
                return commonIUD;
            }
        }
        public async Task<CommonIUD> GetOtherAgreementDocuments(AgreementDoc agreementDoc)
        {
            commonIUD = new CommonIUD();
            try
            {
                if (agreementDoc.DocType == "Privacy")
                {
                    var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/PrivacyPolicy.pdf");
                    byte[] pdfByte = await File.ReadAllBytesAsync(fPath);
                    commonIUD.FinalMode = DBReturnGridRecord.RecordFound;
                    commonIUD.AdditionalParameter = Convert.ToBase64String(pdfByte, 0, pdfByte.Length);
                    commonIUD.Message = "RecordFound";
                }
                else if (agreementDoc.DocType == "Terms")
                {
                    var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/TermsAndConditionsPromotionPartners.pdf");
                    byte[] pdfByte = await File.ReadAllBytesAsync(fPath);
                    commonIUD.FinalMode = DBReturnGridRecord.RecordFound;
                    commonIUD.AdditionalParameter = Convert.ToBase64String(pdfByte, 0, pdfByte.Length);
                    commonIUD.Message = "RecordFound";
                }
                else
                {
                    commonIUD.FinalMode = DBReturnGridRecord.RecordNotFound;
                    commonIUD.Message = "RecordNotFound";

                }
                return commonIUD;
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                commonIUD.Message = "ERROR";
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "GetAgreementFile", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonIUD> DownloadAgreementDocuments(AgreementDoc obj)
        {
            dbCommon = new dbCommon(this._configuration);
            commonIUD = new CommonIUD();
            DataTable dt;
            try
            {
                string WhereCond = " AND AgreementId=" + obj.AgreementId;
                var t2 = Task.Run(() => dbCommon.DynamicQuery("dealeragreement", WhereCond));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                if (dt != null && dt.Rows.Count != 0)
                {
                    string dealerId = Convert.ToString(dt.Rows[0]["DealerId"]);
                    string ReportURL = Convert.ToString(this._configuration.AppKey("ReportUrl"));
                    string ReportUserName = Convert.ToString(this._configuration.AppKey("ReportUserName"));
                    string ReportPassword = Convert.ToString(this._configuration.AppKey("ReportPassword"));
                    string ReportHost = Convert.ToString(this._configuration.AppKey("ReportHost"));
                    string ReportFolderPath = Convert.ToString(this._configuration.AppKey("ReportFolder"));
                    byte[] pdfByte = null;
                    ServerReport report = new ServerReport();
                    report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ReportUserName, ReportPassword, ReportHost);
                    report.ReportServerUrl = new Uri(ReportURL);
                    if (obj.DocType == "Agreement")
                    {
                        report.ReportPath = ReportFolderPath + "DealerAgreementEmail";
                    }
                    else if (obj.DocType == "Privacy")
                    {
                        report.ReportPath = ReportFolderPath + "PrivacyPolicy";
                    }
                    else if (obj.DocType == "Terms")
                    {
                        report.ReportPath = ReportFolderPath + "TnC";
                    }
                    report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(dealerId)) });
                    pdfByte = report.Render("PDF");
                    commonIUD.FinalMode = DBReturnGridRecord.RecordFound;
                    commonIUD.AdditionalParameter = Convert.ToBase64String(pdfByte, 0, pdfByte.Length);
                    commonIUD.Message = "RecordFound";
                    commonIUD.Recid = 0;
                }
                else
                {
                    commonIUD.FinalMode = DBReturnGridRecord.RecordNotFound;
                    commonIUD.Message = "RecordNotFound";
                    commonIUD.Recid = 0;
                }
                return commonIUD;
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                commonIUD.Message = "ERROR";
                dbLogger.PostErrorLog("BDealer", ex.Message.ToString(), "GetAgreementFile", 10001, "Admin", true);
                return commonIUD;
            }
        }
    }
}
