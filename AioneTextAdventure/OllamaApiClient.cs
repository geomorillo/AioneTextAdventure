using System; 
using System.Net.Http; 
using System.Net.Http.Json; 
using System.Text.Json; 
using System.Threading.Tasks; 

namespace AioneTextAdventure 
{ 
    public class OllamaApiClient 
    { 
        private readonly HttpClient _httpClient; 
        private readonly string _ollamaApiUrl; 

        public OllamaApiClient(string ollamaApiUrl = "http://localhost:11434/api/generate") 
        { 
            _httpClient = new HttpClient(); 
            _ollamaApiUrl = ollamaApiUrl; 
        } 

        public async Task<string> GetOllamaResponse(string prompt, string model = "cogito:latest") 
        { 
            try 
            { 
                var requestBody = new 
                { 
                    model = model, 
                    prompt = prompt, 
                    stream = false 
                }; 

                var response = await _httpClient.PostAsJsonAsync(_ollamaApiUrl, requestBody); 
                response.EnsureSuccessStatusCode(); 

                var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>(); 
                if (jsonResponse.TryGetProperty("response", out var ollamaResponse)) 
                { 
                    return ollamaResponse.GetString() ?? ""; 
                } 
                return "No se pudo obtener una respuesta de Ollama."; 
            } 
            catch (HttpRequestException e) 
            { 
                return $"Error de conexión con Ollama: {e.Message}. Asegúrate de que Ollama esté corriendo en {_ollamaApiUrl}."; 
            } 
            catch (Exception e) 
            { 
                return $"Ocurrió un error: {e.Message}"; 
            } 
        } 
    } 
}