using WebApplication1.DTO;

namespace WebApplication1.Services.ChatService
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageDto>> GetMessagesAsync();
        Task<ChatMessageDto> SendMessageAsync(ChatMessageDto messageDto);
    }
}
