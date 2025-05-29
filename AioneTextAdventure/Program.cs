using System;
using System.Collections.Generic;
using AioneTextAdventure;
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


}
