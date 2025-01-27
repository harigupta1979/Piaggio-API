using AppConfig;
using Core.Module.Onboard;
using DataAccessLayer.DataAccess;
using System;
using System.Data.SqlClient;
using Logger;
using System.Data;
using Core.Module.Dealer;

namespace DataAccessLayer.Onboard
{
    public class dbOnboard
    {
        DbLogger dbLogger; readonly string strCon = "";
        private readonly AppConfig.IConfigManager _configuration;
        public dbOnboard(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            strCon = configuration.GetConnectionString("ICPLDatabase").ToString();
        }
        public long PostdbOnboard(OnboardClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@OnboardId", obj.OnBoardId));
                DB.Parameters.Add(new SqlParameter("@Name", obj.OnBoardName == null ? "" : obj.OnBoardName.Trim()));
                DB.Parameters.Add(new SqlParameter("@Address", obj.Address==null?"":obj.Address.Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactNo", obj.MobileNo));
                DB.Parameters.Add(new SqlParameter("@EmailId", obj.EmailId.Trim()));
                DB.Parameters.Add(new SqlParameter("@CountryId", obj.CountryId));
                DB.Parameters.Add(new SqlParameter("@StateId", obj.StateId));
                DB.Parameters.Add(new SqlParameter("@CityId", obj.CityId));
                DB.Parameters.Add(new SqlParameter("@PinCode", obj.PinCode));
                DB.Parameters.Add(new SqlParameter("@GSTNumber", obj.GSTNumber == null ? "" : obj.GSTNumber.Trim()));
                DB.Parameters.Add(new SqlParameter("@PANNumber", obj.PANNumber == null ? "" : obj.PANNumber.Trim()));
                DB.Parameters.Add(new SqlParameter("@TANNumber", obj.TANNumber == null ? "" : obj.TANNumber.Trim()));
                if (obj.OnBoardAction == "Client" && obj.Action == "update")
                {
                    DB.Parameters.Add(new SqlParameter("@IsCustomerSubmited", obj.IsCustomerSubmited == null ? false : obj.IsCustomerSubmited));
                }
                DB.Parameters.Add(new SqlParameter("@OnBoardAction", obj.OnBoardAction));
                DB.Parameters.Add(new SqlParameter("@BankId", obj.BankId));
                DB.Parameters.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
                DB.Parameters.Add(new SqlParameter("@AccountName", obj.AccountName));
                DB.Parameters.Add(new SqlParameter("@Structure", obj.StructureName));
                DB.Parameters.Add(new SqlParameter("@AccountNo", obj.AccountNo));
                DB.Parameters.Add(new SqlParameter("@IFSCCode", obj.IFSCCode));
                DB.Parameters.Add(new SqlParameter("@ApprovalRemarks", obj.ApprovalRemarks == null ? "" : obj.ApprovalRemarks.Trim())); 
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress)); 
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                DB.Parameters.Add(new SqlParameter("@StructureId", obj.StructureId));
                DB.Parameters.Add(new SqlParameter("@CompanyTypeId", obj.CompanyTypeId));
                DB.Parameters.Add(new SqlParameter("@ContactPersonName", obj.ContactPersonName==null?"":obj.ContactPersonName.Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactPersonMobileNo", obj.ContactPersonMobileNo==null?"":obj.ContactPersonMobileNo.Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactPersonEmailId", obj.ContactPersonEmailId==null?"": obj.ContactPersonEmailId.Trim()));
                DB.Parameters.Add(new SqlParameter("@IsPan", obj.IsPan));
                DB.Parameters.Add(new SqlParameter("@IsTin", obj.IsTin));
                DB.Parameters.Add(new SqlParameter("@IsGst", obj.IsGst));
                DB.Parameters.Add(new SqlParameter("@AuthorisedPerson", obj.AuthorisedPerson == null ? "" : obj.AuthorisedPerson.Trim()));
                DB.Parameters.Add(new SqlParameter("@IsPanDeclare", obj.IsPanDeclare));
                DB.Parameters.Add(new SqlParameter("@IsTinDeclare", obj.IsTinDeclare));
                DB.Parameters.Add(new SqlParameter("@IsGstDeclare", obj.IsGstDeclare));
                DB.Parameters.Add(new SqlParameter("@IsLowTDS", obj.IsLowTDS));
                return DB.ExecuteScalar("usp_Insert_Update_Onboard");

            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbOnboard", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
        public long PostdbOnboardDataMigration(DataRow dr, OnboardClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@OnboardId", 0));
                DB.Parameters.Add(new SqlParameter("@Name", dr["Marketing Firm Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@Address", dr["Address"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactNo", dr["Mobile No"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@EmailId", dr["Email Id"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@CountryName", dr["Country Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@StateName", dr["State Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@CityName", dr["City Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@PinCode", dr["Pin Code"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@GSTNumber", dr["Gst No"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@PANNumber", dr["PAN Number"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@TANNumber", dr["TAN Number"].ToString().Trim()));
                //DB.Parameters.Add(new SqlParameter("@OnBoardAction", obj.OnBoardAction));
                DB.Parameters.Add(new SqlParameter("@BankName", dr["Bank Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@AccountType", dr["Account Type"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@AccountName", dr["Account Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@AccountNo", dr["Account No"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@IFSCCode", dr["IFSC Code"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@CompanyType", dr["Company Type"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactPersonName", dr["Contact Person Name"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactPersonMobileNo", dr["Contact Person Mobile No"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@ContactPersonEmailId", dr["Contact Person Email Id"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@AuthorisedPerson", dr["Authorised Person"].ToString().Trim()));
                //DB.Parameters.Add(new SqlParameter("@Structure", dr["Structure"].ToString().Trim()));
                DB.Parameters.Add(new SqlParameter("@DealerCode", dr["Dealer Code"].ToString().Trim()));
                return DB.ExecuteScalar("usp_Insert_Update_Onboard_DataMigration");

            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbOnboardDataMigration", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
        public long PostdbOnboarDocument(BusinessPartnerDocumentClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@DocumentId", obj.DocumentId));
                DB.Parameters.Add(new SqlParameter("@OnBoardId", obj.OnBoardId));
                DB.Parameters.Add(new SqlParameter("@DocumentName", obj.FileName));
                DB.Parameters.Add(new SqlParameter("@FileSize", obj.FileSize));
                DB.Parameters.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
                DB.Parameters.Add(new SqlParameter("@FileExtension", obj.FileExtension));
                DB.Parameters.Add(new SqlParameter("@MongodbId", obj.MongodbId));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy == null ? DBNull.Value : obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy == null ? DBNull.Value : obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("usp_Insert_Update_OnboardDocument");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbOnboarDocument", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
        public long PostTDSDocumentUpload(TDSDocumentClass obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@LowtdsId", obj.LowtdsId));
                DB.Parameters.Add(new SqlParameter("@OnBoardId", obj.OnBoardId));
                DB.Parameters.Add(new SqlParameter("@TDSFileName", obj.TDSFileName));
                DB.Parameters.Add(new SqlParameter("@DealerId", obj.DealerId));
                DB.Parameters.Add(new SqlParameter("@Extension", obj.FileExtension));
                DB.Parameters.Add(new SqlParameter("@FromDate", obj.FromDate));
                DB.Parameters.Add(new SqlParameter("@ToDate", obj.ToDate));
                DB.Parameters.Add(new SqlParameter("@Rate", obj.Rate));
              //  DB.Parameters.Add(new SqlParameter("@Extension", obj.IsLowTDS));
                DB.Parameters.Add(new SqlParameter("@IsActive", obj.IsActive));
                DB.Parameters.Add(new SqlParameter("@MongodbId", obj.MongodbId));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy == null ? DBNull.Value : obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy == null ? DBNull.Value : obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("usp_Insert_Update_LowTDS");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostTDSDocumentUpload", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
        //public long PostdbOnboardApproval(OnboardClass obj)
        //{
        //    DBAccess DB = new DBAccess(this._configuration);
        //    try
        //    {
        //        DB.Parameters.Add(new SqlParameter("@OnboradId", obj.OnBoardId));
        //        //DB.Parameters.Add(new SqlParameter("@MaxiHierarchyLevel", obj.MaximumHierarchyLevel));
        //        //DB.Parameters.Add(new SqlParameter("@TotalUsers", obj.TotalUsers));
        //        DB.Parameters.Add(new SqlParameter("@AccessModule", obj.AccessModule));
        //        //DB.Parameters.Add(new SqlParameter("@UserPrefix", obj.UserPrefix));
        //        DB.Parameters.Add(new SqlParameter("@ApprovedBy", obj.ApprovedBy));
        //        DB.Parameters.Add(new SqlParameter("@ApprovalRemarks", obj.ApprovalRemarks));
        //        DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.CreatedBy == null ? DBNull.Value : obj.CreatedBy));
        //        DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
        //        return DB.ExecuteScalar("usp_OnBoardApprove");
        //    }
        //    catch (Exception ex)
        //    {
        //        dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbOnboardApproval", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
        //        if (ex.Message.Contains("UNIQUE KEY"))
        //        {
        //            //throw ex;
        //            return -99;
        //        }
        //        return 0;
        //    }
        //}

        public int PostdbBulkOnboard(DataTable empData, string tablename = "TempOnboardUpload")
        {
            try
            {
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
                            bulkCopy.ColumnMappings.Add("Marketing Firm Name", "Marketing Firm Name");
                            bulkCopy.ColumnMappings.Add("Mobile No", "Mobile No");
                            bulkCopy.ColumnMappings.Add("Email Id", "Email Id");
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
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbBulkOnboard", 10001, "Dealer", true);
                return 0;
            }
        }
        public DataSet GetDealercode(string OnBoardId )
        {
            try
            {

                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@OnBoardId", OnBoardId));
                DB.Parameters.Add(new SqlParameter("@Action", "GetDealerCode"));
                return DB.ExecuteDataSet("[usp_GetDealerCode]");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "GetDealercode", 10001, "Admin", true);
                return null;
            }

        }

        public int PostdbBulkOnboadDataMigration(DataTable empData, string tablename = "TempDataMigration")
        {
            try
            {
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
                            bulkCopy.ColumnMappings.Add("Company Type", "Company Type");
                            bulkCopy.ColumnMappings.Add("Marketing Firm Name", "Marketing Firm Name");
                            bulkCopy.ColumnMappings.Add("Mobile No", "Mobile No");
                            bulkCopy.ColumnMappings.Add("Email Id", "Email Id");
                            bulkCopy.ColumnMappings.Add("Pin Code", "Pin Code");
                            bulkCopy.ColumnMappings.Add("Country Name", "Country Name");
                            bulkCopy.ColumnMappings.Add("State Name", "State Name");
                            bulkCopy.ColumnMappings.Add("City Name", "City Name");
                            bulkCopy.ColumnMappings.Add("Address", "Address");
                            bulkCopy.ColumnMappings.Add("Contact Person Name", "Contact Person Name");
                            bulkCopy.ColumnMappings.Add("Contact Person Mobile No", "Contact Person Mobile No");
                            bulkCopy.ColumnMappings.Add("Contact Person Email Id", "Contact Person Email Id");
                            bulkCopy.ColumnMappings.Add("Authorised Person", "Authorised Person");
                            bulkCopy.ColumnMappings.Add("Gst No", "Gst No");
                            bulkCopy.ColumnMappings.Add("PAN Number", "PAN Number");
                            bulkCopy.ColumnMappings.Add("TAN Number", "TAN Number");
                            bulkCopy.ColumnMappings.Add("Bank Name", "Bank Name");
                            bulkCopy.ColumnMappings.Add("Account Type", "Account Type");
                            bulkCopy.ColumnMappings.Add("Account Name", "Account Name");
                            bulkCopy.ColumnMappings.Add("Account No", "Account No");
                            bulkCopy.ColumnMappings.Add("IFSC Code", "IFSC Code");
                            //bulkCopy.ColumnMappings.Add("Structure", "Structure");
                            bulkCopy.ColumnMappings.Add("Dealer Code", "Dealer Code");
                            bulkCopy.ColumnMappings.Add("Agreement Start Date", "Agreement Start Date");
                            bulkCopy.ColumnMappings.Add("Agreement End Date", "Agreement End Date");
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
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbBulkOnboadDataMigration", 10001, "Admin", true);
                return 0;
            }
        }

        public DataSet PostdbOnboardUpload()
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                return DB.ExecuteDataSet("usp_PostOnboardUpload");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbOnboardUpload", 10001, "Admin", true);
                return null;
            }
        }

        public DataSet PostGenerateClientOTP(OnboardOtp obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Action", "onboard"));
                DB.Parameters.Add(new SqlParameter("@UserName", obj.Username));
                DB.Parameters.Add(new SqlParameter("@OTP", obj.Userotp));
                DB.Parameters.Add(new SqlParameter("@Type", obj.Type));
                return DB.ExecuteDataSet("usp_Insert_USER_OTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostGenerateClientOTP", 10001, "Admin", true);
                return null;
            }
        }
        public DataSet Verify_Client_OTP(OnboardOtp obj)
        {
            try
            {
                DBAccess DB = new DBAccess(this._configuration);
                DB.Parameters.Add(new SqlParameter("@Action", "select"));
                DB.Parameters.Add(new SqlParameter("@UserName", obj.Username));
                DB.Parameters.Add(new SqlParameter("@OTP", obj.Userotp));
                DB.Parameters.Add(new SqlParameter("@Type", obj.Type));
                DB.Parameters.Add(new SqlParameter("@Otpid", obj.Otpid));
                return DB.ExecuteDataSet("usp_Insert_USER_OTP");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "Verify_Client_OTP", 10001, "Admin", true);
                return null;
            }
        }
        public DataSet PostdbOnboadDataMigration()
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                return DB.ExecuteDataSet("usp_PostOnboardDataMigration");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbOnboadDataMigration", 10001, "Admin", true);
                return null;
            }
        }
        public long PostdbDealerAgreement(DealerAgreement obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@AgreementId", obj.AgreementId));
                DB.Parameters.Add(new SqlParameter("@AgreementRefId", obj.AgreementRefId));
                DB.Parameters.Add(new SqlParameter("@AgreementStartDate", obj.AgreementStartDate));
                DB.Parameters.Add(new SqlParameter("@AgreementEndDate", obj.AgreementEndDate));
                DB.Parameters.Add(new SqlParameter("@SentBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                return DB.ExecuteScalar("dbo.usp_Insert_Update_Agreement");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("dbOnboard", ex.Message.ToString(), "PostdbDealerAgreement", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                return 0;
            }
        }
    }
}