using AppConfig;
using Core.Module;
using DataAccessLayer.DataAccess;
using Logger;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DbCustomer
    {
        readonly DbLogger dbLogger;
        private readonly AppConfig.IConfigManager _configuration;
        public DbCustomer(IConfigManager configuration)
        {
            this._configuration = configuration;
        }
        public long PostdbCustomer(CustomerMaster obj)
        {
            DBAccess DB = new DBAccess(this._configuration);
            try
            {
                DB.Parameters.Add(new SqlParameter("@CUSTOMER_ID", obj.CustomerId));
                DB.Parameters.Add(new SqlParameter("@TITLE_ID", obj.TitleId));
                DB.Parameters.Add(new SqlParameter("@CUSTOMER_TYPE", obj.CustomerTypeId));
                DB.Parameters.Add(new SqlParameter("@FIRST_NAME", obj.FirstName.Trim()));
                DB.Parameters.Add(new SqlParameter("@LAST_NAME", obj.LastName == null ? "" : obj.LastName.Trim()));
                DB.Parameters.Add(new SqlParameter("@EMAIL_ID", obj.EmailId.Trim()));
                DB.Parameters.Add(new SqlParameter("@MOBILE_NO", obj.MobileNo));
                DB.Parameters.Add(new SqlParameter("@ALTMOBILE_NO", obj.AltMobileNo));
                DB.Parameters.Add(new SqlParameter("@DOB", obj.Dob));
                DB.Parameters.Add(new SqlParameter("@ADDRESS", obj.Address == null ? "" : obj.Address.Trim()));
                DB.Parameters.Add(new SqlParameter("@OCCUPATION_ID", obj.OccupationId));
                DB.Parameters.Add(new SqlParameter("@LANGUAGE_ID", obj.LanguageId));
                DB.Parameters.Add(new SqlParameter("@COUNTRY_ID", obj.CountryId));
                DB.Parameters.Add(new SqlParameter("@STATE_ID", obj.StateId));
                DB.Parameters.Add(new SqlParameter("@CITY_ID", obj.CityId));
                DB.Parameters.Add(new SqlParameter("@PINCODE", obj.PinCode));
                DB.Parameters.Add(new SqlParameter("@PAN_NO", obj.PanNo));
                DB.Parameters.Add(new SqlParameter("@AADHAR_NO", obj.AadharNo));
                DB.Parameters.Add(new SqlParameter("@GST_NO", obj.GstNo));
                //@CUSTOMER_ID_OUT
                DB.Parameters.Add(new SqlParameter("@ISACTIVE", obj.IsActive == null ? false : true));
                DB.Parameters.Add(new SqlParameter("@CreatedBy", obj.CreatedBy));
                DB.Parameters.Add(new SqlParameter("@UpdatedBy", obj.UpdatedBy));
                DB.Parameters.Add(new SqlParameter("@IpAddress", obj.IpAddress));
                DB.Parameters.Add(new SqlParameter("@Action", obj.Action.ToLower()));
                SqlParameter outPutVal = new SqlParameter("@CUSTOMER_ID_OUT", SqlDbType.Int);
                outPutVal.Direction = ParameterDirection.Output;
                DB.Parameters.Add(outPutVal);
                return DB.ExecuteScalar("usp_Insert_Update_Customer");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DbCustomer", ex.Message.ToString(), "PostdbCustomer", 10001, obj.CreatedBy == null ? (obj.UpdatedBy == null ? null : Convert.ToString(obj.UpdatedBy)) : Convert.ToString(obj.CreatedBy), true);
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    return -99;
                }
                return 0;
            }
        }
    }
}
