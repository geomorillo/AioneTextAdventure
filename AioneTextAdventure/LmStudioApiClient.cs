using System.Text;
using Newtonsoft.Json;

namespace AioneTextAdventure
{
    public class LmStudioApiClient : IAiApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public LmStudioApiClient(string baseUrl = "http://localhost:1234/v1/")
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
        }

        public async Task<string> GetCompletionAsync(string prompt)
        {
            var requestBody = new
            {
                prompt = prompt,
                max_tokens = 500,
                temperature = 0.7
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}completions", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<LmStudioCompletionResponse>(responseContent);

                if (jsonResponse?.Choices != null && jsonResponse.Choices.Count > 0 && jsonResponse.Choices[0]?.Text != null)
                {
                    return jsonResponse.Choices[0].Text ?? string.Empty;
                }
                return "No se pudo obtener una respuesta de LM Studio.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al llamar a la API de LM Studio: {response.StatusCode} - {errorContent}");
            }
        }
    }

    public class LmStudioCompletionResponse
    {
        [JsonProperty("choices")]
        public List<Choice>? Choices { get; set; }
    }

    public class Choice
    {
        [JsonProperty("text")]
        public string? Text { get; set; }
    }
}