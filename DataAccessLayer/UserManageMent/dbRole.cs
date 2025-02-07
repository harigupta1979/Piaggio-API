using AppConfig;
using Core.Module.UserManagement;
using Core.Module.UserManageMent;
using DataAccessLayer.DataAccess;
using Logger;
using System;
using System.Data.SqlClient;


namespace DataAccessLayer.UserManagement
{
    public class dbRole
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbRole(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public long PostdbRole(RoleClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@RoleId", obj.RoleId));
                DB.Parameters.Add(new SqlParameter("@RoleName", obj.RoleName==null?"": obj.RoleName.Trim()));
                DB.Parameters.Add(new SqlParameter("@RoleDesc", obj.RoleDesc==null?"":obj.RoleDesc.Trim()));
                DB.Parameters.Add(new SqlParameter("@RoleParentId", obj.RoleParentId)); 
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("usp_Insert_Update_Role");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbRole", ex.Message.ToString(), "PostdbRole", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }

        public long PostdbRoleMenuMappinging(RoleMenuMappingingClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@RoleId", obj.RoleId));
                DB.Parameters.Add(new SqlParameter("@MenuId", obj.MenuId));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("usp_Insert_Update_RoleMenuMappingingMaster");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbRole", ex.Message.ToString(), "PostdbRoleMenuMappinging", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
    }
}
