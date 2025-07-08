using X.Finance.Component.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Business.DataObjects
{
    public class AccountLineData
    {
        public long Id { get; set; }
        public long DocId { get; set; }

        [Required]
        public long AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }  // Always positive input, sign handled in service
                
        public DateTime DocDate { get; set; }

        public long BusinessUnitId { get; set; }

        public long CompanyId { get; set; }

        public long EntityType { get; set; }

        public bool IsHeader { get; set; }

        public string PostingType { get; set; }

        public short AccountType { get; set; }

        public short LedgerType { get; set; }

        public long? ContactId { get; set; }

        public long TaxAuthorityId { get; set; }

        [MaxLength(500)]
        public string Narration { get; set; }
    }
}
