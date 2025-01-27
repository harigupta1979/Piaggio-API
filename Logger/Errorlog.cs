using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logger
{
    public partial class Errorlog
    {
     
        //Test - Dhanu  Test  --
        public long ID { get; set; }

        [Required]
        [StringLength(100)]
        public string FunctionName { get; set; }

        [Required]
        [StringLength(100)]
        public string ControllerName { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string ErrorMessage { get; set; }
       
        public string MailType { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ReferenceID { get; set; }
    }
}
