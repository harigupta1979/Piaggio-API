using AppConfig;
using Core.Module.Master;
using DataAccessLayer.DataAccess;
using System;
using System.Data.SqlClient;
using Logger;

namespace DataAccessLayer.Master
{
   public class dbCampaign
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public dbCampaign(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public long PostdbCampaign(CampaignClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@CampaignId", obj.CampaignId));
                //DB.Parameters.Add(new SqlParameter("@CampaignName", obj.CampaignName.Trim()));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("usp_Insert_Update_Campaign");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCampaign", ex.Message.ToString(), "PostdbCampaign", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    //throw ex;
                    return -99;
                }
                return 0;
            }
        }
    }
}
