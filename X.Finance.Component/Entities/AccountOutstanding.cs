using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Component.Entities
{
    public class AccountOutstanding
    {
        [Key]
        public long Id { get; set; }

        public long DocId { get; set; }         // FK to AccountDocument
        public long DocLineId { get; set; }         // FK to AccountDocument
        public long ContactId { get; set; }
        public long AccountId { get; set; }
        public decimal OpenAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DocDate { get; set; }
        public string DocType { get; set; } = string.Empty;
        public bool IsClosed { get; set; }

        // Optional navigation if you like
        public AccountDocument? AccountDocument { get; set; }
    }
}
