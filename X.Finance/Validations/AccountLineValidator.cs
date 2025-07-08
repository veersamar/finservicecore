using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using X.Finance.Business.DataObjects;

namespace X.Finance.Business.Validations
{
    /// <summary>
    /// For real-world finance, especially double-entry, the best practice is to Put complex business rule validations in your Service Layer — not the Controller
    /// Controllers should only check basic input shape (e.g. required fields, JSON format).
    /// </summary>
    public class AccountLineValidator
    {
        
    }
}
