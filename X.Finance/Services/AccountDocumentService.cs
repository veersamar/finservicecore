using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using X.Finance.Business.DataObjects;
using X.Finance.Business.Validations;
using X.Finance.Component.Entities;
using X.Finance.Component.Enums;
using X.Finance.Data.Data;

namespace X.Finance.Business.Services
{
    public class AccountDocumentService : IAccountDocumentService
    {
        private readonly AccountDbContext _Context;
        private readonly AccountOutstandingService _OutstandingService;

        private readonly AccountDocumentValidator _Validator;

        public AccountDocumentService(AccountDbContext context, AccountDocumentValidator validator)
        {
            _Context = context;
            _OutstandingService = new AccountOutstandingService(_Context);
            _Validator = validator;
        }

        public async Task<long> CreateAccountDocumentAsync(AccountDocumentData document)
        {
            // Validate input using instance validator
            _Validator.ValidateDocument(document.AccountLines);

            using var transaction = await _Context.Database.BeginTransactionAsync();

            try
            {
                // Convert DTO -> Entity
                var accountDocument = new AccountDocument
                {
                    DocNo = document.DocNo,
                    DocSeries = document.DocSeries,
                    DocName = document.DocName,
                    DocDate = document.DocDate,
                    CompanyId = document.CompanyId,
                    AccountLines = document.AccountLines.Select(line => new AccountLine
                    {
                        DocDate = document.DocDate,
                        BusinessUnitId = line.BusinessUnitId,
                        CompanyId = line.CompanyId,
                        EntityType = line.EntityType,
                        IsHeader = line.IsHeader,
                        AccountId = line.AccountId,
                        ContactId = line.ContactId,
                        Narration = line.Narration,
                        PostingType = (int)Enum.Parse(typeof(PostingType), line.PostingType, ignoreCase: true),
                        Amount = (int)Enum.Parse(typeof(PostingType), line.PostingType, ignoreCase: true) == (int)PostingType.Debit
                            ? line.Amount
                            : -1 * line.Amount,
                        VersionNo = BitConverter.GetBytes(DateTime.UtcNow.Ticks) // Simple versioning
                    }).ToList()
                };

                // Validate balancing:
                var total = accountDocument.AccountLines.Sum(x => x.Amount);
                if (total != 0)
                    throw new InvalidOperationException("The document is unbalanced. Debits must equal credits.");

                _Context.AccountDocuments.Add(accountDocument);
                await _Context.SaveChangesAsync();

                await _OutstandingService.CreateOutstandingAsync(accountDocument, document.AccountLines);

                // Commit transaction
                await transaction.CommitAsync();

                return accountDocument.Id;
            }
            catch(Exception ex)
            {
                // Optional explicit rollback (Dispose rolls back if not committed)
                await transaction.RollbackAsync();

                throw;
            }            
        }
    }
}
