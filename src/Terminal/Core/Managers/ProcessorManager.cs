using Microsoft.Extensions.Logging;

namespace Terminal.Core.Managers
{
    /// <summary>
    /// Manager to process transaction for the banks
    /// </summary>
    public class ProcessorManager
    {
        private readonly AccountManager _accountManager;
        private readonly ILogger<ProcessorManager> _logger;
        
        public ProcessorManager(
            AccountManager accountManager, 
            ILogger<ProcessorManager> logger
        )
        {
            _accountManager = accountManager;
            _logger = logger;
        }

        public bool HandlePayment(string bank, long amount)
        {
            var pay = _accountManager.Pay(bank, amount);
            if (pay)
            {
                _logger.LogInformation($"[SUCCESS] Processing transaction via {bank}: -{amount}");
            }
            else
            {
                _logger.LogError($"[FAILED] Processing transaction via {bank}: -{amount}");
            }

            return pay;
        }
    }
}
