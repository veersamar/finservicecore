using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using X.Finance.Business.DataObjects;
using X.Finance.Data.Data;

namespace X.Finance.Business.Validations
{
    public class AccountDocumentValidator
    {
        private readonly AccountDbContext _context;

        public AccountDocumentValidator(AccountDbContext context)
        {
            _context = context;
        }
        public void ValidateDocument(List<AccountLineData> lines)
        {
            if (lines == null || lines.Count < 2)
                throw new InvalidOperationException("At least two lines are required.");

            var debitTotal = lines
                .Where(l => l.PostingType == "Debit")
                .Sum(l => Math.Abs(l.Amount));

            var creditTotal = lines
                .Where(l => l.PostingType == "Credit")
                .Sum(l => Math.Abs(l.Amount));

            if (debitTotal != creditTotal)
                throw new InvalidOperationException("Debit and Credit totals must equal.");

            // Example: check if ContactID is valid
            foreach (var line in lines)
            {
                if (line.AccountType == 1 && line.ContactId.HasValue)
                    throw new InvalidOperationException(
                        $"ContactID should not be set for AccountType {line.AccountType}.");
            }
        }
    }
}
