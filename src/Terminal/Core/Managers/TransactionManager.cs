using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Terminal.Core.Exceptions;
using Terminal.Infrastructure.Storage;

namespace Terminal.Core.Managers
{
    /// <summary>
    /// Manager to register and keep transactions for banks
    /// </summary>
    public class TransactionManager
    {
        private const string BitBank = "BitBank";
        private const string TransactionStorageKey = "Transactions";
        
        private readonly IStorage _storage;
        private readonly ProcessorManager _processorManager;
        private readonly ILogger<TransactionManager> _logger;

        private class TransactionContainer
        {
            public List<Dictionary<string, Dictionary<string, long>>> Transactions;

            public TransactionContainer()
            {
                Transactions = new List<Dictionary<string, Dictionary<string, long>>>();
            }
        }

        public TransactionManager(IStorage storage, ProcessorManager processorManager, ILogger<TransactionManager> logger)
        {
            _storage = storage;
            _processorManager = processorManager;
            _logger = logger;
        }

        public void AddTransaction(DateTime date, long amount)
        {
            TransactionContainer transactionsContainer;
            var transaction = new Dictionary<string, Dictionary<string, long>> {{date.ToString(), new Dictionary<string, long>()}};
            transaction[date.ToString()] = new Dictionary<string, long> {{BitBank, amount}};
            _logger.LogInformation($"New transaction: {date.ToString()} = {amount}");
            
            try
            {
                transactionsContainer = _storage.Get<TransactionContainer>("transactions");
            }
            catch (NotFoundException)
            {
                transactionsContainer = new TransactionContainer();
            }
            
            transactionsContainer.Transactions.Add(transaction);
            _storage.Save(TransactionStorageKey, transactionsContainer);
            _processorManager.HandlePayment(BitBank, amount);
        }
    }
}
