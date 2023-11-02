﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;

        public AccountsController(IUserDao userDao, IAccountDao accountDao)
        {
            this.userDao = userDao;
            this.accountDao = accountDao;
        }

        [HttpGet("{account_id}")]
        public ActionResult<Account> GetAccount(int account_id)
        {
            Account account = accountDao.GetAccountById(account_id);
            if(account != null)
            {
                return account;
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("/balance")]
        public decimal GetBalanceById(int id)
        {
            decimal balance = accountDao.GetBalanceByUserId(id);
            if(balance != 0)
            {
                return balance;
            }
            else
            {
                return 0.0M;
            }
        }

        [HttpPut("/balance")]
        public bool UpdateBalanceByGiving(int accountId, decimal amount)
        {
            try
            {
                bool giving = accountDao.TransferMoney(accountId, amount);
                return giving;
            }
            catch (DaoException)
            {
                throw new DaoException("Unacceptable command");
            }
        }

        [HttpPut("/balance")]
        public bool UpdateBalanceByTaking(int accountId, decimal amount)
        {
            try
            {
                bool taking = accountDao.TransferMoney(accountId, amount);
                return taking;
            }
            catch (DaoException)
            {
                throw new DaoException("Unacceptable command");
            }
        }


    }
}
