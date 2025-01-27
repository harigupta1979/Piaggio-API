using AppConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class DBAccess
    {
        private readonly AppConfig.IConfigManager _configuration;
        private IDbCommand cmd = new SqlCommand();
        private string strConnectionString = "";
        private bool handleErrors = false;
        private string strLastError = "";
        public DBAccess(IConfigManager configuration)
        {
            this._configuration = configuration;
            strConnectionString = configuration.GetConnectionString("ICPLDatabase").ToString();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = strConnectionString;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
        }

        private void Open()
        {
            cmd.Connection.Open();
        }

        private void Close()
        {
            cmd.Connection.Close();
        }


        public DataSet ExecuteDataSet()
        {
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)cmd;
                ds = new DataSet();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
           
            return ds;
        }
        public DataTable ExecuteDataTable()
        {
            SqlDataAdapter da = null;
            DataTable ds = null;
            try
            {
                da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)cmd;
                ds = new DataTable();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
           
            return ds;
        }

        public DataSet ExecuteDataSet(string commandtext)
        {
            DataSet ds = null;
            try
            {
                cmd.CommandText = commandtext;
                ds = this.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            
            return ds;
        }
        public DataTable ExecuteDataTable(string commandtext)
        {
            DataTable dt = null;
            try
            {
                cmd.CommandText = commandtext;
                dt = this.ExecuteDataTable();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
           
            return dt;
        }

        public DataSet ExecuteDataSetView(string commandtext)
        {
            DataSet ds = null;
            try
            {
                cmd.CommandText = commandtext;
                cmd.CommandType = CommandType.Text;
                ds = this.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
           
            return ds;
        }
        public string ExecuteQuerySingalResult(string commandtext)
        {
            string value = "";
            SqlConnection cnn = new SqlConnection();
            try
            {
                SqlCommand cmdd = new SqlCommand();
                cmdd.CommandText = commandtext;
                cmdd.CommandType = CommandType.Text;

                strConnectionString = this._configuration.GetConnectionString("ICPLDatabase").ToString();
               
                cnn.ConnectionString = strConnectionString;
                cmdd.Connection = cnn;
                cnn.Open();
                SqlDataReader reader = cmdd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    value = reader[0].ToString();
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            finally{
                cnn.Close();
            }
          
            return value;
        }


        public System.Data.IDataParameterCollection Parameters
        {
            get
            {
                return cmd.Parameters;
            }
        }


        public int ExecuteNonQuery(string commandtext)
        {
            int i = -1;
            try
            {
                cmd.CommandText = commandtext;
                i = this.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
          
            return i;
        }

        public int ExecuteNonQuery()
        {
            int i = -1;
            try
            {
                this.Open();
                i = cmd.ExecuteNonQuery();
                this.Close();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            finally
            {
                this.Close();
            }

            return i;
        }
        public int ExecuteScalar()
        {
            int i = -1;
            try
            {
                this.Open();
                i = (int)cmd.ExecuteScalar();
                this.Close();
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            finally
            {
                this.Close();
            }
            return i;
        }


        public int ExecuteNonQueryWithTrans(string commandtext, SqlTransaction tr)
        {
            int i = -1;

            SqlCommand cmd1;

            try
            {
                if (tr != null)
                {
                    cmd1 = new SqlCommand(commandtext, tr.Connection);
                    cmd1.Transaction = tr;
                    i = cmd1.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            

            return i;
        }



        public long ExecuteScalar(string commandtext)
        {
            long i = -1;
            try
            {
                this.Open();
                cmd.CommandText = commandtext;
                object obj = cmd.ExecuteScalar();
                if (obj == null) {
                    return 0;
                }
                i = Convert.ToInt64(obj.ToString());
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            finally
            {
                this.Close();
            }
            return i;
        }
        public object Trn_ExecuteScalar(string commandtext)
        {
            object obj = null;
            try
            {
                this.Open();
                cmd.CommandText = commandtext;
                obj = cmd.ExecuteScalar();
                //string[] result = obj.ToString().Split(new char[] { ',' });
                //i = Convert.ToInt64(obj.ToString());
            }
            catch (Exception ex)
            {
                if (handleErrors)
                    strLastError = ex.Message;
                else
                    throw;
            }
            finally
            {
                this.Close();
            }
            return obj;
        }
    }
}
