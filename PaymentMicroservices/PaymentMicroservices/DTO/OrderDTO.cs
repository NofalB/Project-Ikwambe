using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PaymentMicroservices.DTO
{
    public class OrderDTO
    {
        private PropertyInfo[] _PropertyInfos = null;

        public string CheckoutPaymentIntent;
        [DataMember(Name = "create_time", EmitDefaultValue = false)]
        public string CreateTime;
        [DataMember(Name = "expiration_time", EmitDefaultValue = false)]
        public string ExpirationTime;
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id;
        [DataMember(Name = "links", EmitDefaultValue = false)]
        public List<LinkDescription> Links;
        [DataMember(Name = "payer", EmitDefaultValue = false)]
        public Payer Payer;
        [DataMember(Name = "purchase_units", EmitDefaultValue = false)]
        public List<PurchaseUnit> PurchaseUnits;
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status;
        [DataMember(Name = "update_time", EmitDefaultValue = false)]
        public string UpdateTime;

        public OrderDTO(string checkoutPaymentIntent, string expirationTime, string id, Payer payer, List<PurchaseUnit> purchaseUnits, string status, string updateTime)
        {
            CheckoutPaymentIntent = checkoutPaymentIntent;
            ExpirationTime = expirationTime;
            Id = id;
            Payer = payer;
            PurchaseUnits = purchaseUnits;
            Status = status;
            UpdateTime = updateTime;

        }
        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}
