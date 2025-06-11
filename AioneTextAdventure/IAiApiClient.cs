using System.Threading.Tasks;

namespace AioneTextAdventure
{
    public interface IAiApiClient
    {
        Task<string> GetCompletionAsync(string prompt);
    }
}