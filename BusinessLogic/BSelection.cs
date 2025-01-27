using AppConfig;
using Core.Module;
using DataAccessLayer;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BSelection
    {
        private readonly IConfigManager _configuration;
        dbSelection dbSelection; DbLogger dbLogger;
        CommonList objList;

        public BSelection(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbSelection = new dbSelection(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonList> GetSelection(SelectionClass obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbSelection.GetdbSelection(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "GeSelection", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonList> GetLocationByPinCode(PinCodeClass obj)
        {

            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbSelection.GetdbLocationByPinCode(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "GetLocationByPinCode", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetRoleBaseAccessControl(SelectionClass obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbSelection.GetRoleBaseAccessControl(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "GetRoleBaseAccessControl", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetRoleBaseAccessModule(SelectionClass obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t1 = Task.Run(() => dbSelection.GetRoleBaseAccessModule(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "GetRoleBaseAccessModule", 10001, "Admin", true);
                return objList;
            }
        }

        public async Task<CommonList> GetDynemicmenu(SelectionClass obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {
                var t2 = Task.Run(() => dbSelection.GetDynemicmenu(obj));
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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "GetDynemicmenu", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonList> GetSelectionAutoCmt(SelectionClassAuto obj)
        {
            objList = new CommonList();
            DataTable dt;
            try
            {

                var t1 = Task.Run(() => dbSelection.GetdbSelectionAutoCmt(obj));
                await Task.WhenAll(t1);
                dt = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "GetdbSelectionAutoCmt", 10001, "Admin", true);
                return objList;
            }
        }


        public async Task<CommonList> Get_Dropdown_Details(DropDowncommon obj)
        {
            objList = new CommonList();
            DataTable dt;

            try
            {

                var t2 = Task.Run(() => dbSelection.Get_Dropdown_Details(obj));
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
                dbLogger.PostErrorLog("BSelection", ex.Message.ToString(), "Get_Dropdown_Details", 10001, "Admin", true);
                return objList;


            }
        }

    }

}
