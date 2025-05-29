using System; 
using System.Collections.Generic; 
using System.Text.RegularExpressions; 
using System.Threading.Tasks; 

namespace AioneTextAdventure 
{ 
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
                if (Regex.IsMatch(line, @"^\d+\.\s*")) 
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