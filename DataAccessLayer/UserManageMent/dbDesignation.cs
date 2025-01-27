using AppConfig;
using Core.Module.UserManagement;
using DataAccessLayer.DataAccess;
using Logger;
using System;
using System.Data.SqlClient;


namespace DataAccessLayer.UserManagement
{
    public class dbDesignation
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbDesignation(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public long PostdbDesignation(DesignationClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@DesignationId", obj.DesignationId));
                DB.Parameters.Add(new SqlParameter("@DesignationName", obj.DesignationName==null?"": obj.DesignationName.Trim()));
                DB.Parameters.Add(new SqlParameter("@DesignationDesc", obj.DesignationDesc==null?"" : obj.DesignationDesc.Trim()));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("usp_Insert_Update_Designation");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbDesignation", ex.Message.ToString(), "PostdbDesignation", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
    }
}
