using System;
using System.Data.SqlClient;
using Logger;
using System.Data;
using AppConfig;
using DataAccessLayer.DataAccess;
using Core.Module.Master;

namespace DataAccessLayer.Master
{
   public class dbCampaignUpload
    {
        DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        readonly string strCon = "";
        public dbCampaignUpload(IConfigManager configuration)
        {
            this._configuration = configuration;
            strCon = configuration.GetConnectionString("ICPLDatabase").ToString();
        }
        public int InsertbulkDynamicTempTable(DataTable empData, string tablename = "TempCampaignUpload")
        {
            try
            {
                try
                {
                    empData.Columns["Taxable amount\nMarketing Budget Amount"].ColumnName = "Marketing Budget Amount";
                }
                catch(Exception ex)
                {
                    dbLogger.PostErrorLog("dbCodbCampaignUploadmmon", ex.Message.ToString(), "InsertbulkDynamicTempTable", 10001, "Dealer", true);
                    return 0;

                }
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 100;
                        bulkCopy.DestinationTableName = tablename;
                        try
                        {


                            bulkCopy.ColumnMappings.Add("S.No.", "S.No.");
                            bulkCopy.ColumnMappings.Add("VUR No.", "VUR No.");
                            bulkCopy.ColumnMappings.Add("Month", "Month");
                            bulkCopy.ColumnMappings.Add("QMF Code", "QMF Code");

                            bulkCopy.ColumnMappings.Add("Dealer Code", "Dealer Code");
                            bulkCopy.ColumnMappings.Add("Exp Code", "Exp Code");
                            //bulkCopy.ColumnMappings.Add("Structure", "Structure");
                            bulkCopy.ColumnMappings.Add("Dealer Name", "Dealer Name");

                            bulkCopy.ColumnMappings.Add("Marketing Firm Name", "Marketing Firm Name");
                            bulkCopy.ColumnMappings.Add("Marketing Firm PAN", "Marketing Firm PAN");
                            bulkCopy.ColumnMappings.Add("Marketing Budget Amount", "Marketing Budget Amount");
                            bulkCopy.ColumnMappings.Add("GST", "GST");

                            bulkCopy.ColumnMappings.Add("TDS", "TDS");
                            bulkCopy.ColumnMappings.Add("Payable Amount", "Payable Amount");
                            bulkCopy.ColumnMappings.Add("Bank Name", "Bank Name");
                            bulkCopy.ColumnMappings.Add("Account Number", "Account Number");

                            bulkCopy.ColumnMappings.Add("IFSC", "IFSC");
                            bulkCopy.ColumnMappings.Add("GST Payment Status", "GST Payment Status");
                            bulkCopy.ColumnMappings.Add("Gst Utr", "Gst Utr");
                            bulkCopy.ColumnMappings.Add("Gst Date Payment", "Gst Date Payment");

                            bulkCopy.ColumnMappings.Add("Location", "Location");
                            bulkCopy.ColumnMappings.Add("State", "State");
                            bulkCopy.ColumnMappings.Add("Zone", "Zone");
                            bulkCopy.ColumnMappings.Add("Payment Type", "Payment Type");
                            //bulkCopy.ColumnMappings.Add("Image", "Image");
                            //bulkCopy.ColumnMappings.Add("Video", "Video");
                            //bulkCopy.ColumnMappings.Add("SMS", "SMS");
                            //bulkCopy.ColumnMappings.Add("Email", "Email");
                            bulkCopy.WriteToServer(empData);
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            connection.Close();

                        }
                        //finally { connection.Close(); }
                    }

                    transaction.Commit();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCodbCampaignUploadmmon", ex.Message.ToString(), "InsertbulkDynamicTempTable", 10001, "Dealer", true);
                return 0;
            }
        }
        public long PostdbCampaignUploadedit(CampaignUploadClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@CampaignUploadId", obj.CampaignUploadId));
                DB.Parameters.Add(new SqlParameter("@FromDate", obj.FromDate));
                DB.Parameters.Add(new SqlParameter("@ToDate", obj.ToDate));
                DB.Parameters.Add(new SqlParameter("@FileName", obj.CampaignName));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                  DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
           return DB.ExecuteScalar("dbo.usp_Update_CampaignUpload");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCampaignUpload", ex.Message.ToString(), "PostdbCampaignUploadedit", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
        return 0;
            }
        }
        public DataSet PostdbCampaignUpload(CampaignUploadClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@CampaignUploadId", obj.CampaignUploadId));
                DB.Parameters.Add(new SqlParameter("@FromDate", obj.FromDate));
                DB.Parameters.Add(new SqlParameter("@ToDate", obj.ToDate));
                DB.Parameters.Add(new SqlParameter("@FileName", obj.TempletName));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteDataSet("dbo.usp_PostCampaignUpload");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCampaignUpload", ex.Message.ToString(), "PostdbCampaignUpload", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
              return null;
            }
        }
        public long PostdbImages(CampaignImagesClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@CampaignUploadId", obj.CampaignUploadId));
                DB.Parameters.Add(new SqlParameter("@ImageName", obj.FileName));
                DB.Parameters.Add(new SqlParameter("@ImagePath", obj.FilePath));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action));
                return DB.ExecuteScalar("dbo.usp_Insert_CampaignUploadImage");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbCampaignUpload", ex.Message.ToString(), "PostdbImages", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
    }
}
