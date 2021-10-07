using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentMicroservices.Services;
using PayPalCheckoutSdk.Orders;

namespace PaymentMicroservices
{
    public static class PaypalOrderAPI
    {
        [Function("CreateOrder")]
        public static async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateOrder");
            try
            {
                if (!String.IsNullOrEmpty(req.Url.Query))
                {
                    string currencyCode = HttpUtility.ParseQueryString(req.Url.Query).Get("currency");
                    string value = HttpUtility.ParseQueryString(req.Url.Query).Get("value");

                    Console.WriteLine("Creating Order with minimum payload..");
                    var request = new OrdersCreateRequest();

                    //return=minimal. The server returns a minimal response to optimize communication between the API caller and the server. A minimal response includes the id, status and HATEOAS links.
                    //return= representation.The server returns a complete resource representation, including the current state of the resource.
                    request.Headers.Add("prefer", "return=representation");

                    request.RequestBody(BuildOrderRequestBody(currencyCode, value));
                    var response = await PaypalClientService.Client().Execute(request);
                    var result = response.Result<Order>();

                    var response1 = req.CreateResponse(HttpStatusCode.OK);

                    await response1.WriteStringAsync(JsonConvert.SerializeObject(result));
                    return response1;
                }
            }
            catch (Exception e)
            {

                var errResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                logger.LogInformation(e.ToString());

                await errResponse.WriteStringAsync("missing query params");
                return errResponse;
            }
            
            return null;
            


            
        }

        [Function("GetOrder")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
             FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateOrder");
            try
            {
                if (!String.IsNullOrEmpty(req.Url.Query))
                {
                    string param1 = HttpUtility.ParseQueryString(req.Url.Query).Get("orderId");
                    OrdersGetRequest request = new OrdersGetRequest(param1);

                    var response = await PaypalClientService.Client().Execute(request);
                    var result = response.Result<Order>();

                    var response1 = req.CreateResponse(HttpStatusCode.OK);
                    await response1.WriteStringAsync(JsonConvert.SerializeObject(result));
                    return response1;
                }
            }
            catch (Exception e)
            {

                var errResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                logger.LogInformation(e.ToString());

                await errResponse.WriteStringAsync("missing query params");
                return errResponse;
            }

            return null;
        }

        private static OrderRequest BuildOrderRequestBody(string currencyCode,string value)
        {
            OrderRequest orderRequest = new OrderRequest()
            {
                //using CAPTURE to immediately get the funds from the payer instead of authorizing first
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext
                {
                    CancelUrl = "https://www.example.com",
                    ReturnUrl = "https://youtu.be/dQw4w9WgXcQ?t=42"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = currencyCode,
                            Value = value
                        }

                    }
                }
            };

            return orderRequest;
        }
    }
}
