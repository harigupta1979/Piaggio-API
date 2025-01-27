using AppConfig;
using Core.Module;
using Core.Module.UserManagement;
using DataAccessLayer.DataAccess;
using Logger;
using System;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer.UserManagement
{
    public class dbUser
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbUser(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public long PostdbUser(UserClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@CountryId", obj.CountryId));
                DB.Parameters.Add(new SqlParameter("@StateId", obj.StateId));
                DB.Parameters.Add(new SqlParameter("@CityId", obj.CityId));
                DB.Parameters.Add(new SqlParameter("@UserId", obj.UserId));
                DB.Parameters.Add(new SqlParameter("@RoleId", obj.RoleId));
                DB.Parameters.Add(new SqlParameter("@FirstName", obj.FirstName == null ? "" : obj.FirstName.Trim()));
                DB.Parameters.Add(new SqlParameter("@LastName", obj.LastName == null ? "" : obj.LastName.Trim()));
                DB.Parameters.Add(new SqlParameter("@UserName", obj.UserName == null ? "" : obj.UserName.Trim()));
                DB.Parameters.Add(new SqlParameter("@EmailAddress", obj.EmailAddress == null ? "" : obj.EmailAddress.Trim()));
                DB.Parameters.Add(new SqlParameter("@Password", obj.Password == null ? "" : obj.Password.Trim()));
                DB.Parameters.Add(new SqlParameter("@Address", obj.Address == null ? "" : obj.Address.Trim()));
                DB.Parameters.Add(new SqlParameter("@PinCode", obj.PinCode));
                DB.Parameters.Add(new SqlParameter("@ContactNo", obj.ContactNo));
                DB.Parameters.Add(new SqlParameter("@DOB", obj.DOB));
                //DB.Parameters.Add(new SqlParameter("@ShowroomId", obj.ShowroomId == null ? "" : obj.ShowroomId.Trim()));
                DB.Parameters.Add(new SqlParameter("@DealerId", obj.DealerId));
                DB.Parameters.Add(new SqlParameter("@UserType", obj.UserType));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                DB.Parameters.Add(new SqlParameter("@DesignationId", obj.DesignationId));
                DB.Parameters.Add(new SqlParameter("@ReportingPersonId", obj.ReportingPersonId));
                DB.Parameters.Add(new SqlParameter("@IspwdChange", false));
                DB.Parameters.Add(new SqlParameter("@PwdExpireDays", obj.PwdExpireDays));
                DB.Parameters.Add(new SqlParameter("@IsPwdExpired", false));
                DB.Parameters.Add(new SqlParameter("@IsUserLocked", false));
                return DB.ExecuteScalar("usp_Insert_Update_User");

            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbUser", ex.Message.ToString(), "PostdbUser", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }

        public DataSet Get_UserProfileDetails(UserProfileClass obj)
        {
            try { 
            DBAccess DB = new DBAccess(this._configuration);
            DB.Parameters.Add(new SqlParameter("@UserId", obj.UserId));
            DB.Parameters.Add(new SqlParameter("@DOB", obj.DOB));
            DB.Parameters.Add(new SqlParameter("@Address", obj.Address == null ? "" : obj.Address.Trim()));
            DB.Parameters.Add(new SqlParameter("@CountryId", obj.CountryId));
            DB.Parameters.Add(new SqlParameter("@StateId", obj.StateId));
            DB.Parameters.Add(new SqlParameter("@CityId", obj.CityId));
            DB.Parameters.Add(new SqlParameter("@PinCode", obj.PinCode == null ? "" : obj.PinCode.Trim()));
            if (obj.IsUpdate)
            {
                DB.Parameters.Add(new SqlParameter("@Action", "update"));

            }
            else
            {
                DB.Parameters.Add(new SqlParameter("@Action", "select"));
            }
            return DB.ExecuteDataSet("usp_Get_User_ProfileDetails");
        }
               catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbUser", ex.Message.ToString(), "Get_UserProfileDetails", 10001, "Dealer", true);
                return null;
            }
        }
    }
}
