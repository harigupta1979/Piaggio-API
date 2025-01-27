using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Module
{
    public class ReportClass
    {
        public Nullable<Int64> DealerId { get; set; }
        public Nullable<Int64> QuotationId { get; set; }
        public Nullable<Int64> ProposalId { get; set; }
        public Nullable<Int64> InvoiceId { get; set; }
        public string ReportName { get; set; }
        public Nullable<Int64> TrnsId { get; set; }
    }
}
