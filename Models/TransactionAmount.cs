using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Models
{
    public class TransactionAmount
    {
        // The ISO 4217 currency code.
        public string Currency { get; set; }

        // A string containing the exact amount of the payment in the given currency.
        public string Value { get; set; }

        public TransactionAmount(string currency, string value)
        {
            Currency = currency;
            Value = value;
              
        }

    }
}
