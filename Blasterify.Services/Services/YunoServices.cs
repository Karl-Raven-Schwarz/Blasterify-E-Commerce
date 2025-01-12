﻿using Newtonsoft.Json;
using System.Text;

namespace Blasterify.Services.Services
{
    public class YunoServices
    {
        static string? PublicAPIKey;
        static string? PrivateSecretKey;
        static string UrlBase = "https://api-sandbox.y.uno/v1/";
        public static readonly string AccountId = "0d8f44ff-15fc-4a8c-b65e-fa5dcdf84ccc";

        public static readonly List<string> ErrorCodes = new() {
            "CUSTOMER_ID_DUPLICATED"
        };

        public static void SetKeys(string publicAPIKey, string privateSecretKey)
        {
            PublicAPIKey = publicAPIKey;
            PrivateSecretKey = privateSecretKey;
        }

        public static string GetPublicAPIKey()
        {
            return PublicAPIKey!;
        }

        public static async Task<string> CreateCustomer(Blasterify.Models.Yuno.CustomerRequest customerRequest)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("charset", "utf-8");
            client.DefaultRequestHeaders.Add("public-api-key", PublicAPIKey);
            client.DefaultRequestHeaders.Add("private-secret-key", PrivateSecretKey);

            var json = JsonConvert.SerializeObject(customerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api-sandbox.y.uno/v1/customers", content);

            var jsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CustomerRequest>(jsonString);
                return data!.id;
            }
            else
            {
                var data = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.ErrorResponse>(jsonString);
                return data!.Code;
            }
        }

        public static async Task<string> SendPostMethod<T>(T entity, String method)
        {
            String UrlMetodo = UrlBase + method;
            String jsonText = String.Empty;
            HttpResponseMessage response;
            HttpClient client = new();
            client.BaseAddress = new Uri(UrlMetodo);

            HttpRequestMessage request = new(HttpMethod.Post, UrlMetodo);

            jsonText = JsonConvert.SerializeObject(entity);

            request.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            Random rnd = new Random();
            int num = rnd.Next();

            request.Headers.Add("accept", "application/json");
            request.Headers.Add("charset", "utf-8");
            request.Headers.Add("public-api-key", PublicAPIKey);
            request.Headers.Add("private-secret-key", PrivateSecretKey);
            request.Headers.Add("X-idempotency-key", num.ToString());

            response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> GetCustomer(String merchant_customer_id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api-sandbox.y.uno/v1/customers?merchant_customer_id={merchant_customer_id}"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "charset", "utf-8" },
                    { "public-api-key", "sandbox_gAAAAABmUMNXVdBwRm0wIsrU2cCUka1J8UoITkVo2P0gx7fRxguQI_6M1fU_51RpakcAsoxzL7rWwJOlIhZVKv-zrQyQCpMGwAXzh3RzDhnCoPZWzb415LlVIWAOz7Tm5lMpnSkGI_guHyMwaH8NdIMi78p8N6cxbggKeH4z3Fz1BmkRpXAFhyyrFfrfYar8XNvfR_7n9ZqPhjX1ELx9pNz4Zvtt_OiC7A5-IMbYOjNa1dp4mC8C-rG9x7iPzPoXD3zSHI8Jes60" },
                    { "private-secret-key", "gAAAAABmUMNXbtEDkxpJXEMeVTwrSfW3fsrVmb7GC1dcbuq4Hu_rZs9XSOP0dzXMo6QsVjuTCBUtAmQ13VYlzPPcwkMlyzLR-uN6WEwL7tqB6BhZTZ5xb3QZkZDW_Km1ZPs4jS4AmmQL5NpDHIqtXm08MvLN04hXvP3J52mg_WuA9xfZJ8eGKeu0Xw4HkBREVoD-ATPtRNoU0nWxDZVQwlJOEGc84xtzUT1hfOD9AI8efBOIhw5W5-VvbAm0dnA9SZ-QgY0LyvYE" },
                },
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return body;
        }
    }
}