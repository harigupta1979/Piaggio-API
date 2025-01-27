using Logger;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using AppConfig;
using BusinessLogic.Healper;
using DataAccessLayer.Master;
using Core.Module;
using DataAccessLayer;
using Core.Module.Master;

namespace BusinessLogic.Master
{
   public class BCampaignUpload
    {
        private readonly IConfigManager _configuration;
        dbCampaignUpload dbCampaignUpload; DbLogger dbLogger;
        CommonIUD commonIUD; dbCommon dbCommon; CommonList objListTable; CommonListDataSet objList;
        CommonList objList2;
        public BCampaignUpload(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbCampaignUpload = new dbCampaignUpload(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonListDataSet> CampaignUpload(CampaignUploadClass obj, string filePath, string fileExt, DataTable dtdata)
        {
            objList = new CommonListDataSet();
            DataSet dt = new DataSet();
            try
            {
                string Colums = string.Empty;
                int i = dbCampaignUpload.InsertbulkDynamicTempTable(dtdata);
                if (i == 1)
                {
                    var t2 = Task.Run(() => dbCampaignUpload.PostdbCampaignUpload(obj));
                    await Task.WhenAll(t2);
                    dt = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                    objList.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    objList.Data = dt;
                    objList.Count = dt.Tables.Count;
                    objList.Message = "";
                    objList.AdditionalParameter = "";
                    return objList;

                }
                return objList;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BCampaignUpload", ex.Message.ToString(), "CampaignUpload", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonIUD> CampaignUploadedit(CampaignUploadClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                 
                var t1 = Task.Run(() => dbCampaignUpload.PostdbCampaignUploadedit(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
               if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE;
                    
                    commonIUD.Recid = Recid;
                    
                        commonIUD.Message = "Data Updated Successfully!";
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


                return commonIUD;
            }
        }
        public async Task<CommonList>GetCampaignUpload(CampaignUploadSearch obj)
        {
            objList2 = new CommonList();
            DataTable dt2;
            dbCommon = new dbCommon(this._configuration);
            try
            {
                QueryBuilder queryBuilder = new QueryBuilder();

                var t1 = Task.Run(() => queryBuilder.BuildQuerySearch(obj));
                await Task.WhenAll(t1);
                string WhereCond = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                var t2 = Task.Run(() => dbCommon.DynamicQuery("Campaign", WhereCond));
                await Task.WhenAll(t2);
                dt2 = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (dt2 != null)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        objList2.FinalMode = DBReturnGridRecord.RecordFound;
                        objList2.Data = dt2;
                        objList2.Count = dt2.Rows.Count;
                        objList2.Message = "";
                        objList2.AdditionalParameter = "";
                        return objList2;
                    }
                }
                return objList2;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BCampaignUpload", ex.Message.ToString(), "GetCampaignUpload", 10001, "Admin", true);
                return objList2;
            }
        }
        public async Task<CommonIUD> PostImages(CampaignImagesClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                var fPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CampaignConatiner");
                int num = new Random().Next(1000, 9999);
                obj.FileName = obj.CampaignUploadNo.Replace("-", "_") + "_" + obj.Action+"_" + num + '.' + obj.FileExtension;
                obj.FilePath = fPath + "\\" + obj.FileName;
                File.WriteAllBytes(obj.FilePath, Convert.FromBase64String(obj.Base64.Replace("data:image/png;base64,", String.Empty)));
                var t1 = Task.Run(() => dbCampaignUpload.PostdbImages(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                return commonIUD;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BCampaignUpload", ex.Message.ToString(), "PostImages", 10001, "Admin", true);
                return commonIUD;
            }

        }
        public async Task<CommonIUD> PostUploadVedio(CampaignImagesClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {

                var t1 = Task.Run(() => dbCampaignUpload.PostdbImages(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;
                if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE;
                    commonIUD.Recid = Recid;
                    commonIUD.Message = "Data Inserted Successfully!";
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
                return commonIUD;
            }
        }
    }
}
