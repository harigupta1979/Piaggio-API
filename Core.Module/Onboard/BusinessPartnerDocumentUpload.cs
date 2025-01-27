using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Module.Onboard
{
    public class BusinessPartnerDocumentClass:commonClass
    {
        public Nullable<Int64> DocumentId { get; set; }
        public Nullable<Int64> OnBoardId { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string DocumentType { get; set; }
        public string DocumentBas64 { get; set; }
        public string MongodbId { get; set; }
        public Nullable<Int64> DocumentTypeId { get; set; }
    }
    public class OnboardView : commonSearchClass
    {
        public string MongoId { get; set; }

    }
    public class TDSDocumentClass : commonClass
    {
        public Nullable<Int64> LowtdsId { get; set; }
        public Nullable<Int64> OnBoardId { get; set; }
        public string FileSize { get; set; }
        public string TDSFileName { get; set; }
        public string FileExtension { get; set; }
        public string DocumentBas64 { get; set; }
        public string MongodbId { get; set; }
        public Nullable<Int64> DealerId { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public Nullable<double> Rate { get; set; }
        public string RoutUrl { get; set; }

    }
    public class TDSSearch : commonSearchClass
    {
        public Nullable<Int64> DealerId { get; set; }
    }
    public class TDSApproval : commonClass
    {
        public Nullable<Int64> DealerId { get; set; }
        public Nullable<Int64> LowtdsId { get; set; }
        public Nullable<Int64> OnboardId { get; set; }
    }
}
