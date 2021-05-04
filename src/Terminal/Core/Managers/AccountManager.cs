using System.Collections.Generic;
using System.Linq;
using Terminal.Core.Exceptions;
using Terminal.Infrastructure.Storage;

namespace Terminal.Core.Managers
{
    /// <summary>
    /// Bank account to manage transaction payment
    /// </summary>
    public class AccountManager
    {
        private const string BankAccountsKey = "BankAccounts";
        private readonly IStorage _storage;
        
        public class Account
        {
            public string Bank;

            public long Balance;
        
            public Account(string bank, long balance)
            {
                Bank = bank;
                Balance = balance;
            }
        }

        public AccountManager(IStorage storage)
        {
            _storage = storage;
        }

        public void AddBankAccount(string name, long balance)
        {
            var bankAccounts = new List<Account>();
            
            try
            {
                bankAccounts = _storage.Get<List<Account>>(BankAccountsKey);
            }
            catch (NotFoundException)
            {
            }
            
            bankAccounts.Add(new Account(name, balance));
            
            _storage.Save(BankAccountsKey, bankAccounts);
        }
        
        public Account[] GetBankAccounts()
        {
            return _storage.Get<List<Account>>(BankAccountsKey).ToArray();
        }

        public bool Pay(string bank, long transaction_Amount)
        {
            var bankAccount = GetBankAccounts().FirstOrDefault(account => account.Bank == bank);
            if (bankAccount == null)
            {
                return false;
            }
            else if (bankAccount.Balance >= transaction_Amount)
            {
                bankAccount.Balance -= transaction_Amount;
                _storage.Save(bank, bankAccount.Balance);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
