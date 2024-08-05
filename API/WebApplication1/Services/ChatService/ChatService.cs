using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services.ChatService
{
    public class ChatService : IChatService
    {
        private readonly AppDb _context;
        private readonly IMapper _mapper;

        public ChatService(AppDb context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessagesAsync()
        {
            var messages = await _context.ChatMessages.ToListAsync();
            return _mapper.Map<IEnumerable<ChatMessageDto>>(messages);
        }

        public async Task<ChatMessageDto> SendMessageAsync(ChatMessageDto messageDto)
        {
            var message = _mapper.Map<ChatMessage>(messageDto);
            message.Timestamp = DateTime.UtcNow;
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return _mapper.Map<ChatMessageDto>(message);
        }
    }
}
