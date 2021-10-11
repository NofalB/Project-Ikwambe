using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    // Transaction myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Link
    {
        public string href { get; set; }
        public string method { get; set; }
        public string rel { get; set; }
    }

    public class Address
    {
        public string country_code { get; set; }
        public string address_line_1 { get; set; }
        public string admin_area_1 { get; set; }
        public string admin_area_2 { get; set; }
        public string postal_code { get; set; }
    }

    public class Name
    {
        public string given_name { get; set; }
        public string surname { get; set; }
        public string full_name { get; set; }
    }

    public class Payer
    {
        public Address address { get; set; }
        public string email_address { get; set; }
        public Name name { get; set; }
        public string payer_id { get; set; }
    }

    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Payee
    {
        public string email_address { get; set; }
        public string merchant_id { get; set; }
    }

    public class Shipping
    {
        public Address address { get; set; }
        public Name name { get; set; }
    }

    public class PurchaseUnit
    {
        public Amount Amount { get; set; }
        public Payee payee { get; set; }
        public string reference_id { get; set; }
        public Shipping shipping { get; set; }
    }

    public class Transaction
    {
        public DateTime create_time { get; set; }
        public string TransactionId { get; set; }
        public string intent { get; set; }
        public List<Link> links { get; set; }
        public Payer payer { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public string status { get; set; }
        public string PartitionKey { get; set; }

    }


    public class CreateTransaction
    {
        public string Message { get; set; }
        public string Link { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }



}
