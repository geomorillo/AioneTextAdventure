using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Añadir using para LmStudioApiClient


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

            // Preguntar al usuario qué API de IA desea usar
            Console.WriteLine("¿Qué API de IA deseas usar? (1) Ollama (2) LM Studio");
            string? apiChoice = Console.ReadLine();

            IAiApiClient apiClient;
            if (apiChoice == "2")
            {
                apiClient = new LmStudioApiClient();
                Console.WriteLine("Usando LM Studio API.");
            }
            else if (apiChoice == "1")
            {
                apiClient = new OllamaApiClient();
                Console.WriteLine("Usando Ollama API.");
            }
            else
            {
                Console.WriteLine("Opción no válida. Usando Ollama API por defecto.");
                apiClient = new OllamaApiClient();
            }

            AICharacter.Initialize(apiClient);
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
                string? playerInput;
                List<string> currentOptions = GameContext.GetCurrentOptions(); // Obtener opciones de la iteración anterior

                if (currentOptions.Count > 0)
                {
                    Console.WriteLine("Opciones:");
                    foreach (string option in currentOptions)
                    {
                        Console.WriteLine(option);
                    }

                    Console.Write("Tu acción (o número de opción): ");
                    string? finalPlayerInput = Console.ReadLine();

                    if (string.IsNullOrEmpty(finalPlayerInput))
                    {
                        Console.WriteLine("Por favor, introduce una acción válida.");
                        continue;
                    }

                    if (int.TryParse(finalPlayerInput, out int optionNumber) && optionNumber > 0 && optionNumber <= currentOptions.Count)
                    {
                        playerInput = currentOptions[optionNumber - 1].Substring(currentOptions[optionNumber - 1].IndexOf('.') + 1).Trim();
                        Console.WriteLine($"Has elegido: {playerInput}");
                    }
                    else
                    {
                        playerInput = finalPlayerInput; // 
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Tú: ");
                    Console.ResetColor();
                    playerInput = Console.ReadLine();
                }

                if (string.IsNullOrEmpty(playerInput))
                {
                    Console.WriteLine("Por favor, introduce una acción válida.");
                    continue;
                }

                if (playerInput.ToLower() == "salir")
                {
                    Console.WriteLine("Gracias por jugar. ¡Hasta pronto!");
                    break;
                }

                string aiResponse = await AICharacter.GetResponse(playerInput);
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


}
