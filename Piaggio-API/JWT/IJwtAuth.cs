using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_Dealer_API.JWT
{
    public interface IJwtAuth
    {
        string AuthenticationToken(string username, string role, Core.Module.CommonList list);
    }
}
 