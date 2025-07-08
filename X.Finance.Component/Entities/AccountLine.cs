using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Component.Entities
{
    public class AccountLine
    {
        [Key]
        public Int64 Id { get; set; }

        [ForeignKey(nameof(AccountDocument))]
        public long DocId { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        [Required]
        public long EntityType { get; set; }

        public int PostingType { get; set; }

        public bool IsHeader { get; set; }

        [Required]
        public long AccountId { get; set; }
                        
        public long? ContactId { get; set; }

        public long BusinessUnitId { get; set; }

        public long CompanyId { get; set; }        

        [MaxLength(500)]
        public string? Narration { get; set; }

        public decimal Amount { get; set; }

        [Timestamp]
        public required byte[] VersionNo { get; set; }

        // Navigation property
        public AccountDocument? AccountDocument { get; set; }
    }
}
