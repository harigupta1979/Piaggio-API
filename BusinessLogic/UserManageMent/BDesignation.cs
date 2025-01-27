using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.UserManagement;
using DataAccessLayer;
using DataAccessLayer.UserManagement;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.UserManagement
{
    public class BDesignation
    {
        private readonly IConfigManager _configuration;
        dbDesignation dbDesignation; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objList;

        public BDesignation(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbDesignation = new dbDesignation(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostDesignation(DesignationClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                if (obj.DesignationId != null && obj.DesignationId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.DesignationId = 0; }
                var t1 = Task.Run(() => dbDesignation.PostdbDesignation(obj));
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
                    if (obj.DesignationId != null && obj.DesignationId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.DesignationId = 0; }
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
                dbLogger.PostErrorLog("BDesignation", ex.Message.ToString(), "PostDesignation", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GetDesignation(DesignationSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("designation",WhereCond, obj.LoginUserId));
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
                dbLogger.PostErrorLog("BDesignation", ex.Message.ToString(), "GetDesignation", 10001, "Admin", true);
                return objList;
            }
        }
    }
}
