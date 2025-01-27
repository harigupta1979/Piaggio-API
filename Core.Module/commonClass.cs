using System;
using System.Data;

namespace Core.Module
{
    public class commonClass
    {
        public Nullable<Boolean> IsActive { get; set; }
        public Nullable<Int64> CreatedBy { get; set; }
        public Nullable<Int64> UpdatedBy { get; set; }
        public Nullable<Int64> SearchBy { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; } = "";

    }
    public class commonSearchClass
    {
        public Nullable<Boolean> IsActive { get; set; }
        public string RadioSearch { get; set; }
        public Nullable<Int64> SearchBy { get; set; }
        public Nullable<Int64> LoginUserId { get; set; }
    }

    public class CommonIUD
    {
        public string FinalMode;
        public long Recid;
        public string Message = "";
        public string AdditionalParameter = "";
    }
    public struct strct_DBInsertUpdateDelete
    {
        public DBReturnInsertUpdateDelete FinalMode;
        public string msgtext;
        public string Recid;
    }
    public struct DBReturnInsertUpdateDelete
    {
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string ERROR = "ERROR";
        public const string DUPLICATE = "DUPLICATE";
    }
    public class common_DB_TRN_IUD
    {
        public string FinalMode;
        public string msgtext;
        public long ncode;
        public string trnno;
    }
    public struct DBReturnGridRecord
    {
        public const string RecordFound = "DataFound";
        public const string RecordNotFound = "DataNotFound";
    }
    public class CommonList
    {
        public string FinalMode = DBReturnGridRecord.RecordNotFound;
        public int Count = 0;
        public DataTable Data = null;
        public string Message = "Record not Found";
        public string AdditionalParameter = "";
    }
    public class CommonListMongodb
    {
        public string FinalMode = DBReturnGridRecord.RecordNotFound;
        public int Count = 0;
        public dynamic Data = null;
        public string Message = "Record not Found";
        public string AdditionalParameter = "";
    }
    public class CommonListDataSet
    {
        public string FinalMode = DBReturnGridRecord.RecordNotFound;
        public int Count = 0;
        public string Message = "Record not Found";
        public string AdditionalParameter = "";
        public DataSet Data = null;
    }
    public class SelectionClass
    {
        public string Condition { get; set; }
        public Nullable<Int32> FilterId { get; set; }
        public Nullable<Int32> FilterId2 { get; set; }
        public string FilterId3 { get; set; }
        public Nullable<Int32> LoginUserId { get; set; }
    }

    public class DropDowncommon
    {
        public string condition { get; set; }
        public string Type { get; set; }
    }
    public class PinCodeClass
    {
        public string PinCode { get; set; }

    }
    public class SelectionClassAuto
    {
        public string Condition { get; set; }
        public Nullable<Int32> FilterId { get; set; }
        public Nullable<Int32> FilterId2 { get; set; }
        public string FilterId3 { get; set; }
    }
}

