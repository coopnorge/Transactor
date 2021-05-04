using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Terminal.Core.Clients;
using Terminal.Core.Exceptions;
using Terminal.Core.Generators;
using Terminal.Core.Managers;

namespace Terminal.Core
{
    /// <summary>
    /// Core Application loop
    /// </summary>
    public class Application
    {
        private const short DelayMilliSec = 3000;
        private byte _failsCheck = 3;
        private bool _isRunning = false;

        private readonly PaymentGateway _paymentGateway;
        private readonly TransactionManager _transactionManager;
        private readonly IPrinterGenerator _printerGenerator;
        private readonly AccountManager _accountManager;
        private readonly ILogger<Application> _logger;

        public Application(
            PaymentGateway paymentGateway,
            TransactionManager transactionManager, 
            IPrinterGenerator printerGenerator,
            AccountManager accountManager, 
            ILogger<Application> logger
        )
        {
            _paymentGateway = paymentGateway;
            _transactionManager = transactionManager;
            _printerGenerator = printerGenerator;
            _accountManager = accountManager;
            _logger = logger;
        }

        public void Run()
        {
            _isRunning = true;
            _logger.LogInformation("Creating bank accounts...");
            _accountManager.AddBankAccount("BitBank", 5000);
            _accountManager.AddBankAccount("CapsBank", 10000);
            _logger.LogInformation(_printerGenerator.GenerateStatement());
            _logger.LogInformation("Executing application...");
            
            while (_isRunning)
            {
                try
                {
                    var trxBatch = new Dictionary<string, long>();
                    foreach (var _ in Enumerable.Range(0, 5))
                    {
                        var (trxDate, trxAmount) = _paymentGateway.ReceiveTransaction();
                        trxBatch[trxDate.ToString()] = trxAmount;
                    }

                    Parallel.ForEach(Enumerable.Range(0, trxBatch.Count), i =>
                    {
                        var j = 0;
                        foreach (var (trxDate, trxAmount) in trxBatch)
                        {
                            if (j == i)
                            {
                                _transactionManager.AddTransaction(DateTime.Parse(trxDate), trxAmount);
                                break;
                            }
                            else
                            {
                                j++;
                            }
                        }
                    });
                }
                catch (NotFoundException e)
                {
                    _failsCheck--;
                    _logger.LogError($"Application interrupted with exception: {e.Message}");
                    _logger.LogError($"Trying to recover, attempts left: {1 + _failsCheck}");
                }
                catch (TransactorException e)
                {
                    _logger.LogCritical($"Unexpected exception: {e.Message}");
                }
                finally
                {
                    _isRunning = _failsCheck != 0;
                }
                
                _logger.LogInformation(_printerGenerator.GenerateStatement());
                Thread.Sleep(DelayMilliSec);
            }
        }
    }
}
