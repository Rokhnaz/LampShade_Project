using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AccountManagement.Application.Contracts.Role
{
    public interface IRoleApplication
    {
        public OperationResult Create(CreateRole command);
        public OperationResult Edit(EditRole command);
        public List<RoleViewModel> List();
        public EditRole GetDetails(long id);
    }
}
