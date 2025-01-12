using Blasterify.Services.Data;
using Blasterify.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly DataContext _context;

        public TestController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            string userClientcountry = "US";
            int totalPrice = 322;
            string getCustomerId = "30fee24f-9e08-4d18-ac2c-80f94eb0565e";
            string getMerchantOrderId = "1020";

            var checkoutSessionRequest = new Blasterify.Models.Yuno.CheckoutSessionRequest
            {
                Country = userClientcountry,
                Amount = new Blasterify.Models.Yuno.Amount
                {
                    Currency = "USD",
                    Value = totalPrice
                },
                Customer_Id = getCustomerId,
                Merchant_Order_Id = getMerchantOrderId,
                Payment_Description = "Rent movies.",
                Account_Id = YunoServices.AccountId
            };

            var response = await YunoServices.SendPostMethod(checkoutSessionRequest, new String("checkout/sessions"));

            var checkoutSession = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CheckoutSession>(response);

            return Ok(checkoutSession);
        }
    }
}
