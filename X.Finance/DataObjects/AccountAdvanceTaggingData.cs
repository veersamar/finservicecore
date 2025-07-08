using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Business.DataObjects
{
    public class AccountAdvanceTaggingData
    {
        [Required]
        public long DocId { get; set; }   

        [Required]
        public long RefDocID { get; set; }   // FK to Invoice AccountDocument

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
