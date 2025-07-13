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
using X.Finance.Data.Repositories;

namespace X.Finance.Business.Services
{
    public class AccountDocumentService : IAccountDocumentService
    {
        private readonly IAccountDocumentRepository _documentRepository;
        private readonly AccountOutstandingService _outstandingService;
        private readonly AccountDocumentValidator _validator;

        public AccountDocumentService(
            IAccountDocumentRepository documentRepository, 
            AccountOutstandingService outstandingService,
            AccountDocumentValidator validator)
        {
            _documentRepository = documentRepository;
            _outstandingService = outstandingService;
            _validator = validator;
        }

        public async Task<long> CreateAccountDocumentAsync(AccountDocumentData document)
        {
            // Validate input using instance validator
            _validator.ValidateDocument(document.AccountLines);

            using var transaction = await _documentRepository.BeginTransactionAsync();

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

                var docId = await _documentRepository.CreateAsync(accountDocument);
                
                await _outstandingService.CreateOutstandingAsync(accountDocument, document.AccountLines);

                // Commit transaction
                await transaction.CommitAsync();

                return docId;
            }
            catch
            {
                // Optional explicit rollback (Dispose rolls back if not committed)
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
