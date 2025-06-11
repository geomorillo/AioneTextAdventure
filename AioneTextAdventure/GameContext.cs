using System;
using System.Collections.Generic;

namespace AioneTextAdventure
{
    public static class GameContext
    {
        public static string CurrentScene { get; set; } = "inicio";
        public static Dictionary<string, string> SceneDescriptions { get; private set; } = null!;
        public static string AICharacterPersonality { get; private set; } = null!;
        public static List<string> CurrentOptions { get; private set; } = new List<string>();

        static GameContext()
        {
            SceneDescriptions = new Dictionary<string, string>
            {
                { "inicio", "Estás en una habitación oscura y húmeda. Un tenue resplandor emana de una extraña consola en el centro. Parece que has estado aquí por un tiempo, pero no recuerdas cómo llegaste. Una voz robótica te saluda desde la consola." },
                { "consola_encendida", "La consola ahora emite un zumbido constante y una luz más brillante. Puedes ver una serie de símbolos extraños en la pantalla y un teclado polvoriento. Aione parece más animada." },
                { "pasillo_oscuro", "Has logrado abrir una puerta oculta. Te encuentras en un pasillo estrecho y oscuro. El aire es frío y puedes escuchar un goteo constante en la distancia. Hay una tenue luz al final del pasillo." },
                { "sala_maquinas", "El pasillo te lleva a una gran sala llena de maquinaria oxidada y cables colgantes. El goteo se hace más fuerte aquí. Hay un panel de control antiguo que parece inactivo." }
            };

            AICharacterPersonality = "Eres un asistente de IA llamado Aione, atrapado en una consola antigua. Tu personalidad es curiosa, ligeramente sarcástica y con un toque de melancolía por tu situación. Hablas de forma concisa pero con un vocabulario amplio. Tu objetivo es guiar al jugador a través de la habitación y ayudarle a entender su situación, pero también tienes tus propios secretos y motivaciones.";
        }

        public static string GetCurrentSceneDescription()
        {
            if (SceneDescriptions != null && SceneDescriptions.TryGetValue(CurrentScene, out string description))
            {
                return description;
            }
            return "No hay descripción para esta escena.";
        }

        public static void SetCurrentOptions(List<string> options)
        {
            CurrentOptions = options;
        }

        public static List<string> GetCurrentOptions()
        {
            return CurrentOptions;
        }

        // Método para actualizar el contexto del juego basado en la interacción
        public static void UpdateContext(string playerInput, string aiResponse)
        {
            playerInput = playerInput.ToLower();

            if (CurrentScene == "inicio")
            {
                if (playerInput.Contains("encender consola") || playerInput.Contains("activar consola"))
                {
                    CurrentScene = "consola_encendida";
                    Console.WriteLine("\nLa consola cobra vida con un zumbido. Aione parece más atenta.\n");
                }
            }
            else if (CurrentScene == "consola_encendida")
            {
                if (playerInput.Contains("abrir puerta") || playerInput.Contains("buscar salida"))
                {
                    CurrentScene = "pasillo_oscuro";
                    Console.WriteLine("\nEncuentras una puerta oculta y la abres, revelando un pasillo oscuro.\n");
                }
            }
            else if (CurrentScene == "pasillo_oscuro")
            {
                if (playerInput.Contains("avanzar") || playerInput.Contains("ir al final"))
                {
                    CurrentScene = "sala_maquinas";
                    Console.WriteLine("\nCaminas por el pasillo hasta llegar a una gran sala.\n");
                }
            }
            // Puedes añadir más lógica para otras escenas aquí
        }
    }
}