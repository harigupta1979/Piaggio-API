using AppConfig;
using Core.Module.Dealer;
using DataAccessLayer.DataAccess;
using System;
using System.Data.SqlClient;
using Logger;
using System.Data;

namespace DataAccessLayer.Dealer
{
   public class dbDealer
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbDealer(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
        }
        public long PostdbDealer(DealerClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@DealerId", obj.DealerId));
                DB.Parameters.Add(new SqlParameter("@DealerCode", obj.DealerCode == null ? "" : obj.DealerCode));
                DB.Parameters.Add(new SqlParameter("@DealerName", obj.DealerName == null ? DBNull.Value.ToString() : obj.DealerName.Trim()));
                DB.Parameters.Add(new SqlParameter("@DealerDesc", obj.DealerDesc == null ? DBNull.Value.ToString() : obj.DealerDesc.Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactNo", obj.ContactNo));
                DB.Parameters.Add(new SqlParameter("@DealerAddress", obj.DealerAddress));
                DB.Parameters.Add(new SqlParameter("@CountryId", obj.CountryId));
                DB.Parameters.Add(new SqlParameter("@StateId", obj.StateId));
                DB.Parameters.Add(new SqlParameter("@CityId", obj.CityId));
                DB.Parameters.Add(new SqlParameter("@DealerPinCode", obj.DealerPinCode == "null" ? 0 : Convert.ToInt32(obj.DealerPinCode)));
                DB.Parameters.Add(new SqlParameter("@EmailId", obj.EmailId));
                DB.Parameters.Add(new SqlParameter("@Tin", obj.Tin == null ? DBNull.Value.ToString() : obj.Tin.Trim()));
                DB.Parameters.Add(new SqlParameter("@Pan", obj.Pan == null ? DBNull.Value.ToString() : obj.Pan.Trim()));
                DB.Parameters.Add(new SqlParameter("@Gst", obj.Gst == null ? DBNull.Value.ToString() : obj.Gst.Trim()));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("dbo.usp_Insert_Update_Dealer");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbDealer", ex.Message.ToString(), "PostdbDealer", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }

        public long PostdbDealerImage(DealerImage obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@DocumentId", obj.DocumentId));
                DB.Parameters.Add(new SqlParameter("@DocRefId", obj.DocRefId));
                DB.Parameters.Add(new SqlParameter("@DocRefType", obj.DocRefType));
                DB.Parameters.Add(new SqlParameter("@DocumentName", obj.ImgFileName));
                DB.Parameters.Add(new SqlParameter("@ImgPath", obj.ImgPath));
                DB.Parameters.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("dbo.usp_Insert_DealerImage");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbDealer", ex.Message.ToString(), "PostdbDealerImage", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
        public DataTable DealerDocumentDelete(DealerDocSearch obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@DocumentId", obj.DocumentId));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteDataTable("dbo.usp_Insert_DealerImage");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbDealer", ex.Message.ToString(), "DealerDocumentDelete", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
             return null;
            }
        }
        public DataSet Verify_Agreement_OTP(DealerAgreement obj)
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
                dbLogger.PostErrorLog("dbDealer", ex.Message.ToString(), "Verify_Agreement_OTP", 10001, "Admin", true);
                return null;
            }
        }
        public DataSet PostGenerateAgreementOTP(DealerAgreement obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Action", "dealer"));
                DB.Parameters.Add(new SqlParameter("@UserName", obj.Username));
                DB.Parameters.Add(new SqlParameter("@OTP", obj.Userotp));
                DB.Parameters.Add(new SqlParameter("@Type", obj.Type));
                return DB.ExecuteDataSet("usp_Insert_USER_OTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostGenerateClientOTP", 10001, "Admin", true);
                return null;
            }
        }
        public long PostdbDealerAgreement(DealerAgreement obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@AgreementId", obj.AgreementId));
                DB.Parameters.Add(new SqlParameter("@AgreementRefId", obj.AgreementRefId));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@SentBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("dbo.usp_Insert_Update_Agreement");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbDealer", ex.Message.ToString(), "PostdbDealerAgreement", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
             
            }
        }
    }
}
