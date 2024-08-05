using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.ChatHub;
using WebApplication1.DTO;
using WebApplication1.Services.ChatService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<Chat> _hubContext;

        public ChatController(IChatService chatService, IHubContext<Chat> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _chatService.GetMessagesAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatMessageDto messageDto)
        {
            var message = await _chatService.SendMessageAsync(messageDto);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.Sender, message.Text);
            return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
        }
    }
}
