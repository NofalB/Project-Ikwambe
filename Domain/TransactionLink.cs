﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TransactionLink
    {
        // The API resource URL of the payment itself.
        public string SelfUrl { get; set; }
        //The URL your customer should visit to make the payment. This is where you should redirect the consumer to.
        public string Checkout { get; set; }

        public TransactionLink(string selfUrl, string checkout)
        {
            SelfUrl = selfUrl;
            Checkout = checkout;

        }


    }
}