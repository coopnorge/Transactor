using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Core.Managers;

namespace Terminal.Core.Generators
{
    using static FormattableString;

    /// <summary>
    /// Simple string statement generator for printing account balance
    /// </summary>
    public class PrinterGenerator : IPrinterGenerator
    {
        private const string StatementHeader = "| Bank account | Balance |";
        private string _buffer = string.Empty;
        
        private readonly AccountManager _accountManager;

        public PrinterGenerator(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }
        
        public string GenerateStatement()
        {
            var bankAccounts = _accountManager.GetBankAccounts();
            if (!bankAccounts.Any()) return string.Empty;

            _buffer = string.Empty;
            
            WriteLine("\n");
            WriteLine(StatementHeader + "\n");
            PrintStatementLinesFor(bankAccounts);
            WriteLine("\n");

            return _buffer;
        }
        
        private void PrintStatementLinesFor(IEnumerable<AccountManager.Account> bankAccounts)
        {
            bankAccounts
                .Select(StatementLine)
                .Reverse()
                .ToList()
                .ForEach(WriteLine);
        }

        private void WriteLine(string line)
        {
            _buffer += line;
        }

        private static string StatementLine(AccountManager.Account account)
        {
            return Invariant($"| {account.Bank} | {account.Balance:0.00} |\n");
        }
    }
}