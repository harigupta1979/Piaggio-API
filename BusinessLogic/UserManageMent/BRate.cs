using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.UserManageMent;
using DataAccessLayer;
using DataAccessLayer.UserManageMent;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.UserManageMent
{
   public class BRate
    {
        private readonly IConfigManager _configuration;
        dbRate dbRate; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objList;

        public BRate(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbRate = new dbRate(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostRate(RateClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                if (obj.RateId != null && obj.RateId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.RateId = 0; }
                var t1 = Task.Run(() => dbRate.PostdbRate(obj));
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
                    if (obj.RateId != null && obj.RateId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.RateId = 0; }
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
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BRate", ex.Message.ToString(), "PostRate", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GetRate(RateSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("rate", WhereCond, obj.LoginUserId));
                await Task.WhenAll(t2);
                dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objList.FinalMode = DBReturnGridRecord.RecordFound;
                    objList.Data = dt;
                    objList.Count = dt.Rows.Count;
                    objList.Message = "";
                    objList.AdditionalParameter = "";
                    return objList;
                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BRate", ex.Message.ToString(), "GetRate", 10001, "Admin", true);
                return objList;
            }
        }
    }
}
