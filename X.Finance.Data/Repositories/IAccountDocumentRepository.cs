using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using X.Finance.Component.Entities;

namespace X.Finance.Data.Repositories
{
    public interface IAccountDocumentRepository
    {
        Task<AccountDocument> GetByIdAsync(long id);
        Task<long> CreateAsync(AccountDocument document);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
