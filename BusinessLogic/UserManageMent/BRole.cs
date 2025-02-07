using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.UserManagement;
using Core.Module.UserManageMent;
using DataAccessLayer;
using DataAccessLayer.UserManagement;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic.UserManagement
{
    public class BRole
    {
        private readonly IConfigManager _configuration;
        dbRole dbRole; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objList;

        public BRole(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbRole = new dbRole(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonIUD> PostRole(RoleClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {

                if (obj.RoleId != null && obj.RoleId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.RoleId = 0; }
                var t1 = Task.Run(() => dbRole.PostdbRole(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
                else if (Recid != null && Recid != 0)
                {
                    //////obj.RoleId = Recid;
                    //var t2 = Task.Run(() => dbRole.PostdbRoleMenuMappinging(obj));
                    //await Task.WhenAll(t2);
                    //dynamic status = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.RoleId != null && obj.RoleId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.RoleId = 0; }
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
                dbLogger.PostErrorLog("BRole", ex.Message.ToString(), "PostRole", 10001, "Admin", true);
                return commonIUD;
            }
        }
        public async Task<CommonIUD> PostRoleMenuMappinging(RoleMenuMappingingClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {

                if (obj.RoleId != null && obj.RoleId != 0)
                {
                obj.Action = "insert"; 
                var t1 = Task.Run(() => dbRole.PostdbRoleMenuMappinging(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid == -99)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
                else if (Recid != null && Recid != 0)
                {
  
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; 
                    commonIUD.Recid = Recid;
                    commonIUD.Message = "Permission Updated Successfully!"; 
                    commonIUD.AdditionalParameter = "";
                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                }
                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                    commonIUD.Message = "Provide role !";
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
                dbLogger.PostErrorLog("BRole", ex.Message.ToString(), "PostRole", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonList> GetRole(RoleSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("role", WhereCond,obj.LoginUserId));
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
                dbLogger.PostErrorLog("BRole", ex.Message.ToString(), "GetRole", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetRoleMenu(RoleMenu obj)
        {
            objList = new CommonList();
            DataTable dt;
            dbCommon = new dbCommon(this._configuration);
            try
            {
                var t2 = Task.Run(() => dbCommon.GetMenuRole(obj));
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
                dbLogger.PostErrorLog("BRole", ex.Message.ToString(), "GetRoleMenu", 10001, "Admin", true);
                return objList;
            }
        }
    }
}
