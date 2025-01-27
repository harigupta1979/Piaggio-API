

using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Module.Dealer
{
   public class DealerClass : commonClass
    {
        [Required(ErrorMessage = "Dealer Name is required")]
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string DealerDesc { get; set; }
        public Nullable<Int32> DealerId { get; set; }
        public string ContactNo { get; set; }
        public string DealerAddress { get; set; }
        public Nullable<Int32> CountryId { get; set; }
        public Nullable<Int32> StateId { get; set; }
        public Nullable<Int32> CityId { get; set; }
        public string DealerPinCode { get; set; }
        public string EmailId { get; set; }
        public string Tin { get; set; }
        public string Pan { get; set; }
        public string Gst { get; set; }
    }
   public class DealerSearch : commonSearchClass
    {
        public string DealerName { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
    }
    public class DealerImage : commonClass
    {
        public Nullable<Int64> DocumentId { get; set; }
        public Nullable<Int64> DocRefId { get; set; }
        public Nullable<Int64> DocumentTypeId { get; set; }
        public string ImgBase64 { get; set; }       
        public string DocRefType { get; set; }
        public string ImgFileName { get; set; }
        public string ImgPath { get; set; }
        public string FileSize { get; set; }
        public string FileExtension { get; set; }
    }
    public class DealerDocSearch : commonClass
    {
        public Nullable<Int64> DocRefId { get; set; }
        public Nullable<Int64> DocumentId { get; set; }
    }
    public class DealerAgreement : commonClass
    {
        public Nullable<Int64> AgreementId { get; set; }
        public Nullable<Int64> AgreementRefId { get; set; }
        public Nullable<Int64> SentBy { get; set; }
        public DateTime? AgreementStartDate { get; set; }
        public DateTime? AgreementEndDate { get; set; }
         public string Username { get; set; }
        public int Userotp { get; set; }
        public string Type { get; set; }
        public int Otpid { get; set; }
    }
    public class AgreementDoc
    {
        public Nullable<Int64> AgreementId { get; set; }
        public string DocType { get; set; }
    }
}
