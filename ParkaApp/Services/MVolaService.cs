using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ParkaApp.Services.Interfaces;


namespace ParkaApp.Services
{ 
    public class MVolaService : IMVolaService
    {
        private readonly HttpClient _httpClient;

        private const string baseUrl = "https://devapi.mvola.mg";
        private const string consumerKey = "8aeLSUwdf3Dwy7TX7vQjn97ggzUa";
        private const string consumerSecret = "N_niMlqxZUp1TCyGMBIQnJfwwiUa";
        private const string merchantNumber = "0343500003";
        private const string partnerName = "ParkaAppTest";



        public MVolaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // =========================
        // TOKEN
        // =========================
        public async Task<string> GetAccessTokenAsync()
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}")
            );

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var form = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "scope", "EXT_INT_MVOLA_SCOPE" }
            };

            request.Content = new FormUrlEncodedContent(form);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            using var json = JsonDocument.Parse(content);
            return json.RootElement.GetProperty("access_token").GetString()!;
        }

        // =========================
        // MERCHANT PAY
        // =========================
        public async Task<string?> MerchantPayAsync(
            string customerNumber,
            string amount
        )
        {
            // Token
            string token = await GetAccessTokenAsync();

            // Correlation ID
            var correlationId = Guid.NewGuid().ToString();

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{baseUrl}/mvola/mm/transactions/type/merchantpay/1.0.0/"
            );

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("Version", "1.0");
            request.Headers.Add("X-CorrelationID", correlationId);
            request.Headers.Add("UserLanguage", "MG");
            request.Headers.Add("UserAccountIdentifier", $"msisdn;{merchantNumber}");
            request.Headers.Add("partnerName", partnerName);

            var payload = new
            {
                amount = amount,
                currency = "Ar",
                descriptionText = "Paiement Marchand",
                requestDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                requestingOrganisationTransactionReference = Guid.NewGuid().ToString(),
                originalTransactionReference = "MVOLA_123",

                debitParty = new[]
                {
                    new { key = "msisdn", value = customerNumber }
                },

                creditParty = new[]
                {
                    new { key = "msisdn", value = merchantNumber }
                },

                metadata = new[]
                {
                    new { key = "partnerName", value = partnerName },
                    new { key = "fc", value = "USD" },
                    new { key = "amountFc", value = "10" }
                }
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine("PAYMENT STATUS: " + response.StatusCode);
            Console.WriteLine(content);

            if (!response.IsSuccessStatusCode)
                return null;

            try
            {
                using var json = JsonDocument.Parse(content);
                if (json.RootElement.TryGetProperty("serverCorrelationId", out var id))
                    return id.GetString();
            }
            catch { }

            return correlationId;
        }

        // =========================
        // STATUS
        // =========================
        public async Task<string?> GetStatusAsync(
            string serverCorrelationId
        )
        {
            // Token
            string token = await GetAccessTokenAsync();


            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{baseUrl}/mvola/mm/transactions/type/merchantpay/1.0.0/status/{serverCorrelationId}"
            );

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("Version", "1.0");
            request.Headers.Add("X-CorrelationID", Guid.NewGuid().ToString());
            request.Headers.Add("UserLanguage", "MG");
            request.Headers.Add("UserAccountIdentifier", $"msisdn;{merchantNumber}");
            request.Headers.Add("partnerName", partnerName);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine("STATUS RESPONSE: " + response.StatusCode);
            Console.WriteLine(content);

            return content;
        }
    }
}