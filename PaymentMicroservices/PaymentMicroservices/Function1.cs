using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PaymentMicroservices.Services;
using PayPalCheckoutSdk.Orders;

namespace PaymentMicroservices
{
    public static class Function1
    {
        [Function("CreatePayment")]
        public static async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            Console.WriteLine("Create Order with minimum payload..");
            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            //request.Headers.Add("authorization","Basic<AVqSBkkqxw0jOZsOL1meIPnI25Ozcj12Ec8xgZqTrf6R80GftrWaxPDFvC0j4YO2K_Kyze2yNTgwhPRi:EBBGNsdoiHdTKNZk1ax86VSNmzxIYG3A1rO2m0C3A2Hoa24Z5hvrQsm2oZmnF0KF2dL3iOkQNtf7tob5>");
            request.RequestBody(BuildRequestBodyWithMinimumFields());
            var response = await PaypalClientService.Client().Execute(request);

            
            
            var result = response.Result<Order>();
            List<String> paymentLinks = new List<string>();

            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Order Id: {0}", result.Id);
            Console.WriteLine("Intent: {0}", result.CheckoutPaymentIntent);
            Console.WriteLine("Links:");
            foreach (LinkDescription link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
                paymentLinks.Add(link.Rel);
                paymentLinks.Add(link.Href);
                paymentLinks.Add(link.Method);

            }
            AmountWithBreakdown amount = result.PurchaseUnits[0].AmountWithBreakdown;
            Console.WriteLine("Total Amount: {0} {1}", amount.CurrencyCode, amount.Value);

            var response1 = req.CreateResponse(HttpStatusCode.OK);

            //returns header and status code of the response
            //await response1.WriteAsJsonAsync(response);

            await response1.WriteAsJsonAsync(result.ToString());
            return response1;
        }

        private static OrderRequest BuildRequestBodyWithMinimumFields()
        {
            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext
                {
                    CancelUrl = "https://www.example.com",
                    ReturnUrl = "https://www.example.com"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = "220.00"
                        }

                    }
                }
            };

            return orderRequest;
        }
    }
}
