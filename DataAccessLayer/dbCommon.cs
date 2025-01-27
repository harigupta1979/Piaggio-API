using AppConfig;
using DataAccessLayer.DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;
using Logger;
namespace DataAccessLayer
{
    public class dbCommon
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbCommon(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public DataTable DynamicQuery(string ActionType, string Condition, Nullable<Int64> LoginUserId = null)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@ActionType", ActionType));
                DB.Parameters.Add(new SqlParameter("@Condition", Condition));
                DB.Parameters.Add(new SqlParameter("@LoginUserId", LoginUserId));
                return DB.ExecuteDataTable("usp_DynamicQuery");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCommon", ex.Message.ToString(), "DynamicQuery", 10001, "Dealer", true);
                return null;
            }
        }

        public DataTable DynamicSMTP()
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@Action", "GetCred"));
                return DB.ExecuteDataTable("usp_DynamicSMTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCommon", ex.Message.ToString(), "DynamicSMTP", 10001, "Common", true);
                return null;
            }
        }
        public DataTable UpdateSMTP(string SMTPId)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@SMTPId", SMTPId));
                DB.Parameters.Add(new SqlParameter("@Action", "UpdateSMTP"));
                return DB.ExecuteDataTable("usp_DynamicSMTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCommon", ex.Message.ToString(), "UpdateSMTP", 10001, "Common", true);
                return null;
            }
        }
        public int DeleteMaster(string Id, string Type)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@Id", Id));
                DB.Parameters.Add(new SqlParameter("@Type", Type));
                return DB.ExecuteNonQuery("usp_delete_master");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCommon", ex.Message.ToString(), "DeleteMaster", 10001, "Dealer", true);
                return 0;
            }
        }
        public long PdfScheduler(string DealerId, string RefId, string ReqType, string CreatedBy)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@DealerId", DealerId));
                DB.Parameters.Add(new SqlParameter("@RefId", RefId));
                DB.Parameters.Add(new SqlParameter("@ReqType", ReqType));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", CreatedBy));
                DB.Parameters.Add(new SqlParameter("@Action", "Insert"));
                return DB.ExecuteNonQuery("usp_PdfScheduler");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCommon", ex.Message.ToString(), "DeleteMaster", 10001, "Dealer", true);
                return 0;
            }
        }

        public DataTable GetMenuRole(Core.Module.UserManagement.RoleMenu obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@SuperAdmin", obj.IsSuperAdmin));
                DB.Parameters.Add(new SqlParameter("@RoleId", obj.RoleId));
                DB.Parameters.Add(new SqlParameter("@Mode", obj.Mode));
                return DB.ExecuteDataTable("usp_Get_MenuRole");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCommon", ex.Message.ToString(), "GetMenuRole", 10001, "Dealer", true);
                return null;
            }
        }



    }


}
