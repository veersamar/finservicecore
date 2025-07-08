using X.Finance.Business.DataObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Finance.Business.Services
{
    public interface IAccountDocumentService
    {
        Task<long> CreateAccountDocumentAsync(AccountDocumentData dto);
    }
}
