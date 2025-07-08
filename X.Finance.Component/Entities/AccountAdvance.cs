using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Component.Entities
{
    public class AccountAdvance
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long DocId { get; set; }   // FK to Receipt AccountDocument

        public long LineId { get; set; }   // FK to Receipt AccountLine
        public long ContactId { get; set; }
        public long AccountId { get; set; }

        [Required]
        public long RefDocId { get; set; }   // FK to Invoice AccountDocument
        public long RefLineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        // Navigation properties (optional, but good practice)
        //[ForeignKey("DocID")]
        public AccountDocument? AccountDocument { get; set; }

        //[ForeignKey("InvoiceDocID")]
        //public virtual AccountDocument RefDocument { get; set; }
    }
}
