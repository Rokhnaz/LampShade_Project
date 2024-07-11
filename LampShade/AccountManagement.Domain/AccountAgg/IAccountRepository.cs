using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;
using AccountManagement.Application.Contracts.Account;

namespace AccountManagement.Domain.AccountAgg
{
    public interface IAccountRepository:IRepository<long,Account>
    {
        Account GetBy(string username);
        EditAccount GetDetails(long Id);
        List<AccountViewModel> Search(AccountSearchModel searchModel);
    }
}
