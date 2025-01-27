using AppConfig;
using DataAccessLayer;
using Logger;
using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class Mailler
    {
        private readonly IConfigManager _configuration;
        dbCommon dbCommon; DbLogger dbLogger;
        public Mailler(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            dbCommon = new dbCommon(this._configuration);
        }
        public bool SendMail(string EmailId, string Attachment, decimal TxnLog, string Subject, string MailBody)
        {
            try
            {
                string ToMail = EmailId.Trim();
                System.Net.Mail.AlternateView AvHtml = System.Net.Mail.AlternateView.CreateAlternateViewFromString(Convert.ToString(MailBody), null, System.Net.Mime.MediaTypeNames.Text.Html);
                System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
                var smtpData = dbCommon.DynamicSMTP();
                SmtpClient.Host = this._configuration.AppKey("SMTPHost"); // System.Configuration.ConfigurationManager.AppSettings["SMTPHost"];
                SmtpClient.Port = Convert.ToInt32(this._configuration.AppKey("SMTPPort")); //Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                string smtpId = smtpData.Rows[0]["SMTPId"].ToString();
                string FromEmail = smtpData.Rows[0]["SMTPSenderMailAddress"].ToString();  //System.Configuration.ConfigurationManager.AppSettings["SMTPSenderMailAddress"];
                string Password = smtpData.Rows[0]["SMTPPassword"].ToString();  //System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                using (System.Net.Mail.MailMessage ObjMail = new System.Net.Mail.MailMessage(FromEmail, ToMail))
                {
                    ObjMail.IsBodyHtml = true;
                    ObjMail.Subject = Subject;
                    ObjMail.AlternateViews.Add(AvHtml);
                    SmtpClient.EnableSsl = true;
                    SmtpClient.UseDefaultCredentials = false;
                    SmtpClient.Credentials = new System.Net.NetworkCredential(FromEmail, Password);
                    System.Net.Mail.Attachment At = null;
                    if (Attachment != "" && Attachment != null)
                    {
                        At = new System.Net.Mail.Attachment(Attachment);
                        ObjMail.Attachments.Add(At);
                    }
                    ObjMail.Priority = System.Net.Mail.MailPriority.High;
                    SmtpClient.Send(ObjMail);
                    SmtpClient.Dispose();
                    if (Attachment != "" && Attachment != null)
                    {
                        At.Dispose();
                    }
                }
                var dt = dbCommon.UpdateSMTP(smtpId);
                if (Convert.ToBoolean(dt.Rows[0][0]))
                {
                    dbLogger.SendMail(this._configuration.AppKey("Errormail"), "", 10001, "SMTP Exhausted: " + FromEmail, "SMTP Exhausted: " + FromEmail);
                }
                return true;
            }
            catch (Exception Ex)
            {
                dbLogger.SendMail(this._configuration.AppKey("Errormail"), "", 10001, "SMTP Error", "SMTP Error: " + Ex.Message + ";" + Ex.StackTrace);
                return false;
            }
        }

        public bool SendMail(string EmailId, List<string> Attachments, decimal TxnLog, string Subject, string MailBody)
        {
            try
            {
                string ToMail = EmailId.Trim();
                System.Net.Mail.AlternateView AvHtml = System.Net.Mail.AlternateView.CreateAlternateViewFromString(Convert.ToString(MailBody), null, System.Net.Mime.MediaTypeNames.Text.Html);
                System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
                var smtpData = dbCommon.DynamicSMTP();
                SmtpClient.Host = this._configuration.AppKey("SMTPHost"); // System.Configuration.ConfigurationManager.AppSettings["SMTPHost"];
                SmtpClient.Port = Convert.ToInt32(this._configuration.AppKey("SMTPPort")); //Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                string smtpId = smtpData.Rows[0]["SMTPId"].ToString();
                string FromEmail = smtpData.Rows[0]["SMTPSenderMailAddress"].ToString();  //System.Configuration.ConfigurationManager.AppSettings["SMTPSenderMailAddress"];
                string Password = smtpData.Rows[0]["SMTPPassword"].ToString();  //System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                using (System.Net.Mail.MailMessage ObjMail = new System.Net.Mail.MailMessage(FromEmail, ToMail))
                {
                    ObjMail.IsBodyHtml = true;
                    ObjMail.Subject = Subject;
                    ObjMail.AlternateViews.Add(AvHtml);
                    SmtpClient.EnableSsl = true;
                    SmtpClient.UseDefaultCredentials = false;
                    SmtpClient.Credentials = new System.Net.NetworkCredential(FromEmail, Password);
                    System.Net.Mail.Attachment At = null;
                    if (Attachments != null)
                    {
                        foreach (string Attachment in Attachments)
                        {
                            At = new System.Net.Mail.Attachment(Attachment);
                            ObjMail.Attachments.Add(At);
                        }
                    }
                    ObjMail.Priority = System.Net.Mail.MailPriority.High;
                    SmtpClient.Send(ObjMail);
                    SmtpClient.Dispose();
                    if (Attachments != null)
                    {
                        At.Dispose();
                    }
                }
                var dt = dbCommon.UpdateSMTP(smtpId);
                if (Convert.ToBoolean(dt.Rows[0][0]))
                {
                    dbLogger.SendMail(this._configuration.AppKey("Errormail"), "", 10001, "SMTP Exhausted: " + FromEmail, "SMTP Exhausted: " + FromEmail);
                }
                return true;
            }
            catch (Exception Ex)
            {
                dbLogger.SendMail(this._configuration.AppKey("Errormail"), "", 10001, "SMTP Error", "SMTP Error: " + Ex.Message + ";" + Ex.StackTrace);
                return false;
            }
        }
    }
}
