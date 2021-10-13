using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    // Transaction myDeserializedClass = JsonConvert.DeserializeObject<Transaction>(myJsonResponse); 
    public class Link
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }
    }


    //if specified as keyless then EF can not determine the relationship between entities
    //if there is one entity within another entity then it runs fine, it theres another one in that referenced entity, shitstorm and PK needed
    public class Address
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("address_line_1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("admin_area_1")]
        public string AdminArea1 { get; set; }

        [JsonProperty("admin_area_2")]
        public string AdminArea2 { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        public string AddressId { get; set; }
    }

    public class Name
    {
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        public string NameId { get; set; }

    }

    public class Payer
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("payer_id")]
        public string PayerId { get; set; }
    }

    public class Amount
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public string AmountId { get; set; }

    }

    public class Payee
    {
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }

        public string PayeeId { get; set; }

    }

    public class Shipping
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }
        //shipping needs a created pk because it cant rely on other classes as PK
        public string ShippingId { get; set; }
    }

    public class Payments
    {
        public string PaymentsId { get; set; }

        [JsonProperty("captures")]
        public List<Capture> Captures { get; set; }
    }

    public class Capture
    {

        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("final_capture")]
        public bool FinalCapture { get; set; }

        [JsonProperty("id")]
        public string CaptureId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("update_time")]
        public DateTime UpdateTime { get; set; }
    }

    public class PurchaseUnit
    {
        [JsonProperty("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("payee")]
        public Payee Payee { get; set; }

        [JsonProperty("payments")]
        public Payments Payments { get; set; }

        [JsonProperty("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }
    }

    public class Transaction
    {
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("id")]
        public string TransactionId { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("payer")]
        public Payer Payer { get; set; }

        [JsonProperty("purchase_units")]
        public List<PurchaseUnit> PurchaseUnits { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public string PartitionKey { get; set; }
    }


    public class CheckoutUrl
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("orderId")]
        public string TransactionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }



}
