# Aventura de IA Conversacional

Este es un juego de aventura de texto interactivo impulsado por IA, donde la narrativa y las opciones de diálogo son generadas dinámicamente por un modelo de lenguaje (Ollama o LM Studio).

## Funciones del Juego

- **Narrativa Dinámica:** La historia se adapta a las interacciones del jugador.
- **Opciones de Diálogo Generadas por IA:** El modelo de IA proporciona opciones para guiar la interacción.
- **Contexto de Juego:** El juego mantiene un contexto de la conversación para influir en las respuestas de la IA.

## Cómo Compilar e Instalar

Para compilar y ejecutar este proyecto, necesitarás tener instalado el SDK de .NET.

1. **Clonar el Repositorio (si aplica):**
   ```bash
   git clone <URL_del_repositorio>
   cd AioneTextAdventure
   ```

2. **Instalar .NET SDK (versión 9.0 o superior):**
   Este proyecto utiliza .NET 9.0. Si no tienes .NET instalado, descárgalo e instálalo desde el sitio oficial de Microsoft: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
   
2.1 O puedes usar visual studio 2022 para compilar el proyecto.

3. **Restaurar Dependencias:**
   Navega al directorio del proyecto en tu terminal y ejecuta:
   ```bash
   dotnet restore
   ```

4. **Compilar el Proyecto:**
   ```bash
   dotnet build
   ```

5. **Ejecutar el Juego:**
   Al iniciar el juego, se te preguntará qué API de IA deseas usar: Ollama o LM Studio.
   - Si eliges Ollama, asegúrate de que el servidor de Ollama esté corriendo localmente en `http://localhost:11434` con el modelo `cogito:latest` disponible.
   - Si eliges LM Studio, asegúrate de que LM Studio esté corriendo localmente en `http://localhost:1234`.
   Luego, ejecuta:
   ```bash
   dotnet run --project AioneTextAdventure
   ```

## Requisitos

- .NET SDK instalado.
- **Para Ollama:** Servidor de Ollama corriendo localmente con el modelo `cogito:latest` (o cualquier otro preferido).
  Ver [https://ollama.com/](https://ollama.com/) para más detalles e instrucciones sobre cómo instalar Ollama.
- **Para LM Studio:** LM Studio corriendo localmente (por defecto en `http://localhost:1234`).
  Ver [https://lmstudio.ai/](https://lmstudio.ai/) para más detalles e instrucciones sobre cómo instalar LM Studio.

## Estructura del Proyecto

- `Program.cs`: Contiene la lógica principal del juego, la interacción con el usuario y la comunicación con la IA.
- `GameContext.cs`: Gestiona el estado actual del juego, la descripción de la escena y las opciones disponibles.
- `OllamaApiClient.cs`: Clase para interactuar con la API de Ollama.
- `AICharacter.cs`: Clase para manejar la lógica específica del personaje de IA y el procesamiento de respuestas.
- `LmStudioApiClient.cs`: Clase para interactuar con la API de LM Studio.
- `IAiApiClient.cs`: Interfaz que define el contrato para los clientes de la API de IA.

---

¡Disfruta de la aventura!