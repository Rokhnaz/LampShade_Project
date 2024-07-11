using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagement.Application.Contracts.Account
{
    public class EditAccount:CreateAccount
    {
        public long Id { get; set; }
    }
}
