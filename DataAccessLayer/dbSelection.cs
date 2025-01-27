using AppConfig;
using Core.Module;
using DataAccessLayer.DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;
using Logger;

namespace DataAccessLayer
{
    public class dbSelection
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbSelection(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public DataTable GetdbSelection(SelectionClass obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Condition", obj.Condition.ToLower()));
                DB.Parameters.Add(new SqlParameter("@FilterId", obj.FilterId));
                DB.Parameters.Add(new SqlParameter("@FilterId2", obj.FilterId2));
                DB.Parameters.Add(new SqlParameter("@FilterId3", obj.FilterId3));
                DB.Parameters.Add(new SqlParameter("@LoginUserId", obj.LoginUserId));
                return DB.ExecuteDataTable("usp_masterSelection");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "GetdbSelection", 10001, "Dealer", true);
                return null;
            }
        }
        public DataTable GetdbLocationByPinCode(PinCodeClass obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@PinCode", obj.PinCode.ToLower()));
                return DB.ExecuteDataTable("usp_GetLocationByPinCode");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "GetdbLocationByPinCode", 10001, "Dealer", true);
                return null;
            }
        }

        public DataTable GetRoleBaseAccessControl(SelectionClass obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@SubMenuName", obj.Condition));
                DB.Parameters.Add(new SqlParameter("@UserID", obj.FilterId));
                return DB.ExecuteDataTable("USP_RoleBaseAccessControll");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "GetRoleBaseAccessControl", 10001, "Dealer", true);
                return null;
            }
        }

        public DataTable GetRoleBaseAccessModule(SelectionClass obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Issuperadmin", obj.Condition.ToLower() == "true" ? 1 : 0));
                DB.Parameters.Add(new SqlParameter("@UserID", obj.FilterId));
                return DB.ExecuteDataTable("USP_GET_COMP_ACCESS_DETAILS_BYID");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "GetRoleBaseAccessModule", 10001, "Dealer", true);
                return null;
            }
        }

        public DataTable GetDynemicmenu(SelectionClass obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@SuperAdmin", (obj.Condition.ToLower() == "true" ? 1 : 0)));
                DB.Parameters.Add(new SqlParameter("@UserID", obj.FilterId));
                return DB.ExecuteDataTable("USP_DynamicManu");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "GetDynemicmenu", 10001, "Dealer", true);
                return null;
            }
        }
        public DataTable GetdbSelectionAutoCmt(SelectionClassAuto obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Condition", obj.Condition.ToLower()));
                DB.Parameters.Add(new SqlParameter("@FilterId3", obj.FilterId3));
                DB.Parameters.Add(new SqlParameter("@FilterId", obj.FilterId));
                DB.Parameters.Add(new SqlParameter("@FilterId2", obj.FilterId2));
                return DB.ExecuteDataTable("usp_selectionAutoComplete");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "GetdbSelectionAutoCmt", 10001, "Dealer", true);
                return null;
            }
        }

        public DataTable Get_Dropdown_Details(DropDowncommon _obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@condition", _obj.condition));
                DB.Parameters.Add(new SqlParameter("@type", _obj.Type));
                return DB.ExecuteDataTable("USP_GET_DropdownDetails");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbSelection", ex.Message.ToString(), "Get_Dropdown_Details", 10001, "Dealer", true);
                return null;
            }
        }
    }
}
