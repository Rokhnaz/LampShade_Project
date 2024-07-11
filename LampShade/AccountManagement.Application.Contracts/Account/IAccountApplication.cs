using _0_Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagement.Application.Contracts.Account
{
    public interface IAccountApplication
    {
        OperationResult Create(CreateAccount command);
        OperationResult Edit(EditAccount command);
        OperationResult ChangePassword(ChangePassword command);
        OperationResult Login(Login command);
        EditAccount GetDetails(long Id);
        List<AccountViewModel> Search(AccountSearchModel searchModel);
        void Logout();
    }
}
