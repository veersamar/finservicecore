using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Component.Entities
{
    public class AccountDocument
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        public Int64 EntityType { get; set; }

        [Required]
        public Int64 DocType { get; set; }

        public int DocNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string DocSeries { get; set; }

        [Required]
        [MaxLength(200)]
        public string DocName { get; set; }

        public DateTime DocDate { get; set; }

        public long CompanyId { get; set; }

        [Timestamp]
        public byte[] VersionNo { get; set; }

        public required ICollection<AccountLine> AccountLines { get; set; }

    }
}
