using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using X.Finance.Component.Entities;
using X.Finance.Data.Data;

namespace X.Finance.Data.Repositories
{
    public class AccountDocumentRepository : IAccountDocumentRepository
    {
        private readonly AccountDbContext _context;

        public AccountDocumentRepository(AccountDbContext context)
        {
            _context = context;
        }

        public async Task<AccountDocument> GetByIdAsync(long id)
        {
            return await _context.AccountDocuments
                .Include(doc => doc.AccountLines)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<long> CreateAsync(AccountDocument document)
        {
            _context.AccountDocuments.Add(document);
            await SaveChangesAsync();
            return document.Id;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<AccountOutstanding> GetOutstandingByDocIdAsync(long docId)
        {
            return await _context.AccountOutstandings
                .FirstOrDefaultAsync(o => o.DocId == docId);
        }

        public async Task CreateAdvanceAsync(AccountAdvance advance)
        {
            _context.AccountAdvances.Add(advance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOutstandingAsync(AccountOutstanding outstanding)
        {
            _context.AccountOutstandings.Update(outstanding);
            await _context.SaveChangesAsync();
        }
    }
}