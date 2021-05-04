using System;

namespace Terminal.Core.Clients
{
    public class PaymentGateway
    {
        private readonly Random _random;
        
        public PaymentGateway()
        {
            _random= new Random();
        }

        /// <summary>
        /// Receive transaction 
        /// </summary>
        /// <returns>Transaction Date and Amount</returns>
        public (DateTime date, int transactionAmount) ReceiveTransaction()
        {
            var amount = _random.Next(100, 2000);
            var date = DateTime.Now.AddMilliseconds(_random.Next(1000, 10000));

            return (date, amount);
        }
    }
}