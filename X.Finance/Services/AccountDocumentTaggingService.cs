using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using X.Finance.Business.DataObjects;
using X.Finance.Component.Entities;
using X.Finance.Data.Data;

namespace X.Finance.Business.Services
{
    public class AccountDocumentTaggingService
    {
        private readonly AccountDbContext _context;

        public AccountDocumentTaggingService(AccountDbContext context)
        {
            _context = context;
        }

        // Pseudocode plan:
        // 1. Check if the AccountOutstanding.OpenAmount and AccountAdvanceTaggingData.Amount are being compared or assigned with mismatched types.
        // 2. Ensure all values used in calculations and assignments are explicitly cast to decimal where needed.
        // 3. If any LINQ or EF query is projecting to double, cast to decimal before use.

        public async Task TagToDocument(AccountAdvanceTaggingData data)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                long docId = data.DocId;
                long refDocId = data.RefDocID;
                decimal amount = data.Amount;

                var document = await _context.AccountOutstandings
                    .FirstOrDefaultAsync(o => o.DocId == docId);
                var refDocument = await _context.AccountOutstandings
                    .FirstOrDefaultAsync(o => o.DocId == refDocId);

                if (document == null)// || refDocument == null)
                    throw new InvalidOperationException("Receipt or Invoice not found.");

                // Explicitly cast OpenAmount to decimal if needed (should already be decimal, but for safety)
                decimal docOpenAmount = Convert.ToDecimal(document.OpenAmount);
                //decimal refDocOpenAmount = Convert.ToDecimal(refDocument.OpenAmount);

                if (docOpenAmount < amount)
                    throw new InvalidOperationException("Not enough open amount on Receipt.");

                //if (refDocOpenAmount < amount)
                //    throw new InvalidOperationException("Invoice already settled for this amount.");

                var application = new AccountAdvance
                {
                    DocId = document.DocId,
                    LineId = document.DocLineId,
                    ContactId = document.ContactId,
                    AccountId = document.AccountId,
                    RefDocId = refDocId,
                    Amount = amount,
                    CreationDate = DateTime.UtcNow
                };

                _context.AccountAdvances.Add(application);

                // Use decimal arithmetic
                document.OpenAmount = docOpenAmount - amount;
                //refDocument.OpenAmount = refDocOpenAmount - amount;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
