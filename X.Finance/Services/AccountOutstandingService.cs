using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using X.Finance.Business.DataObjects;
using X.Finance.Component.Entities;
using X.Finance.Component.Enums;
using X.Finance.Data.Data;

namespace X.Finance.Business.Services
{
    public class AccountOutstandingService
    {
        private readonly AccountDbContext _context;

        public AccountOutstandingService(AccountDbContext context)
        {
            _context = context;
        }

        public async Task CreateOutstandingAsync(AccountDocument document, List<AccountLineData> accountLines)
        {
            // Automatically create Outstanding
            var totalCustomerAmount = accountLines
                .Where(l => l.PostingType == PostingType.Debit.ToString())
                .Sum(l => l.Amount);

            var contactLine = document.AccountLines.FirstOrDefault(l => l.ContactId.GetValueOrDefault() != 0);

            if (contactLine != null)
            {
                var outstanding = new AccountOutstanding
                {
                    DocId = document.Id,
                    DocLineId = contactLine.Id,
                    ContactId = contactLine.ContactId.GetValueOrDefault(),
                    AccountId = contactLine.AccountId,
                    TotalAmount = Math.Abs(totalCustomerAmount),
                    OpenAmount = Math.Abs(totalCustomerAmount),
                    DocDate = contactLine.DocDate,
                    DocType = "Receipt",
                    IsClosed = false
                };

                _context.AccountOutstandings.Add(outstanding);
                await _context.SaveChangesAsync();
            }
        }
    }
}
