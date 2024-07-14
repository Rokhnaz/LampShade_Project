using AccountManagement.Application.Contracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IFileUploder _fileUploder;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthHelper _authHelper;


        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
            IFileUploder fileUploder,IAuthHelper authHelper)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploder = fileUploder;
            _authHelper = authHelper;
        }

        public OperationResult Register(RegisterAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exist(x => x.UserName == command.UserName || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var password = _passwordHasher.Hash(command.Password);

            var Path = $"ProfilePhotos";
            var PhotoPath = _fileUploder.Upload(command.ProfilePhoto, Path);

            var account = new Account(command.FullName, command.UserName, password, command.Mobile,
                command.RoleId, PhotoPath);
            _accountRepository.Create(account);
            _accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_accountRepository.Exist(x =>(x.UserName == command.UserName || x.Mobile == command.Mobile) && x.Id!=command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var Path = $"ProfilePhotos";
            var PhotoPath = _fileUploder.Upload(command.ProfilePhoto, Path);

            account.Edit(command.FullName,command.UserName,command.Mobile,command.RoleId, PhotoPath);
            _accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordNotMatch);

            var password = _passwordHasher.Hash(command.Password);
            account.ChangePassword(password);

            _accountRepository.SaveChanges();
            return operation.Succedded();

        }

        public EditAccount GetDetails(long Id)
        {
            return _accountRepository.GetDetails(Id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }

        public void Logout()
        {
            _authHelper.SignOut();
        }

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.UserName);
            if (account == null)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            (bool Verified, bool NeedsUpgrade) result= _passwordHasher.Check(account.Password, command.Password);

            if(!result.Verified)
                return operation.Failed(ApplicationMessages.WrongUserPass);


            var authViewModel = new AuthViewModel(account.Id,account.RoleId,account.FullName,account.UserName);

            _authHelper.Signin(authViewModel);
            return operation.Succedded();

        }


    }
}

