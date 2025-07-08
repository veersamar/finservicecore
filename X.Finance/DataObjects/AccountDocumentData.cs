using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Business.DataObjects
{
    public class AccountDocumentData
    {
        public long Id { get; set; }

        [Required]
        public int DocNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string DocSeries { get; set; }

        [Required]
        [MaxLength(200)]
        public string DocName { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [Required]
        public List<AccountLineData> AccountLines { get; set; }
    }
}
