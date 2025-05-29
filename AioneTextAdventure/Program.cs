using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AioneTextAdventure
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bienvenido a la Aventura de IA Conversacional!");
            Console.WriteLine("Escribe 'salir' para terminar el juego.");
            Console.WriteLine("\n-- Comienza la aventura --\n");
            Console.WriteLine(GameContext.GetCurrentSceneDescription());
            Console.WriteLine("\n");

            await Game.Start();
        }
    }

    public static class Game
    {
        public static async Task Start()
        {
            // Aquí irá la lógica principal del juego y la interacción con la IA
            string lastSceneDescription = GameContext.GetCurrentSceneDescription();
            while (true)
            {
                string playerInput;
                List<string> currentOptions = GameContext.GetCurrentOptions(); // Obtener opciones de la iteración anterior

                if (currentOptions.Count > 0)
                {
                    Console.WriteLine("Opciones:");
                    foreach (string option in currentOptions)
                    {
                        Console.WriteLine(option);
                    }

                    Console.Write("Tu acción (o número de opción): ");
                    string finalPlayerInput = Console.ReadLine();

                    if (int.TryParse(finalPlayerInput, out int optionNumber) && optionNumber > 0 && optionNumber <= currentOptions.Count)
                    {
                        playerInput = currentOptions[optionNumber - 1].Substring(currentOptions[optionNumber - 1].IndexOf('.') + 1).Trim();
                        Console.WriteLine($"Has elegido: {playerInput}");
                    }
                    else
                    {
                        playerInput = finalPlayerInput; // Use the free-form input if not an option
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Tú: ");
                    Console.ResetColor();
                    playerInput = Console.ReadLine();
                }


                if (playerInput.ToLower() == "salir")
                {
                    Console.WriteLine("Gracias por jugar. ¡Hasta pronto!");
                    break;
                }

                string aiResponse = await AICharacter.GetResponse(playerInput); // Usar playerInput final
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"IA: {aiResponse}");
                Console.ResetColor();

                GameContext.UpdateContext(playerInput, aiResponse); // Actualizar contexto con playerInput final y aiResponse

                // Si la escena ha cambiado, mostrar la nueva descripción
                if (GameContext.GetCurrentSceneDescription() != lastSceneDescription)
                {
                    Console.WriteLine($"\n-- Nueva Escena: {GameContext.CurrentScene.Replace("_", " ").ToUpper()} --\n");
                    Console.WriteLine(GameContext.GetCurrentSceneDescription());
                    Console.WriteLine("\n");
                    lastSceneDescription = GameContext.GetCurrentSceneDescription();
                }
            }
        }
    }

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

    public static class AICharacter
    {
        private static OllamaApiClient _ollamaClient = new OllamaApiClient();

        public static async Task<string> GetResponse(string playerInput)
        {
            string sceneDescription = GameContext.GetCurrentSceneDescription();
            string aiPersonality = GameContext.AICharacterPersonality;

            string prompt = $"Eres {aiPersonality}\n\nContexto de la escena actual: {sceneDescription}\n\nEl jugador dice: {playerInput}\n\nTu respuesta debe incluir una parte narrativa y, opcionalmente, 3 opciones de diálogo para el jugador. Formatea las opciones como una lista numerada al final de tu respuesta, por ejemplo:\nNarrativa de la IA.\n1. Opción 1\n2. Opción 2\n3. Opción 3\n\nTu respuesta (proporciona pistas claras si el jugador parece perdido o no avanza en la historia. Guíalo sutilmente hacia la siguiente acción relevante o un objeto interactivo): ";

            string fullResponse = await _ollamaClient.GetOllamaResponse(prompt);

            // Parse the AI's response to separate narrative and options
            string[] lines = fullResponse.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> options = new List<string>();
            string narrative = "";

            foreach (string line in lines)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+\.\s*"))
                {
                    options.Add(line);
                }
                else
                {
                    narrative += line + "\n";
                }
            }

            GameContext.SetCurrentOptions(options);
            return narrative.Trim();
        }
    }
}
