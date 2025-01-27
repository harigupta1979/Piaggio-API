using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.Master;
using DataAccessLayer;
using DataAccessLayer.Master;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.Master
{
   public class BCampaign
    {
        private readonly IConfigManager _configuration;
        dbCampaign dbcampaign; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objList;

        public BCampaign(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbcampaign = new dbCampaign(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostCampaign(CampaignClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                if (obj.CampaignId != null && obj.CampaignId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.CampaignId = 0; }
                var t1 = Task.Run(() => dbcampaign.PostdbCampaign(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
                else if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.CampaignId != null && obj.CampaignId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.CampaignId = 0; }
                    commonIUD.AdditionalParameter = "";
                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                }
                return commonIUD;
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BCampaign", ex.Message.ToString(), "PostCampaign", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GetCampaign(CampaignSearch obj)
        {
            objList = new CommonList();
            DataTable dt;
            dbCommon = new dbCommon(this._configuration);
            try
            {
                QueryBuilder queryBuilder = new QueryBuilder();

                var t1 = Task.Run(() => queryBuilder.BuildQuerySearch(obj));
                await Task.WhenAll(t1);
                string WhereCond = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                var t2 = Task.Run(() => dbCommon.DynamicQuery("campaign", WhereCond));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        objList.FinalMode = DBReturnGridRecord.RecordFound;
                        objList.Data = dt;
                        objList.Count = dt.Rows.Count;
                        objList.Message = "";
                        objList.AdditionalParameter = "";
                        return objList;
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BCampaign", ex.Message.ToString(), "GetCampaign", 10001, "Admin", true);
                return objList;
            }
        }
    }
}
