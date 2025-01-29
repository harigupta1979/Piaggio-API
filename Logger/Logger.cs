using AppConfig;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class DbLogger
    {
        private readonly AppConfig.IConfigManager _configuration;
        public DbLogger(IConfigManager configuration)
        {
            this._configuration = configuration;
        }

        public bool PostErrorLog(String controller, String errorMsg, String functionName, decimal? ReferenceID, string UserName, bool IsError = false)
        {
            try
            {
                Errorlog dbErrorLog = new Errorlog();
                dbErrorLog.ControllerName = controller;
                dbErrorLog.ErrorMessage = errorMsg;
                dbErrorLog.FunctionName = functionName;
                dbErrorLog.UserName = UserName;
                dbErrorLog.LastUpdatedOn = DateTime.Now;
                dbErrorLog.ReferenceID = ReferenceID;
                AddtoDbLogger(dbErrorLog);
                ErrorMail(dbErrorLog);
                return true;
            }
            catch (Exception ex)
            {
                AddtoDbException(ex.Message);
                return false;
            }
        }
        public bool PostEmailLog(String controller, String Msgmody, String functionName, decimal? ReferenceID, string emailid,string MailType, bool IsError = false)
        {
            try
            {
                Errorlog dbErrorLog = new Errorlog();
                dbErrorLog.ControllerName = controller;
                dbErrorLog.ErrorMessage = Msgmody;
                dbErrorLog.FunctionName = functionName;
                dbErrorLog.UserName = emailid;
                dbErrorLog.LastUpdatedOn = DateTime.Now;
                dbErrorLog.ReferenceID = ReferenceID;
                dbErrorLog.MailType = MailType;
                AddtoDbEmailLogger(dbErrorLog);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void AddtoDbException(string errMessage)
        {
            try
            {
                Errorlog dbErrorLog = new Errorlog();
                dbErrorLog.ControllerName = "Error Occuer while logging to ELK";
                dbErrorLog.FunctionName = "PostLogToELK";
                dbErrorLog.UserName = "Username";
                dbErrorLog.LastUpdatedOn = DateTime.Now;
                dbErrorLog.ReferenceID = 11111;
                dbErrorLog.ErrorMessage = errMessage;
                AddtoDbLogger(dbErrorLog);
            }
            catch (Exception ex)
            {
                AddtoDbException(ex.Message);
            }
        }
        public bool AddtoDbLogger(Errorlog obj)
        {
            SqlConnection myConnection = new SqlConnection(this._configuration.GetConnectionString("ICPLDatabase"));
            SqlCommand myCmd = new SqlCommand("usp_Logger", myConnection);
            myCmd.CommandType = CommandType.StoredProcedure;
            myCmd.Parameters.AddWithValue("@FunctionName", obj.FunctionName);
            myCmd.Parameters.AddWithValue("@ControllerName", obj.ControllerName);
            myCmd.Parameters.AddWithValue("@UserName", obj.UserName);
            myCmd.Parameters.AddWithValue("@ErrorMessage", obj.ErrorMessage);
            myCmd.Parameters.AddWithValue("@ReferenceID", obj.ReferenceID);
            myCmd.Parameters.AddWithValue("@LastUpdatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            myConnection.Open();
            object message = myCmd.ExecuteScalar();
            myConnection.Close();
            int id = 0;
            if (message != null)
            {
                Int32.TryParse(message.ToString(), out id);
                return true;
            }
            return false;
        }
        public bool AddtoDbEmailLogger(Errorlog obj)
        {
            SqlConnection myConnection = new SqlConnection(this._configuration.GetConnectionString("ICPLDatabase"));
            SqlCommand myCmd = new SqlCommand("usp_EmailLogger", myConnection);
            myCmd.CommandType = CommandType.StoredProcedure;
            myCmd.Parameters.AddWithValue("@FunctionName", obj.FunctionName);
            myCmd.Parameters.AddWithValue("@ControllerName", obj.ControllerName);
            myCmd.Parameters.AddWithValue("@EmailId", obj.UserName);
            myCmd.Parameters.AddWithValue("@MaillerBody", obj.ErrorMessage);
            myCmd.Parameters.AddWithValue("@MailType", obj.MailType);
            myCmd.Parameters.AddWithValue("@ReferenceID", obj.ReferenceID);
            myCmd.Parameters.AddWithValue("@LastUpdatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            myConnection.Open();
            object message = myCmd.ExecuteScalar();
            myConnection.Close();
            int id = 0;
            if (message != null)
            {
                Int32.TryParse(message.ToString(), out id);
                return true;
            }
            return false;
        }

        public void ErrorMail(Errorlog obj)
        {
            try
            {
                var fPath = Path.Combine(Directory.GetCurrentDirectory(), "MailerContent/ErrorMail.html");
                string MaillerBody = System.IO.File.ReadAllText(fPath);
                MaillerBody = MaillerBody.Replace("@ControllerName@", obj.ControllerName);
                MaillerBody = MaillerBody.Replace("@ErrorMessage@", obj.ErrorMessage);
                MaillerBody = MaillerBody.Replace("@FunctionName@", obj.FunctionName);
                MaillerBody = MaillerBody.Replace("@UserName@", obj.UserName);
                MaillerBody = MaillerBody.Replace("@LastUpdatedOn@", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                MaillerBody = MaillerBody.Replace("@ReferenceID@", obj.ReferenceID.ToString());
                SendMail(Convert.ToString(this._configuration.AppKey("Errormail")), "", 10001, "Piaggio:ErrorMail", MaillerBody);
            }
            catch (Exception ex)
            {
                //ErrorMail(ex.Message);
            }
        }
        public bool SendMail(string EmailId, string Attachment, decimal TxnLog, string Subject, string MailBody)
        {
            try
            {
                string ToMail = EmailId.Trim();
                System.Net.Mail.AlternateView AvHtml = System.Net.Mail.AlternateView.CreateAlternateViewFromString(Convert.ToString(MailBody), null, System.Net.Mime.MediaTypeNames.Text.Html);
                System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
                SmtpClient.Host = this._configuration.AppKey("SMTPHost"); // System.Configuration.ConfigurationManager.AppSettings["SMTPHost"];
                SmtpClient.Port = Convert.ToInt32(this._configuration.AppKey("SMTPPort")); //Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                String FromEmail = this._configuration.AppKey("SMTPSenderMailAddress");  //System.Configuration.ConfigurationManager.AppSettings["SMTPSenderMailAddress"];
                String Password = this._configuration.AppKey("SMTPPassword");
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
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
    }

}