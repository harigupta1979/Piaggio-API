using AppConfig;
using Core.Module.UserManageMent;
using DataAccessLayer.DataAccess;
using Logger;
using System;
using System.Data.SqlClient;

namespace DataAccessLayer.UserManageMent
{
   public class dbRate
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbRate(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public long PostdbRate(RateClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@RateId", obj.RateId));
                DB.Parameters.Add(new SqlParameter("@VideoId", obj.VideoId));
                DB.Parameters.Add(new SqlParameter("@CountryId", obj.CountryId));
                DB.Parameters.Add(new SqlParameter("@ZoneId", obj.ZoneId));
                DB.Parameters.Add(new SqlParameter("@StateId", obj.StateId));
                DB.Parameters.Add(new SqlParameter("@CityId", obj.CityId));
                DB.Parameters.Add(new SqlParameter("@VideoRate", obj.VideoRate));
                DB.Parameters.Add(new SqlParameter("@ImageRate", obj.ImageRate));
                DB.Parameters.Add(new SqlParameter("@ImageRateType", obj.ImageRateType));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("dbo.usp_Insert_Update_Rate");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbRate", ex.Message.ToString(), "PostdbRate", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
    }
}
