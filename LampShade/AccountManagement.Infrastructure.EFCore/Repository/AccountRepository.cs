﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository: RepositoryBase<long, Account>, IAccountRepository
    {
        private readonly AccountContext _accountContext;

        public AccountRepository(AccountContext accountContext) : base(accountContext)
        {
            _accountContext = accountContext;
        }

        public Account GetBy(string username)
        {
           return _accountContext.Accounts.FirstOrDefault(x => x.UserName == username);
        }

        public EditAccount GetDetails(long Id)
        {
            return _accountContext.Accounts.Select(x => new EditAccount()
            {
                Id = x.Id,
                FullName = x.FullName,
                Mobile = x.Mobile,
                RoleId = x.RoleId,
                UserName = x.UserName
            }).FirstOrDefault(x => x.Id == Id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            var query = _accountContext.Accounts.Include(x=>x.Role).Select(x => new AccountViewModel()
            {
                Id= x.Id,
                FullName = x.FullName,
                Mobile = x.Mobile,
                UserName = x.UserName,
                ProfilePhoto = x.ProfilePhoto,
                Role = x.Role.Name,
                RoleId = x.RoleId,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
                query = query.Where(x => x.FullName.Contains(searchModel.FullName));
            if (!string.IsNullOrWhiteSpace(searchModel.UserName))
                query = query.Where(x => x.UserName.Contains(searchModel.UserName));
            if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
                query = query.Where(x => x.Mobile.Contains(searchModel.Mobile));
            if (searchModel.RoleId>0)
                query = query.Where(x => x.RoleId==searchModel.RoleId);

            return query.OrderByDescending(x=>x.Id).ToList();
        }
    }
}
