using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.Onboard;
using DataAccessLayer;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Onboard;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;


namespace BusinessLogic.Onboard
{
    public class BBusinessPartnerDocument
    {
        private readonly IConfigManager _configuration;
        dbOnboard dbOnboard; DbLogger dbLogger;
        CommonIUD commonIUD; CommonList objList; dbCommon dbCommon;

        public BBusinessPartnerDocument(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbOnboard = new dbOnboard(this._configuration);
            dbLogger = new DbLogger(this._configuration);
        }
        public async Task<CommonList> GetOnbaordDocumentList(OnboardSearch obj)
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

                var t2 = Task.Run(() => dbCommon.DynamicQuery("onboarddocument", WhereCond));
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
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "GetOnbaordDocumentList", 10001, "Admin", true);
                return objList;
            }
        }
        public async Task<CommonIUD> PostOnbaordDocument(BusinessPartnerDocumentClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
               MongoDBCRUD mongo = new MongoDBCRUD(this._configuration);
                if (obj.DocumentId == null && obj.DocumentId == 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.DocumentId = 0; }
                var t1 = Task.Run(() => mongo.insert(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                obj.MongodbId = Recid;
                var t2 = Task.Run(() => dbOnboard.PostdbOnboarDocument(obj));
                await Task.WhenAll(t2);
                Recid = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.DocumentId != null && obj.DocumentId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Document Updated Successfully!"; } else { commonIUD.Message = "Document Uploaded Successfully!"; obj.DocumentId = 0; }
                    commonIUD.AdditionalParameter = "";
                    return commonIUD;
                }
                else
                {
                    //----------- Remove file in mongo
                    var t3 = Task.Run(() => mongo.delete(obj));
                    await Task.WhenAll(t3);
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                    return commonIUD;
                }
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "PostOnbaordDocument", 10001, "Admin", true);
                return commonIUD;
            }
        }
        public async Task<CommonListMongodb> GetOnbaordDocument(BusinessPartnerDocumentClass obj)
        {
            dynamic data;
            CommonListMongodb commonListMongodb = new CommonListMongodb();
            dbCommon = new dbCommon(this._configuration);
            try
            {
                MongoDBCRUD mongo = new MongoDBCRUD(this._configuration);
                var t2 = Task.Run(() => mongo.select(obj));
                await Task.WhenAll(t2);
                data = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (data != null)
                {
                    commonListMongodb.FinalMode = DBReturnGridRecord.RecordFound;
                    commonListMongodb.Data = data;
                    commonListMongodb.Count = 1;
                    commonListMongodb.Message = "";
                    commonListMongodb.AdditionalParameter = "";
                    return commonListMongodb;
                }
                return commonListMongodb;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "GetOnbaordDocument", 10001, "Admin", true);
                return commonListMongodb;
            }
        }
        public async Task<CommonIUD> GetOnbaordDocumentDelete(BusinessPartnerDocumentClass obj)
        {
            commonIUD = new CommonIUD(); dbCommon = new dbCommon(this._configuration);
            var Recid = (dynamic)null;
            try
            {
                MongoDBCRUD mongo = new MongoDBCRUD(this._configuration);

                //mongo.delete(obj);
                var t1 = Task.Run(() => mongo.delete(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;


                var t2 = Task.Run(() => dbCommon.DeleteMaster(obj.DocumentId.ToString(), "OnboardDocument"));
                await Task.WhenAll(t2);
                Recid = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.DocumentId != null && obj.DocumentId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.DocumentId = 0; }
                    commonIUD.AdditionalParameter = "";
                    return commonIUD;
                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                    return commonIUD;
                }
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "GetOnbaordDocumentDelete", 10001, "Admin", true);
                return commonIUD;
            }
        }

        public async Task<CommonIUD> TDSDocumentDelete(TDSDocumentClass obj)
        {
            commonIUD = new CommonIUD(); dbCommon = new dbCommon(this._configuration);
            var Recid = (dynamic)null;
            try
            {
                MongoDBCRUD mongo = new MongoDBCRUD(this._configuration);
                var t1 = Task.Run(() => mongo.TDSdelete(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;


                var t2 = Task.Run(() => dbCommon.DeleteMaster(obj.LowtdsId.ToString(), "tdsDocument"));
                await Task.WhenAll(t2);
                Recid = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;

                if (Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.LowtdsId != null && obj.LowtdsId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Data Updated Successfully!"; } else { commonIUD.Message = "Data Inserted Successfully!"; obj.LowtdsId = 0; }
                    commonIUD.AdditionalParameter = "";
                    return commonIUD;
                }
                else
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                    return commonIUD;
                }
            }
            catch (Exception ex)
            {
                commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    commonIUD.Message = "Cannot insert duplicate record.";
                }
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "TDSDocumentDelete", 10001, "Admin", true);
                return commonIUD;
            }
        }
        public async Task<CommonIUD> PostTDSDocumentUpload(TDSDocumentClass obj)
        {
            commonIUD = new CommonIUD();
            var Recid = (dynamic)null;
            try
            {
                MongoDBCRUD mongo = new MongoDBCRUD(this._configuration);
                if (obj.LowtdsId != null && obj.LowtdsId != 0) { obj.Action = "update"; } else { obj.Action = "insert"; obj.LowtdsId = 0; }
                var t1 = Task.Run(() => mongo.TDSinsert(obj));
                await Task.WhenAll(t1);
                Recid = t1.Status == TaskStatus.RanToCompletion ? t1.Result : null;

                obj.MongodbId = Recid;
                var t2 = Task.Run(() => dbOnboard.PostTDSDocumentUpload(obj));
                await Task.WhenAll(t2);
                Recid = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                if(Recid == -99)
                {
                    //----------- Remove file in mongo
                    var t3 = Task.Run(() => mongo.TDSdelete(obj));
                    await Task.WhenAll(t3);

                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.DUPLICATE;
                    commonIUD.Message = "Record already exists !";
                }
               else if(Recid != null && Recid != 0)
                {
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.INSERT;
                    if (obj.LowtdsId != null && obj.LowtdsId != 0) { commonIUD.FinalMode = DBReturnInsertUpdateDelete.UPDATE; }
                    commonIUD.Recid = Recid;
                    if (obj.Action == "update") { commonIUD.Message = "Document Updated Successfully!"; } else { commonIUD.Message = "Document Uploaded Successfully!"; obj.LowtdsId = 0; }
                    commonIUD.AdditionalParameter = "";
                   // return commonIUD;
                }
                else
                {
                    //----------- Remove file in mongo
                    var t3 = Task.Run(() => mongo.TDSdelete(obj));
                    await Task.WhenAll(t3);
                    commonIUD.FinalMode = DBReturnInsertUpdateDelete.ERROR;
                  //  return commonIUD;
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
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "PostTDSDocumentUpload", 10001, "Admin", true);
                return commonIUD;
            }
        }
        public async Task<CommonListMongodb> TDSDocumentDownload(TDSDocumentClass obj)
        {
            dynamic data;
            CommonListMongodb commonListMongodb = new CommonListMongodb();
            dbCommon = new dbCommon(this._configuration);
            try
            {
                MongoDBCRUD mongo = new MongoDBCRUD(this._configuration);
                var t2 = Task.Run(() => mongo.TDSselect(obj));
                await Task.WhenAll(t2);
                data = t2.Status == TaskStatus.RanToCompletion ? t2.Result : null;
                if (data != null)
                {
                    commonListMongodb.FinalMode = DBReturnGridRecord.RecordFound;
                    commonListMongodb.Data = data;
                    commonListMongodb.Count = 1;
                    commonListMongodb.Message = "";
                    commonListMongodb.AdditionalParameter = "";
                    return commonListMongodb;
                }
                return commonListMongodb;
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "TDSDocumentDownload", 10001, "Admin", true);
                return commonListMongodb;
            }
        }
        public async Task<CommonList> GetTDSDocumentlist(TDSDocumentClass obj)
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
                var t2 = Task.Run(() => dbCommon.DynamicQuery("tdsdocument", WhereCond));
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
                dbLogger.PostErrorLog("BBusinessPartnerDocument", ex.Message.ToString(), "GetTDSDocumentlist", 10001, "Admin", true);
                return objList;
            }
        }
    }
}
