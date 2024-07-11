using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class RoleApplication:IRoleApplication
    {
        private readonly IRoleRepository _roleRepository;

        public RoleApplication(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public OperationResult Create(CreateRole command)
        {
            var operation=new OperationResult();
            if (_roleRepository.Exist(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var role = new Role(command.Name);
            _roleRepository.Create(role);
            _roleRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditRole command)
        {
            var operation= new OperationResult();
            var role = _roleRepository.Get(command.Id);
            if (role == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);
            if (role.Name == command.Name && role.Id != command.Id)
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            role.Edit(command.Name);
            _roleRepository.SaveChanges();
            return operation.Succedded();

        }

        public List<RoleViewModel> List()
        {
            return _roleRepository.List();
        }

        public EditRole GetDetails(long id)
        {
            return _roleRepository.GetDetails(id);
        }
    }
}
